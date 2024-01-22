using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.EntitiesDBF
{
    public class SolicitudDespachoDetalleDBF
    {
        public string NUMDES { get; set; }
        public string MATDES { get; set; }
        public string ACTDES { get; set; }
        public decimal? CANSOL { get; set; }
        public string UMEDES { get; set; }
        public string SALBOD { get; set; }
        public decimal? SALMAX { get; set; }
        public decimal? CANAPR { get; set; }
        public decimal? CANDES { get; set; }
        public decimal? CANEME { get; set; }
        public string OBSDES { get; set; }
        public string CORDES { get; set; }
        public decimal CANANT { get; set; }
        public string BODEGA { get; set; }
        public string BODEXI { get; set; }
        public string DESPAC { get; set; }
        public string BODEXF { get; set; }
        public string CODIGI { get; set; }
        public string SERVIC { get; set; }


        public static SolicitudDespachoDetalleDBF MapDataRow(DataRow dr)
        {
            return new SolicitudDespachoDetalleDBF()
            {
                NUMDES = dr["NUMDES"].ToString().Trim(),
                MATDES = dr["MATDES"].ToString().Trim(),
                UMEDES = dr["UMEDES"].ToString().Trim(),
                CODIGI = dr["CODIGI"].ToString().Trim(),
                CANSOL = Convert.ToDecimal(dr["CANSOL"].ToString()),
                CANDES = Convert.ToDecimal(dr["CANDES"].ToString()),
                BODEXI = dr["BODEXI"].ToString().Trim(),
                //MESDES = Convert.ToInt32(dr["MESDES"].ToString()),
                //ANODES = Convert.ToInt32(dr["ANODES"].ToString()),
            };
        }


    }
}
