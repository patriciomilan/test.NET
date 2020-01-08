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
    public class Evaluador
    {
        private readonly string conexion;
        public Evaluador(string nombreConexion)
        {
            conexion = nombreConexion;
        }
        public Evaluador()
        {
            conexion = "Metadata";
        }

        public bool Evalua(string expresionLogica)
        {
            string select = "select 1 where " + expresionLogica;
            using (IDbConnection db = DBConexion.Factory(conexion))
            {
                var resultado = db.Query<int>(select).ToList();
                return (resultado.Count > 0);
            }
        }
    }
}
