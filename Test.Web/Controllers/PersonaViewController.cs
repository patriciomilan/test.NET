using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Data;
using Test.Entidad;
using Test.Entidad.Core;

namespace Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaViewController : ControllerBase
    {
        [HttpGet]
        public Listado<PersonaView> Index()
        {
            var repositorioPersona = new Repositorio<PersonaView>();
            return repositorioPersona.Seleccionar();
        }

        //[HttpGet("exportar")]
        //public MemoryStream Exportar() 
        //{
        //    return GetStream(ObtenerCSB());
        //}

        [HttpGet("exportar")]
        public ActionResult Exportar()
        {
            return File(GetStream(ObtenerCSB()), "text/plain", "Personas.csv");
        }
        private string ObtenerCSB()
        {
            var repositorioPersona = new Repositorio<PersonaView>();
            StringBuilder sb = new StringBuilder();
            PersonaView personaView = new PersonaView();
            string[] propiedades = { "Id", "Nombre", "Apellido", "RazonRegistroId", "RazonRegistro" };
            foreach (var propiedad in propiedades)
            {
                // Nombres de Columnas
                sb.Append($"\"{propiedad}\";");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();
            var personas = repositorioPersona.Seleccionar().Data;
            object valor;
            foreach (var persona in personas)
            {
                foreach (var propiedad in propiedades)
                {
                    // Valores de los campos
                    valor = persona.Get(propiedad);
                    if (valor.GetType().ToString() == "System.String" || valor.GetType().ToString() == "System.Guid")
                        sb.Append($"\"{valor}\";");
                    else
                        sb.Append($"{valor};");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private MemoryStream GetStream(string texto)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(texto);
            return new MemoryStream(byteArray);
        }
    }
}