using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("OrdenCompra")]
    public class OrdenCompra : AuditableEntity
    {
        [Key]
        [MaxLength(15)]
        public string OrdenCompraId { get; set; }

        [ForeignKey("Proveedor")]
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

        [ForeignKey("OrdenCompraId")]
        public ICollection<OrdenCompraDetalle> Detalle { get; set; }

    }

    [Table("OrdenCompraDetalle")]
    public class OrdenCompraDetalle : AuditableEntity
    {
        [Key]
        public int OrdenCompraDetalleId { get; set; }

        [MaxLength(15)]
        public string OrdenCompraId { get; set; }
        
        
        [ForeignKey("Material")]
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
