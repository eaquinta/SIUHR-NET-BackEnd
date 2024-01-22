

// Apphr.Application.Reports.Inventario.ReporteExistencias
namespace Apphr.Application.Reports.Inventario
{
    public class ReporteExistencias
    {
		public string Codigo { get; set; }
		public string CODIGI { get; set; }
		public string Descripcion { get; set; }
		public string CodigoBodega { get; set; }
		public decimal Existencia { get; set; }
		public decimal Total { get; set; }
		public decimal CostoUnitario { get; set; }
		public string UnidadMedida { get; set; }
	}
}
