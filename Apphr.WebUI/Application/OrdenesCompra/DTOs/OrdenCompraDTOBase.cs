using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOBase
    {
        [MaxLength(15)]
        public string OrdenCompraId { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        [MaxLength(10)]
        public string Autorizacion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public decimal ValorFacturado { get; set; }
        public DateTime? FechaFacturado { get; set; }

        [MaxLength(10)]
        public string Status { get; set; }

        [MaxLength(15)]
        public string SolicitudPedidoId { get; set; }
    }

    public class OrdenCompraDetalleDTOBase
    {
        public int OrdenCompraDetalleId { get; set; }

        [MaxLength(15)]
        public string OrdenCompraId { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Valor { get; set; }
        public decimal? CantidadRecibido { get; set; }
        public decimal? ValorRecibido { get; set; }
        public string Observacion { get; set; }
    }
}
