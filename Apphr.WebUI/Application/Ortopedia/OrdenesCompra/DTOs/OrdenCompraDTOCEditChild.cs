using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOCEditChild : MovimientoDTOBase
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

        [Display(Name = "Nit")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }

        public string Mode { get; set; }
        //[Required]
        //[Display(Name = "Código")]
        //public int MaterialId { get; set; }
        //public Material Material { get; set; }


       


        
        //public decimal? Valor { get; set; }
        //public int SolicitudPedidoId { get; set; }
    }
}
