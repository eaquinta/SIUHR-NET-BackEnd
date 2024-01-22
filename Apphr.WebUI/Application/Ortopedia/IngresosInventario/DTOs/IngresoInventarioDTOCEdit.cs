using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOCEdit : IngresoInventarioDTOBase
    {
        public Int64 SolicitudPedidoId { get; set; }
       
        [Display(Name = "Orden Compra #")]
        [Required]
        //[Remote("JsOrdenCompraExist", "SolicitudPedido", "Ortopedia",
        //    HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(Resources.Msg),
        //    ErrorMessageResourceName = "OrdenCompra_NotExist")]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }

        [Display(Name = "Solicitud Pedido #")]
        [Required]
        public int NumeroSP { get; set; }

        [Display(Name = "Fecha S.P.")]
        [DataType(DataType.Date)]
        public DateTime FechaSP { get; set; }

        [Display(Name = "Año S.P.")]
        public int AnioSP { get; set; }

        public int ProveedorId { get; set; }
    }
}
