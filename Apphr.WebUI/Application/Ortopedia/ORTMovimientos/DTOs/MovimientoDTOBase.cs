using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models.Entities.Ortopedia;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.ORTMovimientos.DTOs
{
    public class MovimientoDTOBase
    {        
        public Int64 MovimientoId { get; set; }                         // MovimientoId

        [MaxLength(3)]                                                  // Tipo [SOL, ORD, ING, EGR, AJU, DEV] APE,
        public string Tipo { get; set; }

        //[MaxLength(25)]                                                 // Documento
        public Int64 Documento { get; set; }

        [MaxLength(25)]                                                 // DocumentoReferencia
        public string DocumentoReferencia { get; set; }

        public int Linea { get; set; }                                  // Linea
                
        public Int64? SolicitudPedidoId { get; set; }                   // SolicitudPedidoId
        public SolicitudPedido SolicitudPedido { get; set; }
        
        public Int64? OrdenCompraId { get; set; }                         // OrdenCompraId
        public SolicitudPedido OrdenCompra { get; set; }

        public Int64? PacienteId { get; set; }                            // PacienteId
        public ORTPaciente Paciente { get; set; }
                
        public int? CirujanoId { get; set; }                            // CirujanoId
        public ORTCirujano Cirujano { get; set; }

        public int MaterialId { get; set; }                            // MaterialId
        public Material Material { get; set; }
        
        public int? ProveedorId { get; set; }                          // ProveedorId
        public Proveedor Proveedor { get; set; }

        public Int64? DevolucionId { get; set; }                        // DevolucionId
        public ORTDevolucionInventario Devolucion { get; set; }
        
        [Required]                                                      // Cantidad
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Cantidad { get; set; }

        [Required]                                                      // Precio
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\$?\d+(\.(\d{0,4}))?$", ErrorMessage = "El campo {0} debe tener máximo 4 decimales")]
        public decimal Precio { get; set; }

        public decimal Valor { get; set; }                              // Valor

        [DataType(DataType.Date)]                                       // Fecha
        public DateTime Fecha { get; set; }

        [UIHint("Switch")]
        public bool Devolver { get; set; } = false;

        [UIHint("Switch")]
        public bool Devuelto { get; set; } = false; 
    }
}
