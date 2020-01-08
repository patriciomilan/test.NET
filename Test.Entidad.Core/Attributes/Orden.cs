using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entidad.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class Orden : Attribute
    {
        public enum Direccion
        {
            ASC,
            DESC
        }
        public Orden(Direccion direccion, int prioridad = 0)
        {
            OrdenDireccion = direccion;
            Prioridad = prioridad;
        }
        public Direccion OrdenDireccion { get; set; }
        public int Prioridad { get; set; }
    }
}
