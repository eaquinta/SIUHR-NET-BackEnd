using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOCreate : IngresoInventarioDTOBase
    {
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

        public int ProveedorId { get; set; }
    }
}
