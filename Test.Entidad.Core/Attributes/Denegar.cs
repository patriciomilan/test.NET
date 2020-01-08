using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entidad.Core
{

	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public class DenegarInsert : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public class DenegarUpdate : Attribute
	{

	}

}
