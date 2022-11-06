using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class OrdenCompraDBF : BaseDBF
    {
        public string NIT { get; set; }
        public string ORDEN { get; set; }
        public string AUTORI { get; set; }
        public string CHEQUE { get; set; }
        public int? DIAAUT { get; set; }
        public int? MESAUT { get; set; }
        public int? ANOAUT { get; set; }
        public int? ANOORD { get; set; }
        public int? MESORD { get; set; }
        public int? DIAORD { get; set; }
        public decimal? VALORD { get; set; }
        public string OBSORD { get; set; }
        public int? NUMFAC { get; set; }
        public decimal? VALFAC { get; set; }
        public int? ANOFAC { get; set; }
        public int? MESFAC { get; set; }
        public int? DIAFAC { get; set; }
        public int? ANOCHE { get; set; }
        public int? MESCHE { get; set; }
        public int? DIACHE { get; set; }
        public decimal? VALCHE { get; set; }
        public string BANCHE { get; set; }
        public string STATUS { get; set; }
        public decimal? ACUREN { get; set; }
        public int? NUMREN { get; set; }
        public string ORDPAG { get; set; }
        public string ATENTO { get; set; }
        public int? DIAENV { get; set; }
        public int? MESENV { get; set; }
        public int? ANOENV { get; set; }
        public int? PLAN11 { get; set; }
        public string CONTRA { get; set; }
        public int? PLAN29 { get; set; }
        public int? PLAN51 { get; set; }
        public int? PLAN52 { get; set; }
        public int? PLANOT { get; set; }

        [Display(Name = "Fecha Solicitud")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get { return GetDate(ANOORD, MESORD, DIAORD); } }

        public static OrdenCompraDBF MapDataRow(DataRow dr)
        {
            return new OrdenCompraDBF()
            {
                ORDEN = dr["ORDEN"].ToString().Trim(),
                NIT = dr["NIT"].ToString().Trim(),
                DIAORD = Convert.ToInt32(dr["DIAORD"].ToString()),
                MESORD = Convert.ToInt32(dr["MESORD"].ToString()),
                ANOORD = Convert.ToInt32(dr["ANOORD"].ToString()),
                //DEPSOL = dr["DEPSOL"].ToString(),
                //OBSSOL = dr["OBSSOL"].ToString(),
                //SOLSOL = dr["SOLSOL"].ToString(),
                //OTRSOL = dr["OTRSOL"].ToString(),
                //JEFSOL = dr["JEFSOL"].ToString(),
                //GERSOL = dr["GERSOL"].ToString(),
                //DIRSOL = dr["DIRSOL"].ToString(),
                //TIPSOL = dr["TIPSOL"].ToString(),
                //CPROVE = dr["CPROVE"].ToString(),
            };
        }

    }
}
