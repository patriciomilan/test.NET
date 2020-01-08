using System;
using System.Collections.Generic;
using System.Text;
using Test.Entidad.Core;

namespace Test.Entidad
{
    [Conexion("Test")]
    public class Persona : EntidadBase
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int RazonRegistroId { get; set; }

    }

    [Conexion("Test")]
    public class PersonaView : Persona
    {
        [DenegarInsert()]
        [DenegarUpdate()]
        public string RazonRegistro { get; set; }
    }
}
