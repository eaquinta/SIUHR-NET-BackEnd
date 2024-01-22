using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class MovimientoInventarioDBF : BaseDBF
	{		
			public string TIPO { get; set; }
			public string DOCUME { get; set; }
			public string CORREL { get; set; }
			public string CODMATE { get; set; }
			public string CODBODE { get; set; }
			public string VENCE{ get; set; }
			public int? DIAMOV { get; set; }
			public int? MESMOV { get; set; }
			public int? ANOMOV { get; set; }
			public decimal CANMOV{ get; set; }
			public decimal VALMOV{ get; set; }
			public string OBSMOV{ get; set; }
			public string DIA{ get; set; }
			public string HORA{ get; set; }
			public decimal CANANT{ get; set; }
			public decimal VALANT{ get; set; }
			public string MARCA{ get; set; }
			public string OPER{ get; set; }
			public string CODIGI{ get; set; }

			[Display(Name = "Fecha Movimiento")]
			[DataType(DataType.Date)]
			public DateTime? Fecha { get { return GetDate(ANOMOV, MESMOV, DIAMOV); } }

			[Display(Name = "Fecha Vencimiento")]
			[DataType(DataType.Date)]
			public DateTime? FechaVencimiento { get { return GetDateStr(VENCE); } }

		public static MovimientoInventarioDBF MapDataRow(DataRow dr)
		{
			return new MovimientoInventarioDBF()
			{
				TIPO =		dr["TIPO"].ToString().Trim(),
				DOCUME =	dr["DOCUME"].ToString().Trim(),
				CORREL =	dr["CORREL"].ToString().Trim(),
				CODMATE =	dr["CODMATE"].ToString().Trim(),
				OBSMOV =	dr["OBSMOV"].ToString().Trim(),
				CODBODE =	dr["CODBODE"].ToString().Trim(),
				VENCE =		dr["VENCE"].ToString().Trim(),
				CANMOV =	Convert.ToDecimal(dr["CANMOV"].ToString()),
				VALMOV =	Convert.ToDecimal(dr["VALMOV"].ToString()),
				DIAMOV =	Convert.ToInt32(dr["DIAMOV"].ToString()),
				MESMOV =	Convert.ToInt32(dr["MESMOV"].ToString()),
				ANOMOV =	Convert.ToInt32(dr["ANOMOV"].ToString()),
				//PRECIO = Convert.ToDecimal(dr["PRECIO"].ToString()),
			};
		}
	}
}