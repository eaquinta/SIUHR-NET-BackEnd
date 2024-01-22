using System;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class ExistenciaTotalDBF
    {
        public string CODIGO { get; set; }
        public decimal? CANTI { get; set; }
        public decimal? VALOR { get; set; }
        public string CODIGI { get; set; }

        public static ExistenciaTotalDBF MapDataRow(DataRow dr)
        {
            return new ExistenciaTotalDBF()
            {
                CODIGO = dr["CODIGO"].ToString().Trim(),
                CANTI = Convert.ToDecimal(dr["CANTI"].ToString()),
                VALOR = Convert.ToDecimal(dr["VALOR"].ToString()),
                CODIGI = dr["CODIGI"].ToString().Trim()
            };
        }
    }
}