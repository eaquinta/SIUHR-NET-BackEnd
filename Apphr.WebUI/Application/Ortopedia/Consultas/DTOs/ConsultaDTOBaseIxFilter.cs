using Apphr.Application.Common.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultaDTOBaseIxFilter : DTOJsIxFilter
    {
        [UIHint("DropDown")]
        [Display(Name = "Proveedor")]

        public int? ProveedorId { get; set; }

        [UIHint("DropDown")]
        [Display(Name = "Material")]
        public int? MaterialId { get; set; }

        [Display(Name = "Nombre Material")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Nombre Proveedor")]
        public string ProveedorNombre { get; set; }

    }
}
