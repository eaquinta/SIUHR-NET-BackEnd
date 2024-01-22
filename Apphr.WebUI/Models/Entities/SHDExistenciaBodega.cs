using Apphr.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
	[Table("SHDExistenciaBodega")]
	public class SHDExistenciaBodega : AuditableEntity
	{
		[Key]															// Id (*)
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Index("IX_ExistenciaBodega", Order = 1, IsUnique = true)]		// AdministracionId (<<)
		[ForeignKey("Administracion")]
		public int AdministracionId { get; set; }
		public SHDAdministracion Administracion { get; set; }

		[ForeignKey("Material")]										// MaterialId (<<)
		public int MaterialId { get; set; }
		public SHRMaterial Material { get; set; }

		[StringLength(14)]												// [CODIGO]
		[Index("IX_ExistenciaBodega", Order = 2, IsUnique = true)]
		public string CodigoMaterial { get; set; }


		[StringLength(5)]												// [CODIGI]
		[Index("IX_ExistenciaBodega", Order = 3, IsUnique = true)]
		public string CodigiMaterial { get; set; }

		[ForeignKey("Bodega")]											// BodegaId (<<)
		public int BodegaId { get; set; }
		public SHDBodega Bodega { get; set; }
				
		[StringLength(5)]												// [CODBODE]
		[Index("IX_ExistenciaBodega", Order = 4, IsUnique = true)]
		public string CodigoBodega { get; set; }

		public decimal Cantidad { get; set; }							// [CANTI]

		public decimal Valor { get; set; }								// [VALOR]
		public decimal Minimo  { get; set; }							// [MINIMO]
		public decimal Maximo { get; set; }								// [MAXIMO]
		public decimal STOCK { get; set; }								// [STOCK]
		public decimal CANSER { get; set; }								// [CANSER]
		public decimal VALSER { get; set; }								// [VALSER]
		public string BODSER { get; set; }								// [BODSER]
	}
}