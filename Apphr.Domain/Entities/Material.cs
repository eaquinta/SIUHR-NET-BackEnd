using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
	[Table("Materiales")]
	public class Material
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		//[Column("Id")]
		//[Display(Name = "MaterialId")]			
		public int MaterialId { get; set; }		

		//[Display(Name = "Renglón")]
		public string RenglonCodigo { get; set; }

		//[Display(Name = "Grupo")]
		public string GrupoCodigo { get; set; }

		//[Column("CODIGO")]
		//[Display(Name = "Código Material")]
		[Index("IX_Codigo", IsUnique = true)]
		[MaxLength(15)]
		public string Codigo { get; set; }

		//[Column("UNIMED")]
		//[Display(Name = "Unidad Medida")]
		public string UnidadMedida { get; set; }

		//[Column("DESCRI")]
		//[Display(Name = "Descripción")]
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
		public decimal? Precio { get; set; }

		//[Column("MINIMO")]
		//[Display(Name = "Mínimo")]
		public decimal? Minimo { get; set; }

		//[Column("MAXIMO")]
		//[Display(Name = "Máximo")]
		public decimal? Maximo { get; set; }

		//[Column("STATUS")]
		//[Display(Name = "Estatus")]
		public string Estatus { get; set; }

		//[MaxLength(10)]
		//[Display(Name = "Código Siges")]
		public string SigesCodigo { get; set; }
    }
}
