using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class SolicitudDespachoDBF : BaseDBF
    {
        public string NUMDES { get; set; }
        public int? ANODES { get; set; }
        public int? MESDES { get; set; }
        public int? DIADES { get; set; }
        public string DEPDES { get; set; }
        public string SECDES { get; set; }
        public string OTRDES { get; set; }
        public string TIPDES { get; set; }
        public string SOLDES { get; set; }
        public string JEFDES { get; set; }
        public string GERDES { get; set; }
        public string DIRDES { get; set; }
        public string OBSDES { get; set; }
        public int? NLIDES { get; set; }
        public int? STADES { get; set; }
        public string CORDES { get; set; }
        public string SITDES { get; set; }
        public string DESPAC { get; set; }

        [Display(Name = "Fecha Solicitud")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get { return GetDate(ANODES, MESDES, DIADES); } }
        public string NumeroFormatoAutorizado { get { return (OBSDES == null) ? "0" : OBSDES.Substring(67+1, 7); } }

        public static SolicitudDespachoDBF MapDataRow(DataRow dr)
        {
            return new SolicitudDespachoDBF()
            {                
                NUMDES = dr["NUMDES"].ToString().Trim(),
                DIADES = Convert.ToInt32(dr["DIADES"].ToString()),
                MESDES = Convert.ToInt32(dr["MESDES"].ToString()),
                ANODES = Convert.ToInt32(dr["ANODES"].ToString()), 
                DEPDES = dr["DEPDES"].ToString(),
                TIPDES = dr["TIPDES"].ToString().Trim(),
                GERDES = dr["GERDES"].ToString().Trim(),
                JEFDES = dr["JEFDES"].ToString().Trim(),
                SOLDES = dr["SOLDES"].ToString().Trim(),
                OBSDES = dr["OBSDES"].ToString(),
            };
        }
    }

}
