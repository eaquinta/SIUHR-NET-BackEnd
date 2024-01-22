using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTMovimientos")]
    public class ORTMovimiento : AuditableEntity
    {
        [Key]                                                           // ORTMovimientoId
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 MovimientoId { get; set; }

        [StringLength(3)]                                               // Tipo [APE, ING, EGR, AJU, DEV, SOL, ORD]
        public string Tipo { get; set; }

        public Int64 Documento { get; set; }                            // Documento

        [MaxLength(25)]                                                 // DocumentoReferencia
        public string DocumentoReferencia { get; set; }

        public int Linea { get; set; }                                  // Linea

        [ForeignKey("SolicitudPedido")]                                 // SolicitudPedidoId
        public Int64? SolicitudPedidoId { get; set; }
        public ORTSolicitudPedido SolicitudPedido { get; set; }

        [ForeignKey("OrdenCompra")]                                     // OrdenCompraId
        public Int64? OrdenCompraId { get; set; }
        public ORTOrdenCompra OrdenCompra { get; set; }

        [ForeignKey("Paciente")]                                        // PacienteId
        public Int64? PacienteId { get; set; }
        public ORTPaciente Paciente { get; set; }

        [ForeignKey("Cirujano")]                                        // CirujanoId
        public int? CirujanoId { get; set; }
        public ORTCirujano Cirujano { get; set; }

        [ForeignKey("Material")]                                        // MaterialId
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        [ForeignKey("Proveedor")]                                       // ProveedorId
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        [ForeignKey("Bodega")]                                          // BodegaId
        public int? BodegaId { get; set; }
        public Bodega Bodega { get; set; }

        public Decimal Cantidad { get; set; }                           // Cantidad

        public Decimal Precio { get; set; }                             // Precio

        public Decimal Valor { get; set; }                              // Valor
        
        [DataType(DataType.Date)]                                       // Fecha
        public DateTime Fecha { get; set; }

        public int? NoHojaControl { get; set; }                         // Hoja de Contorl

        [ForeignKey("Devolucion")]
        public Int64? DevolucionId { get; set; }                        // DevolucionId

        public ORTDevolucionInventario Devolucion { get; set; }

        [ForeignKey("HojaGasto")]                                       // HojaGasto
        public Int64? HojaGastoId { get; set; }
        public ORTHojaGasto HojaGasto { get; set; }

        public bool Devolver { get; set; }

        public bool Devuleto { get; set; }

        [ForeignKey("Servicio")]
        public int? ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}
