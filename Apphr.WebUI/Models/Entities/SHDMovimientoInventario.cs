using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
	[Table("SHDMovimientosInventario")]
	public class SHDMovimientoInventario : AuditableEntity
	{
		[Key]                                           // MovimientoInventarioId (*)
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MovimientoInventarioId { get; set; }
		
		[ForeignKey("Administracion")]                  // AdministracionId (<<)
		public int AdministracionId { get; set; }
		public SHDAdministracion Administracion { get ; set; }

		[StringLength(3)]								// [TIPO]
		public string Tipo { get; set; }
				
		public int Documento { get; set; }              // [DOCUME]
				
		public int Correlativo { get; set; }			// [CORREL]

		[ForeignKey("Material")]                        // IdMaterial (<<)
		public int MaterialId { get; set; }
		public SHRMaterial Material { get; set; }

		[StringLength(14)]								// [CODMATE]
		public string MaterialCodigo { get; set; }		

		[StringLength(5)]								// [CODIGI]
		public string MaterialCodigi { get; set; }

		[StringLength(5)]						        // [CODBODE]		
		public string BodegaCodigo { get; set; }

		[ForeignKey("Bodega")]							// BodegaId
		public int BodegaId { get; set; }
		public SHDBodega Bodega { get; set; }

		[Display(Name = "Fecha Vencimiento")]           // string VENCE]
		[DataType(DataType.Date)]
		public DateTime? Vence { get; set; }


		public decimal Cantidad { get; set; }           // CANMOV]
		public decimal Valor { get; set; }				// VALMOV
		public string Observacion { get; set; }         // OBSMOV

		[Display(Name = "Fecha Creado")]				// [string DIA, string HORA]
		[DataType(DataType.Date)]
		public DateTime FechaCreado { get; set; }

		[Display(Name = "Fecha Movimiento")]			// [int DIAMOV, int MESMOV, int ANOMOV]
		[DataType(DataType.Date)]
		public DateTime FechaMovimiento { get; set; }

		public decimal CantidadAnterior { get; set; }   // [CANANT]

		public decimal ValorAnterior { get; set; }      // [VALANT]

		public string MARCA { get; set; }				// [MARCA]

		public string OPER { get; set; }				// [OPER]

		[StringLength(10)]								// Factura / Documento Ingreso
		public string DocumentoIngreso { get; set; }
		[StringLength(3)]								// Renglon
		public string Renglon { get; set; }
		[StringLength(10)]								// ProveedorNit
		public string ProveedorNit { get; set; }	
		
		[ForeignKey("Destino")]
		public int? DestinoId { get; set; }
		public SHRDestino Destino { get; set; }

		[StringLength(5)]								// Destino (Depto/Servicio)
		public string DestinoCodigo { get; set; }

		[StringLength(10)]
		public string Solicitud { get; set; }
	}
}