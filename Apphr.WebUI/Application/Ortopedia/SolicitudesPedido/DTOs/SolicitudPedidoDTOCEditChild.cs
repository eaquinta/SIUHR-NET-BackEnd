using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOCEditChild : MovimientoDTOBase
    {
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Código")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales", "Inventario",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        public string Mode { get; set; }
        //[Required]
        //[Display(Name = "Código")]
        //public int MaterialId { get; set; }
        //public Material Material { get; set; }


       


        
        //public decimal? Valor { get; set; }
        //public int SolicitudPedidoId { get; set; }
    }
}
