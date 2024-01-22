using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Materiales")]
	public class Material : AuditableEntity
	{
        [Key]                                                       // MaterialId (*)
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]				
		public int MaterialId { get; set; }

		[StringLength(14), MinLength(6)]                            // Codigo [CODIGO]
		[Index("IX_Material", Order = 1, IsUnique = true)]
		public string Codigo { get; set; }

		[StringLength(5)]                                           // [CODIGI]
		[Index("IX_Material", Order = 2, IsUnique = true)]
		public string Codigi { get; set; }


		[StringLength(254)]                                         // [DESCRI]
		public string Descripcion { get; set; }

		[StringLength(10)]                                          // [UNIMED]
		public string UnidadMedida { get; set; }

		[StringLength(30)]                                          // [PRODUC]
		public string Producto { get; set; }

		[Required]                                                  // [PRECIO]
		public decimal Precio { get; set; }


		public decimal? Minimo { get; set; }                        // [MINIMO]

		public decimal? Maximo { get; set; }                        // [MAXIMO]

		[StringLength(1)]                                           // [OJO]
		public string Ojo { get; set; }
		//[Column("OJO")]
		//[Display(Name = "Tipo Contrato")]
		public string TipoContrato { get; set; }

		[StringLength(14)]                                          // [ALTERNODE]
		public string AlternoDe { get; set; }

		[StringLength(1)]                                           // [BRES]
		public string Bres { get; set; }

		public DateTime FechaCreacion { get; set; } = DateTime.Now.Date;                // [FECHA, HORA]


		[StringLength(3)]                                           // [USUA]
		public string Usuario { get; set; }

		[StringLength(1)]                                           // [STATUS]
		public string Estatus { get; set; }

																	// [Vigeda, VIGEDM, VIGEDD]
		public DateTime? VigenciaDe { get; set; }
																	// [VIGEAA, VIGEAM, VIGEAD]
		public DateTime? VigenciaA { get; set; }

		[StringLength(5)]                                           // [USOBOD]
		public string UsoBodega { get; set; }

		[StringLength(5)]                                           // [USOSER]
		public string UsoServicio { get; set; }

		[StringLength(20)]											// SigesCodigo		
		public string SigesCodigo { get; set; } = "-ND-";

		[StringLength(20)]                                          // RenglonCodigo
		public string RenglonCodigo { get; set; }

		[StringLength(20)]
		public string GrupoCodigo { get; set; }						// GrupoCodigo
		public void SetRenglon()
        {
			RenglonCodigo = Codigo.Substring(0, 3);
		} 
		public void SetGrupo()
        {
			GrupoCodigo = Codigo.Substring(3, 3);
		}
		//public string TipoEventoText { get { return Apphr.Application.Common.mo Apphr.Application.Common.Models. } }
	}
}
