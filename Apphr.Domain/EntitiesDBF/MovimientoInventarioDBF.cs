using System;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class MovimientoInventarioDBF
    {		
			public string TIPO { get; set; }
			public string DOCUME { get; set; }
			public string CORREL { get; set; }
			public string CODMATE { get; set; }
			public string CODBODE { get; set; }
			public string VENCE{ get; set; }
			public decimal DIAMOV{ get; set; }
			public decimal MESMOV{ get; set; }
			public decimal ANOMOV{ get; set; }
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
		public static MovimientoInventarioDBF MapDataRow(DataRow dr)
		{
			return new MovimientoInventarioDBF()
			{
				TIPO = dr["TIPO"].ToString().Trim(),
				DOCUME = dr["DOCUME"].ToString().Trim(),
				CORREL = dr["CORREL"].ToString().Trim(),
				CODMATE = dr["CODMATE"].ToString().Trim(),
				//PRECIO = Convert.ToDecimal(dr["PRECIO"].ToString()),
			};
		}
	}
}


//CREATE TABLE MOVINVEN (
//TIPO		varchar(3), 
//DOCUME	varchar(7), 
//CORREL	varchar(3), 
//CODMATE	varchar(14), 
//CODBODE varchar(5), 
//VENCE varchar(8), 
//DIAMOV Numeric,
//MESMOV                           Numeric,
//ANOMOV                           Numeric,
//CANMOV                           Numeric,
//VALMOV                           Numeric,
//OBSMOV                           varchar(45), 
//DIA varchar(8), 
//HORA varchar(8), 
//CANANT Numeric,
//VALANT                           Numeric,
//MARCA                            varchar(1), 
//OPER varchar(3), 
//CODIGI varchar(5));