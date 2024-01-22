using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudMaterialesSala.DTOs
{
    public class SolicitudMaterialSalaDTOCEdit : SolicitudMaterialSalaDTOBase
    {
        public SolicitudMaterialSalaDTOCEdit()
        {
            Child = new SolicitudMaterialSalaDetalleDTOCEdit();
        }

        [Required]
        [Display(Name = "Registro médico")]
        [Remote("JsPacienteRMExist", "Paciente", "Ortopedia",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "PacienteRM_NotExist")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }

        public SolicitudMaterialSalaDetalleDTOCEdit Child { get; set; }
    }

    public class SolicitudMaterialSalaDetalleDTOCEdit : SolicitudMaterialSalaDetalleDTOBase
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
        [Display(Name = "Unidad de Medida")]
        public string MaterialUM { get; set; }

        
        [Display(Name = "Nit")]
        [Remote("JsProveedorNitExistOpt", "Proveedores", HttpMethod = "POST", ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
        public string ProveedorNit { get; set; }
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
       

    }
}
