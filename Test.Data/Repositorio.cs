using Dapper;
using Test.Entidad.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Test.Data
{
    public class Repositorio<T>
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas
        {
            get
            {
                if (TotalRegistros == 0)
                {
                    return 0;
                }

                var paginacion = ObtenerPaginacion();
                if (paginacion == 0)
                {
                    return 0;
                }
                double resultado = double.Parse(TotalRegistros.ToString()) / double.Parse(paginacion.ToString());
                int parteEntera = int.Parse(Math.Truncate(resultado).ToString());
                bool paginaExtra = (resultado - parteEntera) > 0;
                if (paginaExtra)
                {
                    parteEntera++;
                }
                return parteEntera;
            }
        }

        public virtual Listado<T> Seleccionar(int pagina = 1)
        {
            string sql = ObtenerSQL();
            string sqlOrden = ObtenerOrden();

            if (ObtenerPaginacion() > 0)
            {
                sql = ObtenerSQLPaginacion(pagina, "");
                if (sqlOrden != string.Empty)
                    sql = sql.Replace("ORDER BY Id", sqlOrden);
            }
            else
            {
                if (sqlOrden != string.Empty)
                    sql = $" {sql} {sqlOrden}";
            }

            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                List<T> resultado = db.Query<T>(sql).ToList();
                if (ObtenerPaginacion() > 0)
                {
                    string sqlTotal = string.Format("select count(1) from {0}", Tabla());
                    TotalRegistros = db.Query<int>(sqlTotal).Single();
                }
                return new Listado<T>() { TotalRegistros = this.TotalRegistros, TotalPaginas = this.TotalPaginas, Paginacion = ObtenerPaginacion(), PaginaActual = pagina, Data = resultado };
            }
        }

        public virtual List<T> SeleccionSimple(string condicion, object parameters)
        {
            string sql = string.Format("Select * From {0}", Tabla());
            if (condicion != "")
                sql += string.Format(" where {0}", condicion);

            sql += ObtenerOrden();
            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                return db.Query<T>(sql, parameters).ToList();
            }
        }

        public virtual Listado<T> Seleccionar(string condicion, object parameters, int pagina = 1)
        {
            string sql;
            string sqlOrden = ObtenerOrden();
            if (ObtenerPaginacion() > 0)
            {
                sql = ObtenerSQLPaginacion(pagina, condicion);
                if (sqlOrden != string.Empty)
                    sql = sql.Replace("ORDER BY Id", sqlOrden);
            }
            else
            {
                sql = string.Format("Select * From {0}", Tabla());
                if (condicion != "")
                    sql += string.Format(" where {0}", condicion);

                if (sqlOrden != string.Empty)
                    sql += $" {sqlOrden}";
            }
            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                List<T> resultado = db.Query<T>(sql, parameters).ToList();
                if (ObtenerPaginacion() > 0)
                {
                    string sqlTotal;
                    if (!string.IsNullOrEmpty(condicion))
                        sqlTotal = string.Format("select count(1) from {0} where {1}", Tabla(), condicion);
                    else
                        sqlTotal = string.Format("select count(1) from {0}", Tabla());
                    TotalRegistros = db.Query<int>(sqlTotal, parameters).Single();
                }
                return new Listado<T>() { TotalRegistros = this.TotalRegistros, TotalPaginas = this.TotalPaginas, Paginacion = ObtenerPaginacion(), PaginaActual = pagina, Data = resultado };
            }
        }

        public string ObtenerSQL()
        {
            return string.Format("Select * From {0}", Tabla());
        }

        protected string ObtenerSQLPaginacion(int pagina, string condicion = "")
        {
            int paginaSize = ObtenerPaginacion();
            int inicio = 0, fin = 0;
            inicio = (pagina * paginaSize) - paginaSize + 1;
            fin = pagina * paginaSize;
            if (condicion != "")
            {
                condicion = $"where {condicion}";
            }
            string sql = "";
            sql += string.Format("SELECT * FROM (SELECT ROW_NUMBER() Over(ORDER BY Id) As RowNum, * from {0} {1})", Tabla(), condicion);
            sql += string.Format("as result where RowNum >= {0} AND RowNum <= {1} ORDER BY RowNum; ", inicio, fin);
            return sql;
        }

        protected int ObtenerPaginacion()
        {
            var paginacion = typeof(T).GetCustomAttributes(typeof(Paginacion), true).FirstOrDefault() as Paginacion;
            if (paginacion != null)
            {
                return paginacion.RegistrosPorPagina;
            }
            return 0;
        }

        public List<T> Ejecutar(string query, object parameters)
        {
            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                List<T> resultado = db.Query<T>(query, parameters).ToList();
                return resultado;
            }
        }

        private readonly string condicionUnica = "Id = @id";
        public T Find(int id)
        {
            return Seleccionar(condicionUnica, new { id }, 1).Data.FirstOrDefault();
        }

        public T Find(Guid id)
        {
            return Seleccionar(condicionUnica, new { id }, 1).Data.FirstOrDefault();
        }

        public virtual int Ejecutar(string sql, T entidad)
        {
            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                return db.Execute(sql, entidad);
            }
        }

        public virtual int Actualizar(T entidad)
        {
            PropertyInfo llave = ObtenerPropiedadLLave();
            string query = string.Format("Update {0} set ", Tabla());
            foreach (var propiedad in ObtenerPropiedadesActualizar())
            {
                if (propiedad.Name != llave.Name && propiedad.CanRead && propiedad.CanWrite)
                {
                    query += string.Format("{0} = @{0}, ", propiedad.Name);
                }
            }
            query = query.Substring(0, query.Length - 2);
            query += string.Format(" where {0} = @{0}", llave.Name);
            return Ejecutar(query, entidad);
        }

        public virtual void Insertar(T entidad)
        {
            string campos = string.Empty;
            string valores = string.Empty;
            string identityScope = "";
            PropertyInfo Llave = ObtenerPropiedadLLave();
            var propiedades = ObtenerPropiedadesInsertar();

            if (Llave.PropertyType.ToString() == "System.Guid")
            {
                if (Guid.Parse(Util.Get(entidad, Llave.Name).ToString()) == Guid.Empty)
                {
                    Util.Set(entidad, Llave.Name, Guid.NewGuid());
                }
                campos += string.Format("{0}, ", Llave.Name);
                valores += string.Format("@{0}, ", Llave.Name);

                identityScope = " select 0; ";
            }
            else
                identityScope = " select scope_Identity(); ";

            foreach (var propiedad in ObtenerPropiedadesInsertar())
            {
                if (propiedad.Name != Llave.Name && propiedad.CanRead && propiedad.CanWrite && EsPrimitiva(propiedad))
                {
                    campos += string.Format("{0}, ", propiedad.Name);
                    valores += string.Format("@{0}, ", propiedad.Name);
                }
            }

            campos = campos.Substring(0, campos.Length - 2);
            valores = valores.Substring(0, valores.Length - 2);
            string query = string.Format("Insert into {0}({1}) values ({2}); {3}", Tabla(), campos, valores, identityScope);
            using (IDbConnection db = DBConexion.Factory(ObtenerConexion()))
            {
                var identity = db.Query<int>(query, entidad).Single();
                if (identity > 0)
                {
                    Util.Set(entidad, "Id", identity);
                }
            }
        }

        public virtual int Eliminar(T entidad)
        {
            string query = string.Format("DELETE {0} where Id = @Id", Tabla());
            return Ejecutar(query, entidad);
        }

        public List<PropertyInfo> ObtenerPropiedadesActualizar()
        {
            Type t = typeof(T);
            List<PropertyInfo> propInfos = t.GetRuntimeProperties().ToList();
            List<PropertyInfo> propiedades = new List<PropertyInfo>();
            object[] atributos;
            bool agregarPropiedad = true;
            foreach (var propiedad in propInfos)
            {
                atributos = propiedad.GetCustomAttributes(true);
                agregarPropiedad = true;
                foreach (var customAtributo in atributos)
                {
                    var atributoCustom = customAtributo.ToString();
                    if (atributoCustom == "System.ComponentModel.ReadOnlyAttribute" || atributoCustom == "SX.Entidad.DenegarUpdate")
                    {
                        agregarPropiedad = false;
                    }

                }

                if (!EsPrimitiva(propiedad))
                    agregarPropiedad = false;

                if (EsLLave(propiedad))
                    agregarPropiedad = false;

                if (agregarPropiedad && propiedad.Name != "EstadoEntidad")
                    propiedades.Add(propiedad);
            }
            return propiedades;
        }

        public List<PropertyInfo> ObtenerPropiedadesInsertar()
        {
            Type t = typeof(T);
            List<PropertyInfo> propInfos = t.GetRuntimeProperties().ToList();
            List<PropertyInfo> propiedades = new List<PropertyInfo>();
            object[] atributos;
            bool agregarPropiedad = true;
            foreach (var propiedad in propInfos)
            {
                atributos = propiedad.GetCustomAttributes(true);
                agregarPropiedad = true;
                foreach (var customAtributo in atributos)
                {
                    if (customAtributo.ToString() == "System.ComponentModel.ReadOnlyAttribute")
                    {
                        agregarPropiedad = false;
                    }
                    if (customAtributo.ToString() == "SX.Entidad.DenegarInsert")
                    {
                        agregarPropiedad = false;
                    }
                }

                if (!EsPrimitiva(propiedad))
                    agregarPropiedad = false;

                if (EsLLave(propiedad) && (propiedad.PropertyType.ToString() == "System.Int32" || propiedad.PropertyType.ToString() == "System.Int64"))
                    agregarPropiedad = false;

                if (agregarPropiedad && propiedad.Name != "EstadoEntidad")
                    propiedades.Add(propiedad);
            }
            return propiedades;
        }

        public List<PropertyInfo> ObtenerPropiedades()
        {
            Type t = typeof(T);
            List<PropertyInfo> propInfos = t.GetRuntimeProperties().ToList();
            List<PropertyInfo> propiedades = new List<PropertyInfo>();
            bool agregarPropiedad = true;
            foreach (var propiedad in propInfos)
            {
                agregarPropiedad = true;
                if (!EsPrimitiva(propiedad))
                    agregarPropiedad = false;

                if (agregarPropiedad && propiedad.Name != "EstadoEntidad")
                    propiedades.Add(propiedad);
            }
            return propiedades;
        }

        protected bool EsPrimitiva(PropertyInfo propiedad)
        {
            string baseType = propiedad.PropertyType.BaseType.FullName;
            if (baseType == "System.Enum")
                return true;

            string tipo = propiedad.PropertyType.ToString();
            return (
                tipo == "System.String" ||
                tipo == "System.Int32" ||
                tipo == "System.Int64" ||
                tipo == "System.Double" ||
                tipo == "System.DateTime" ||
                tipo == "System.Decimal" ||
                tipo == "System.TimeSpan" ||
                tipo == "System.Guid" ||
                tipo == "System.Boolean"
                );
        }


        public PropertyInfo ObtenerPropiedadLLave()
        {
            List<PropertyInfo> propiedades = ObtenerPropiedades();
            foreach (var propiedad in propiedades)
            {
                if (EsLLave(propiedad))
                    return propiedad;
            }
            foreach (var propiedad in propiedades)
            {
                if (propiedad.Name == "Id")
                    return propiedad;
            }
            return null;
        }

        protected bool EsReadOnly(PropertyInfo propInfo)
        {
            return TieneDecorador(propInfo, "System.ComponentModel.ReadOnlyAttribute");
        }

        protected bool TieneDecorador(PropertyInfo propInfo, string decorador)
        {
            var attrs = propInfo.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (attr.ToString() == decorador)
                    return true;
            }
            return false;
        }

        protected string ObtenerConexion()
        {
            var conexion = typeof(T).GetCustomAttributes(typeof(Conexion), true).FirstOrDefault() as Conexion;
            if (conexion != null)
            {
                return conexion.NombreConexion;
            }
            return null;
        }

        protected string Tabla()
        {
            Type tipo = typeof(T);
            var decorador = typeof(T).GetCustomAttributes(typeof(NombreTabla), true).FirstOrDefault() as NombreTabla;
            if (decorador != null)
            {
                return decorador.Tabla;
            }
            string tabla = tipo.ToString();
            var nombres = tabla.Split('.');
            return nombres[nombres.Length - 1];
        }

        protected bool EsLLave(PropertyInfo propiedad)
        {
            var attrs = propiedad.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (attr.ToString() == "SX.Entidad.LLave")
                    return true;
            }
            if (propiedad.Name == "Id")
                return true;

            return false;
        }

        protected string ObtenerOrden()
        {
            object[] atributos;
            Type t = typeof(T);
            List<PropertyInfo> propiedades = t.GetRuntimeProperties().ToList();
            List<PropiedadOrden> propiedadesOrdenamiento = new List<PropiedadOrden>();
            foreach (var propiedad in propiedades)
            {
                if (propiedad.CanRead && propiedad.CanWrite)
                {
                    atributos = propiedad.GetCustomAttributes(true);
                    foreach (var customAtributo in atributos)
                    {
                        if (customAtributo.ToString() == "SX.Entidad.Orden")
                        {
                            var ordenInfo = customAtributo as Orden;
                            propiedadesOrdenamiento.Add(new PropiedadOrden
                            {
                                Nombre = propiedad.Name,
                                Direccion = ordenInfo.OrdenDireccion,
                                Prioridad = ordenInfo.Prioridad
                            });
                        }
                    }
                }
            }
            if (propiedadesOrdenamiento.Count == 0)
                return "";
            else
            {
                propiedadesOrdenamiento = propiedadesOrdenamiento.OrderBy(x => x.Prioridad).ToList();
                string ordenamiento = string.Empty;
                foreach (var item in propiedadesOrdenamiento)
                {
                    ordenamiento += $"{item.Nombre} {item.Direccion}, ";
                }
                ordenamiento = " Order By " + ordenamiento.Substring(0, ordenamiento.Length - 2);
                return ordenamiento;
            }
        }

        protected static readonly string ValidChars = "ÁáÉéíÍóÓúÚABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz1234567890*-";
        public string FormatearBusqueda(string buscar)
        {
            if (!string.IsNullOrEmpty(buscar))
            {
                var palabras = buscar.Trim().Split(' ');
                buscar = "";
                foreach (var palabra in palabras)
                {
                    string Salida = "";
                    for (int i = 0; i < palabra.Length; i++)
                    {
                        if (ValidChars.Contains(palabra[i]))
                        {
                            Salida += palabra[i].ToString();
                        }
                    }
                    buscar += string.Format("\"{0}\" ", Salida);
                }
                buscar = buscar.Trim().Replace(" ", " AND ");
            }
            return buscar;
        }


    }
}
