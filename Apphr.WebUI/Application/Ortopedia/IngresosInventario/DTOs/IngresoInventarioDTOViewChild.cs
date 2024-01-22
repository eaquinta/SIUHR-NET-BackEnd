using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOViewChild : MovimientoDTOBase
    {
        [Display(Name = "Bodega")]
        public string BodegaNombre { get; set; }

        [Display(Name = "Descripción")]
        public string BodegaDescripcion { get; set; }

        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
    }
}
