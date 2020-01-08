using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Test.Entidad.Core;

namespace Test.Win
{
    public class Repositorio<T> : Proxy
    {
        public string Controlador { get; set; }
        public Repositorio() : base()
        {
            var arreglo = typeof(T).ToString().Split('.');
            Controlador = arreglo[arreglo.Length - 1];
        }

        public static Repositorio<T> Obtener()
        {
            return new Repositorio<T>();
        }


        public T Get(int id)
        {
            return ObtenerId(id);
        }
        public T Get(Guid id)
        {
            return ObtenerId(id);
        }
        public T Get(string id)
        {
            return ObtenerId(id);
        }

        protected T ObtenerId(object id)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Sufijo() + id.ToString(), Metodo.GET);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T Insertar(T entidad)
        {
            var json = JsonConvert.SerializeObject(entidad);
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + "/", Metodo.POST, json);
            });
            var jsonResult = tarea.Result;
            if (Respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(jsonResult);
            else
                return default(T);

        }

        public T EnviarPost(string accion, object objeto)
        {
            var tarea = Task.Run(async () =>
            {
                string jsonEntrada = JsonConvert.SerializeObject(objeto);
                return await ObtenerRespuesta(Sufijo() + accion, Metodo.POST, jsonEntrada);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T Actualizar(T entidad)
        {
            var json = JsonConvert.SerializeObject(entidad);
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + "/", Metodo.PUT, json);
            });
            var jsonResult = tarea.Result;
            if (Respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(jsonResult);
            else
                return default(T);
        }

        public void Eliminar(T entidad)
        {
            var id = Util.Get(entidad, "Id").ToString();
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + "/" + id, Metodo.DELETE);
            });
            tarea.Wait();
        }

        public Listado<T> Seleccionar()
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + "/", Metodo.GET);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<Listado<T>>(json);
        }

        public Listado<T> Seleccionar(int pagina)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + $"/Listar/{pagina}", Metodo.GET);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<Listado<T>>(json);
        }

        public List<T> Seleccionar(string accion, Metodo metodo)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Controlador + "/" + accion, metodo);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public List<T> Seleccionar(string accion, Metodo metodo, object objeto)
        {
            var tarea = Task.Run(async () =>
            {
                string jsonEntrada = JsonConvert.SerializeObject(objeto);
                return await ObtenerRespuesta(Controlador + "/" + accion, metodo, jsonEntrada);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        public Listado<T> Seleccionar(string accion, Metodo metodo, object objeto, int pagina)
        {
            var tarea = Task.Run(async () =>
            {
                string jsonEntrada = JsonConvert.SerializeObject(objeto);
                return await ObtenerRespuesta(Controlador + "/" + accion + $"/{(pagina == 0 ? 1 : pagina)}", metodo, jsonEntrada);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<Listado<T>>(json);
        }

        public TEntity Obtener<TEntity>(string accion, Metodo metodo, string json)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Sufijo() + accion, metodo, json);
            });
            string jsonResult = tarea.Result;
            return JsonConvert.DeserializeObject<TEntity>(jsonResult);
        }

        public Stream ObtenerArchivo(string accion, Metodo metodo, string json)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuestaArchivo(Sufijo() + accion, metodo, json);
            });
            var memoriaArchivo = tarea.Result;
            memoriaArchivo.Seek(0, SeekOrigin.Begin);
            return memoriaArchivo;
        }

        public string Obtener(string accion, Metodo metodo, string json)
        {
            var tarea = Task.Run(async () =>
            {
                return await ObtenerRespuesta(Sufijo() + accion, metodo, json);
            });
            return tarea.Result;
        }

        public Listado<T> Buscar(string texto)
        {
            var tarea = Task.Run(async () =>
            {
                texto = System.Uri.EscapeDataString(texto);
                return await ObtenerRespuesta(Controlador + $"/Buscar/{texto}", Metodo.GET);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<Listado<T>>(json);
        }

        public Listado<T> Buscar(string texto, int pagina)
        {
            var tarea = Task.Run(async () =>
            {
                texto = System.Uri.EscapeDataString(texto);
                return await ObtenerRespuesta(Controlador + $"/Buscar/{texto}/{pagina}", Metodo.GET);
            });
            var json = tarea.Result;
            return JsonConvert.DeserializeObject<Listado<T>>(json);
        }

        protected string Sufijo()
        {
            if (Controlador != string.Empty)
                return Controlador + "/";
            return string.Empty;
        }
    }
}

