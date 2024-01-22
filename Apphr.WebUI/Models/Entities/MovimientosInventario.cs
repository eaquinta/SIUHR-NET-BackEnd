using Apphr.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
	[Table("MovimientosInvnetario")]
    public class MovimientoInventario
	{
        public MovimientoInventario()
        {
			//MovimientoInventarioId =  Guid.NewGuid();
        }


		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64 MovimientoInventarioId { get; set; }
		
		public int Efecto { get; set; }
		[MaxLength(25)]
		public string Documento { get; set; }
		[MaxLength(25)]
		public string DocumentoReferencia { get; set; }
		public int Line { get; set; }
		[ForeignKey("Bodega")]
		public int BodegaId { get; set; }
		public Bodega Bodega { get; set; }
		[ForeignKey("Material")]
		public int MaterialId { get; set; }
		public Material Material { get; set; }
		public DateTime? FechaVencimiento { get; set; }
		[ForeignKey("Proveedor")]
		public int? ProveedorId { get; set; }
		public Proveedor Proveedor { get; set; }
		[ForeignKey("Paciente")]
		public int? PacienteId { get; set; }
		public Paciente Paciente { get; set; }
		[MaxLength(20)]
		public string Lote { get; set; }		
		public DateTime Fecha { get; set; }		
		public decimal Cantidad { get; set; }
		public decimal Valor { get; set; }
		public decimal Precio { get; set; }
        public string Observacion { get; set; }

        [ForeignKey("TipoMovimientoInventario")]
        public int TipoMovimientoInventarioId { get; set; }
        public TipoMovimientoInventario TipoMovimientoInventario { get; set; }
        [ForeignKey("Destino")]
        public int? DestinoId { get; set; }
        public Destino Destino { get; set; }
        //public string OrdenOperacion { get; set; }
    }
}

//[Column(TypeName = "bigint")]
//public decimal DIAMOV{ get; set; }
//public decimal MESMOV { get; set; }
//public decimal ANOMOV { get; set; }
//public string Vence { get; set; }
//public string DIA { get; set; }
//public string HORA { get; set; }
//public decimal CANANT { get; set; }
//public decimal VALANT { get; set; }
//public string MARCA { get; set; }
//public string OPER { get; set; }		
//public string Documento { get; set; }
//public string CodigoMaterial { get; set; }
