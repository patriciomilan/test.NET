using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entidad.Core
{
    public class Paginacion : Attribute
	{
		public Paginacion(int registrosPorPagina)
		{
			RegistrosPorPagina = registrosPorPagina;
		}

		public int RegistrosPorPagina { get; set; }
	}
}
