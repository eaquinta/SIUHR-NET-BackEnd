using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.InicialAbastecimiento.DTOs
{
    public class InicialAbastecimientoDTOCEdit : InicialAbastecimientoDTOBase
    {

        [Display(Name = "Código Material")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }
        [Display(Name = "Nombre Material")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Nit")]
        [Required]
        [Remote("JsProveedorNitExistOpt", "Proveedores",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Proveedor_NotExist")]
        public string ProveedorNit { get; set; }
        [Display(Name = "Nombre Proveedor")]
        public string ProveedorNombre { get; set; }
    }
}
