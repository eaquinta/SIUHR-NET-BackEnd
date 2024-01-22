using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlMovimientos.DTOs
{
    public class ControlMovimientoDTOIndex
    {
        [Display(Name = "Solicitud Pedido")]
        public string SolicitudPedidoId { get; set; }

        [Display(Name = "Fecha Solicitud")]
        public DateTime? Fecha { get; set; }
        public string SolicitudTipoCompra { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCodigo { get; set; }
        public string MaterialUnidadMedida { get; set; }
        public decimal PendienteIngreso { get; set; }
        public decimal Existencia { get; set; }

    }
}
