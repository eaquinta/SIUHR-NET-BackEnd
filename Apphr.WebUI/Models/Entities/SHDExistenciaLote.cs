using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
	[Table("SHDExistenciasLote")]
	public class SHDExistenciaLote : AuditableEntity
	{
		[Key]															// Id (*)
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		
		[Index("IX_ExistenciaLote", Order = 1, IsUnique = true)]        // AdministracionId (<<)
		[ForeignKey("Administracion")]
		public int AdministracionId { get; set; }
		public SHDAdministracion Administracion { get; set; }

		[ForeignKey("Material")]										// IdMaterial (<<)
		public int IdMaterial { get; set; }
		public SHRMaterial Material { get; set; }

		[StringLength(14)]												// [CODMATE]
		[Index("IX_ExistenciaLote", Order = 2, IsUnique = true)]
		public string CodigoMaterial { get; set; }

		[StringLength(5)]												// [CODIGI]
		[Index("IX_ExistenciaLote", Order = 3, IsUnique = true)]
		public string CodigiMaterial { get; set; }

		

		[StringLength(5)]												// [CODBODE]
		[Index("IX_ExistenciaLote", Order = 4, IsUnique = true)]
		public string CodigoBodega { get; set; }
	
		[Display(Name = "Fecha Vencimiento")]							// [string VENCE]
		[DataType(DataType.Date)]
		[Index("IX_ExistenciaLote", Order = 5, IsUnique = true)]
		public DateTime? Vence { get; set; }

		[StringLength(7)]												// [TRAJE]
		public string TARJE { get; set; }
		public decimal Cantidad { get; set; }							// [CANTI]
		public decimal Valor { get; set; }								// [VALOR]
		public decimal UltimoPrecio { get; set; }						// [ULTPRE]

		[StringLength(15)]												// [LOTE]
		public string Lote { get; set; }
	}
}
