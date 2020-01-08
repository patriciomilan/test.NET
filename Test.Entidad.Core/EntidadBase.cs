using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Test.Entidad.Core
{
    public abstract class EntidadBase : IEntidad
    {
        public EntidadBase()
        {
        }

        public object Get(string propiedad)
        {
            return Util.Get((object)this, propiedad);
        }

        public void Set(string propiedad, object valor)
        {
            Util.Set((object)this, propiedad, valor);
        }

        EstadoEntidad IEntidad.EstadoEntidad { get; set; }

        protected string ObtenerUltimoString(string input, char separador = '.')
        {
            if (input == null)
                return null;
            string[] arreglo = input.Split(separador);
            if (arreglo.Length == 0)
                return input;

            return arreglo[arreglo.Length - 1];
        }

        public List<PropertyInfo> ObtenerPropiedades()
        {
            return Util.ObtenerPropiedades(this);
        }
        private string ObtenerAssemblyName(string clase)
        {
            string[] arreglo = clase.Split('.');
            string assembly = string.Empty;
            if (arreglo.Length == 1)
                return clase;
            for (int i = 0; i < arreglo.Length - 1; i++)
            {
                assembly += string.Format("{0}.", arreglo[i]);
            }
            return assembly.Substring(0, assembly.Length - 1);
        }

    }

	public class EntidadActualizable : EntidadBase
	{
		public virtual EstadoEntidad EstadoEntidad { get; set; }
	}
}
