
namespace Test.Entidad.Core
{
	public interface IEntidad
	{
		EstadoEntidad EstadoEntidad { get; set; }
		object Get(string propiedad);
		void Set(string propiedad, object valor);
		//IMetaEntidad Metadata();
	}
}
