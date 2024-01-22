using System;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class ExistenciaBodegaDBF
    {
        public string CODIGO { get; set; }
        public string BODEGA { get; set; }
        public decimal? CANTI { get; set; }
        public decimal? VALOR { get; set; }
        public decimal? MINIMO { get; set; }
        public decimal? MAXIMO { get; set; }
        public decimal? STOCK { get; set; }
        public string CODIGI { get; set; }
        public decimal? PENDING { get; set; }
        public static ExistenciaBodegaDBF MapDataRow(DataRow dr)
        {
            return new ExistenciaBodegaDBF()
            {
                CODIGO = dr["CODIGO"].ToString().Trim(),
                CANTI = Convert.ToDecimal(dr["CANTI"].ToString()),
                VALOR = Convert.ToDecimal(dr["VALOR"].ToString()),
                CODIGI = dr["CODIGI"].ToString().Trim()
            };
        }
    }
}
