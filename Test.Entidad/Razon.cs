using System;
using System.Collections.Generic;
using System.Text;
using Test.Entidad.Core;

namespace Test.Entidad
{
    [Conexion("Test")]
    public class Razon: EntidadBase
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
