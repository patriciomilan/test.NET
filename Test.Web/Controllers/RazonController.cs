using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RazonController : ControllerBase
    {
        public Listado<Razon> Get()
        {
            var repositorioPersona = new Repositorio<Razon>();
            return repositorioPersona.Seleccionar();
        }
    }
}