using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data
{
    internal class PropiedadOrden
    {
        public string Nombre { get; set; }
        public Test.Entidad.Core.Orden.Direccion Direccion { get; set; }
        public int Prioridad { get; set; }
    }
}
