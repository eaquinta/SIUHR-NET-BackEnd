using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Materiales.DTOs
{
	public class MaterialDTOBase
	{
		[Required]
		public int MaterialId { get; set; }

		//[Display(Name = "Renglón")]
		public string RenglonCodigo { get; set; }

		//[Display(Name = "Grupo")]
		public string GrupoCodigo { get; set; }

		//[Column("CODIGO")]
		[Required, StringLength(14), MinLength(6)]
		[Display(Name = "Código Material")]
		public string Codigo { get; set; }

		[Required, MaxLength(10)]
		[Display(Name = "Unidad de Medida")]
		public string UnidadMedida { get; set; }

		[Required,MaxLength(254)]
		[Display(Name = "Descripción")]
		public string Descripcion { get; set; }

		//[Column("OJO")]
		//[Display(Name = "Tipo Contrato")]
		public string TipoContrato { get; set; }

		//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		//[DataType(DataType.Date)]
		//[Display(Name = "Vigencia De")]
		public DateTime? VigenciaDe { get; set; }

		//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		//[DataType(DataType.Date)]
		//[Display(Name = "Vigencia A")]
		public DateTime? VigenciaA { get; set; }

		//[Column("PRODUC")]
		//[Display(Name = "Producto")]
		public string Producto { get; set; }

		//[Column("PRECIO")]
		//[Display(Name = "Precio")]
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public decimal? Precio { get; set; }

		//[Column("MINIMO")]
		//[Display(Name = "Mínimo")]
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public decimal? Minimo { get; set; }

		//[Column("MAXIMO")]
		//[Display(Name = "Máximo")]
		public decimal? Maximo { get; set; }

		//[Column("STATUS")]
		//[Display(Name = "Estatus")]
		public string Estatus { get; set; }

		[MaxLength(20)]
		[Display(Name = "Código Siges")]
		public string SigesCodigo { get; set; } = "-ND-";
	}
}
