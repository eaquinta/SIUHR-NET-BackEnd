using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultasDTOSaldoGridProveedor : MovimientoDTOBase
    {
        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Nit")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
    }
}
