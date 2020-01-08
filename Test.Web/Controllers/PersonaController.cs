using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Data;
using Test.Entidad;

namespace Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private Repositorio<Persona> repositorio;
        public PersonaController()
        {
            repositorio = new Repositorio<Persona>();
        }

        [HttpPost]
        public void Post(Persona persona)
        {
            repositorio.Insertar(persona);
        }

        [HttpPut]
        public void Put(Persona persona)
        {
            repositorio.Actualizar(persona);
        }
    }
}