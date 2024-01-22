using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOViewChild : MovimientoDTOBase
    {
        [Display(Name = "Material Nombre")]                             // MaterialNombre
        public string MaterialNombre { get; set; }

        [Display(Name = "Código")]                                      // MaterialCodigo
        public string MaterialCodigo { get; set; }

        [Display(Name = "Proveedor Nit")]                               // ProveedorNit
        public string ProveedorNit { get; set; }
        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }
    }
}
