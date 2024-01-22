using System;
using System.Data;


//Apphr.Domain.EntitiesDBF.ReporteExistenciasDBF
namespace Apphr.Domain.EntitiesDBF
{
    public class ReporteExistenciasDBF
    {
        public string CODIGO { get; set; }
        public string CODIGI { get; set; }
		public string VENCE { get; set; }
		public string UNIMED { get; set; }
		public string DESCRI { get; set; }
		public string CODBODE { get; set; }
		public decimal CANMOV { get; set; }
		public decimal VALMOV { get; set; }
		public decimal? CostoUnitario
		{
			get
			{
                if (CANMOV == 0)
                {
					return null;
                }
				var res = Math.Round((VALMOV / CANMOV), 2);
				return Convert.ToDecimal(string.Format("{0:F2}", res));
			}
		}


		public static ReporteExistenciasDBF MapDataRow(DataRow dr)
		{
			var res = new ReporteExistenciasDBF();
			res.CODIGO = dr["CODIGO"].ToString().Trim();
			res.CODIGI = dr["CODIGI"].ToString().Trim();
			res.VENCE = dr.Table.Columns.Contains("VENCE") ? dr["VENCE"].ToString().Trim() : "";
			res.DESCRI = dr["DESCRI"].ToString().Trim();
			res.UNIMED = dr["UNIMED"].ToString().Trim();
			res.CANMOV = Convert.ToDecimal(dr["CANMOV"].ToString());
			res.VALMOV = Convert.ToDecimal(dr["VALMOV"].ToString());
			return res;
		}
	}
}
