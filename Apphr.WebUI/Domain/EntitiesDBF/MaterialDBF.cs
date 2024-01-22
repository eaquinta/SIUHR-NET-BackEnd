using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class MaterialDBF : BaseDBF
	{
		[Display(Name = "Código Material")]
		public string CODIGO { get; set; }		

		[Display(Name = "Unidad Medida")]
		public string UNIMED { get; set; }


		[Display(Name = "Descripción")]
		public string DESCRI { get; set; }


		//[Display(Name = "Tipo Contrato")]
		public string OJO { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		[Display(Name = "Vigencia De")]
		public DateTime? VigenciaDe { get { return GetDate(VIGEDA, VIGEDM, VIGEDD); } }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		[Display(Name = "Vigencia A")]
		public DateTime? VigenciaA { get { return GetDate(VIGEAA, VIGEAM, VIGEAD); } }

		public DateTime? FechaCreacion { get { return ConcatDateTime(FECHA, GetTime(HORA)); } }

		public string PRODUC { get; set; }

		//[Column("PRECIO")]
		//[Display(Name = "Precio")]
		public decimal? PRECIO { get; set; }

		//[Column("MINIMO")]
		//[Display(Name = "Mínimo")]
		public decimal? MINIMO { get; set; }

		//[Column("MAXIMO")]
		//[Display(Name = "Máximo")]
		public decimal? MAXIMO { get; set; }

		//[Col/*u*/mn("STATUS")]
		//[Display(Name = "Estatus")]
		public string STATUS { get; set; }

		//[MaxLength(10)]
		//[Display(Name = "Código Siges")]
		//public string SigesCodigo { get; set; }

		//[Display(Name = "ALTERNODE")]
		public string ALTERNODE { get; set; }


		//[Display(Name = "BRES")]
		public string BRES { get; set; }


		//[Display(Name = "Código Interno")]
		public string CODIGI { get; set; }


		//[Display(Name = "Fecha")]
		//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		//[DataType(DataType.Date)]
		public DateTime? FECHA { get; set; }


		//[Display(Name = "Hora")]
		public string HORA { get; set; }


		//[Display(Name = "Usuario")]
		public string USUA { get; set; }


		//[Display(Name = "Uso Bodega")]
		public string USOBOD { get; set; }


		//[Display(Name = "VIGEDA")]
		public int? VIGEDA { get; set; }


		//[Display(Name = "VIGEDM")]
		public int? VIGEDM { get; set; }


		//[Display(Name = "VIGEDD")]
		public int? VIGEDD { get; set; }


		//[Display(Name = "VIGEAA")]
		public int? VIGEAA { get; set; }


		//[Display(Name = "VIGEAM")]
		public int? VIGEAM { get; set; }


		//[Display(Name = "VIGEAD")]
		public int? VIGEAD { get; set; }


		//[Display(Name = "USOSER")]
		public string USOSER { get; set; }

		//[MaxLength(10)]
		//[Display(Name = "Código Presentación Siges")]
		//public string SigesCodigoPresentacion { get; set; }	

		public static MaterialDBF MapDataRow(DataRow dr)
		{
			DateTime x = Convert.ToDateTime(dr["FECHA"]);
			return new MaterialDBF()
			{
				CODIGO		= dr["CODIGO"].ToString().Trim(),				
				DESCRI		= dr["DESCRI"].ToString().Trim(),				
				UNIMED		= dr["UNIMED"].ToString().Trim(),						
				PRODUC		= dr["PRODUC"].ToString().Trim(),
				PRECIO		= Convert.ToDecimal(dr["PRECIO"].ToString()),
				MINIMO		= Convert.ToDecimal(dr["MINIMO"].ToString()),
				MAXIMO		= Convert.ToDecimal(dr["MAXIMO"].ToString()),
				OJO			= dr["OJO"].ToString().Trim(),
				ALTERNODE	= dr["ALTERNODE"].ToString().Trim(),
				BRES		= dr["BRES"].ToString().Trim(),
				CODIGI		= dr["CODIGI"].ToString().Trim(),
				FECHA		= Convert.ToDateTime(dr["FECHA"]),
				HORA		= dr["HORA"].ToString().Trim(),
				USUA		= dr["USUA"].ToString().Trim(),
				STATUS		= dr["STATUS"].ToString().Trim(),
				USOBOD		= dr["USOBOD"].ToString().Trim(),
				VIGEDA		= Convert.ToInt32(dr["VIGEDA"].ToString()),
				VIGEDM		= Convert.ToInt32(dr["VIGEDM"].ToString()),
				VIGEDD		= Convert.ToInt32(dr["VIGEDD"].ToString()),
				VIGEAA		= Convert.ToInt32(dr["VIGEAA"].ToString()),
				VIGEAM		= Convert.ToInt32(dr["VIGEAM"].ToString()),
				VIGEAD		= Convert.ToInt32(dr["VIGEAD"].ToString()),
				USOSER		= dr["USOSER"].ToString().Trim(),
			};
		}
	}
}
