using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOCEdit : SolicitudPedidoDTOBase
    {
        public SolicitudPedidoDTOCEdit()
        {
            Child = new SolicitudPedidoDTDTOCEdit();
        }

        public string year { get; set; }


        [Required, MaxLength(6)]
        [Remote("IsSolicitudPedidoIdAvailable", "SolicitudesPedido", AdditionalFields = "SolicitudPedidoId,year", HttpMethod = "POST", ErrorMessage = "Esta solicitud ya se encuentra registrada")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Solicitud debe ser un número.")]
        public string serie { get; set; }

        [Display(Name = "Departamento/Servicio")]
        [Required]
        [Remote("JsCodigoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Este departamento no se ecuentra registrado")]
        public string DepartamentoCodigo { get; set; }


        [Display(Name = "Departamento/Servicio Nombre")]
        public string DepartamentoNombre { get; set; }


        [Display(Name = "Código Sección")]
        //[Required]
        [Remote("JsCodigoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Esta sección no se ecuentra registrada")]
        public string SeccionCodigo { get; set; }


        [Display(Name = "Sección Nombre")]
        public string SeccionNombre { get; set; }

        public SolicitudPedidoDTDTOCEdit Child { get; set; }
    }

    public class SolicitudPedidoDTDTOCEdit : SolicitudPedidoDetalleDTOBase
    {
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }        

        [Display(Name = "Código")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }
    }
}
