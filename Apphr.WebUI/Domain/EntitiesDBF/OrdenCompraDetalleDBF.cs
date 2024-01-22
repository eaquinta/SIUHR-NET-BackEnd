using System;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class OrdenCompraDetalleDBF
    {
        public string NIT { get; set; }
        public string ORDEN { get; set; }
        public string PROGR { get; set; }
        public string ACTIV { get; set; }
        public string RENGL { get; set; }
        public string FINAN { get; set; }
		public string CODMAT { get; set; }
		public string UNIDET { get; set; }
		public decimal? CANDET { get; set; }
		public decimal? VALDET { get; set; }
		public string OBSDET { get; set; }
		public string OBSDE1 { get; set; }
		public decimal? CANREC { get; set; }
		public decimal? VALREC { get; set; }
		public decimal? PREDET { get; set; }
		public string CONSTA { get; set; }
		public string AJUSTE { get; set; }
		public string CODIGI { get; set; }
		public string SOLREL { get; set; }
		public decimal? AUTORI { get; set; }

        public static OrdenCompraDetalleDBF MapDataRow(DataRow dr)
        {
            return new OrdenCompraDetalleDBF()
            {
                ORDEN =     dr["ORDEN"].ToString().Trim(),
                NIT =       dr["NIT"].ToString(),
                CODMAT =    dr["CODMAT"].ToString(),
                RENGL =     dr["RENGL"].ToString(),
                UNIDET =    dr["UNIDET"].ToString(),
                SOLREL =    dr["SOLREL"].ToString(),
                CANDET =    Convert.ToDecimal(dr["CANDET"].ToString()),
                PREDET =    Convert.ToDecimal(dr["PREDET"].ToString()),
                VALDET =    Convert.ToDecimal(dr["VALDET"].ToString()),
                CANREC =    Convert.ToDecimal(dr["CANREC"].ToString())
            };
        }
    }
}
