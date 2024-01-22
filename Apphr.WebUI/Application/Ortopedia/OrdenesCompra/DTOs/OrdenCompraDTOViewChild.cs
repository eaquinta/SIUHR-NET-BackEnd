using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOViewChild : MovimientoDTOBase
    {
        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
        
        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        [Display(Name = "Nit")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }
    }
}
