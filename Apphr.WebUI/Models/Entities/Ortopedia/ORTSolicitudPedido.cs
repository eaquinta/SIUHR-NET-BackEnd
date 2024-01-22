using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTSolicitudesPedido")]
    public class ORTSolicitudPedido : AuditableEntity
    {
        [Key]                                                                   // SolicitudPedidoId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 SolicitudPedidoId { get; set; }

        //[ForeignKey("OrdenCompra")]                                             // OrdenCompraId
        public Int64? OrdenCompraId { get; set; }
        //public ORTOrdenCompra OrdenCompra { get; set; }

        public int? NumeroOC { get; set; }                                      // NumeroOC

        public DateTime? FechaOC { get; set; }                                  // FechaOC
                
        [Index("IX_Numero", Order =1, IsUnique = true)]                         // Numero
        public int Numero { get; set; }

        [Index("IX_Numero", Order = 2, IsUnique = true)]                        // Anio 
        public int Anio { get; set; }

        public DateTime Fecha { get; set; }                                     // Fecha

        public int Lineas { get; set; }                                         // Lineas

        [StringLength(3)]                                                       // Tipo
        public string TipoPrioridad { get; set; }

        [StringLength(2)]                                                       // TipoEvento
        public string TipoEvento { get; set; }

        [StringLength(100)]                                                     // Solicito
        public string Solicito { get; set; }

        [StringLength(100)]
        public string Elaboro { get; set; }

        [StringLength(100)]
        public string JefeDepartamento { get; set; }

        [StringLength(100)]
        public string Gerente { get; set; }

        [StringLength(100)]
        public string Director { get; set; }

        public string Observacion { get; set; }
    }
}
