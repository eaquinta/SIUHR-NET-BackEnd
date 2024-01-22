using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTOrdenesCompra")]
    public class ORTOrdenCompra : AuditableEntity
    {
        [Key]                                                                   // OrdenCompraId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 OrdenCompraId { get; set; }

        [Index("IX_Numero", Order = 1, IsUnique = true)]                        // Numero
        public int Numero { get; set; }

        [Index("IX_Numero", Order = 2, IsUnique = true)]                        // Anio 
        public int Anio { get; set; }

        public DateTime Fecha { get; set; }                                     // Fecha

        public int Lineas { get; set; }                                         // Lineas

        public string Observacion { get; set; }                                 // Observacion

        [ForeignKey("SolicitudPedido")]                                         // SolicitudPedidoId
        public Int64? SolicitudPedidoId { get; set; }
        public ORTSolicitudPedido SolicitudPedido { get; set; }

        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
    }

    //[Table("ORTOrdenesCompraDetalle")]
    //public class ORTOrdenCompraDetalle
    //{
    //}
}
