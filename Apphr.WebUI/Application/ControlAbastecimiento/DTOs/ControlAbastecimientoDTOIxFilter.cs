using Apphr.Application.Common.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOIxFilter : DTOJsIxFilter
    {
        [Display(Name = "Código Material")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
           HttpMethod = "POST",
           ErrorMessageResourceType = typeof(Resources.Msg),
           ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
        public int MaterialId { get; set; }
    }
}
