using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTOCEdit : SolicitudDespachoDTOBase
    {
        public SolicitudDespachoDTOCEdit() : base()
        {
            Child = new SolicitudDespachoChildDTOCEdit();
        }

        public string year { get; set; }


        [Required, MaxLength(6)]
        [Remote("JsSolicitudAvailable", "SolicitudesDespacho", AdditionalFields = "SolicitudDespachoId, year", HttpMethod = "POST", ErrorMessage = "Esta solicitud ya se encuentra registrada")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Solicitud debe ser un número.")]
        public string serie { get; set; }


        [Display(Name = "Código Departamento")]
        [Required]
        [Remote("JsCodigoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Este departamento no se ecuentra registrado")]
        public string DepartamentoCodigo { get; set; }


        [Display(Name = "Departamento Nombre")]
        public string DepartamentoNombre { get; set; }


        [Display(Name = "Código Sección")]
        [Required]
        [Remote("JsCodigoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Esta sección no se ecuentra registrada")]        
        public string SeccionCodigo { get; set; }


        [Display(Name = "Sección Nombre")]
        public string SeccionNombre { get; set; }


        public SolicitudDespachoChildDTOCEdit Child { get; set; }


        [Display(Name = "Despacho No. ")]
        public string DespachoInventarioId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }




    }

    public class SolicitudDespachoChildDTOCEdit : SolicitudDespachoDTDTOBase
    {
        [Display(Name = "Código")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }
        
        
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\$?\d+(\.(\d{0,4}))?$", ErrorMessage = "El campo {0} debe tener máximo 4 decimales")]        
        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }


        [Display(Name = "Unidad Medida")]
        public string UnidadMedida { get; set; }
    }
}
