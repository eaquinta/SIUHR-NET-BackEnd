using Apphr.Domain.Common;
using System;
using System.Data;

namespace Apphr.Domain.DTOs
{
    public class DTOMovimientosInventarioDBF : BaseDBF
    {
        private DateTime? _FECMOV;
        private DateTime? _VENCE;
        public Int64 SECUENCIA { get; set; }
        public string CODIGOBODEGA { get; set; }        
        public string TIPO { get; set; }
        public string CODIGOMATERIAL { get; set; }
        public string DOCUME { get; set; }
        public int ANOMOV { get; set; }
        public int MESMOV { get; set; }
        public int DIAMOV { get; set; }
        public string UNIMED { get; set; }
        public string OBSMOV { get; set; }
        public decimal CANMOV { get; set; }
        public decimal VALMOV { get; set; }
        public DateTime? VENCE
        {
            get
            {
                return _VENCE;
            }
            set
            {
                if (value == Convert.ToDateTime("30/12/1899 00:00:00"))
                    _VENCE = null;
                else
                    _VENCE = value;
            }
        }
        public DateTime? FECMOV {
            get
            {
                return _FECMOV;
            }
            set
            {
                _FECMOV = (value == Convert.ToDateTime("29/12/1899 00:00:00")) ? null : value;
            }
        }
        public decimal COSMOV { get { return decimal.Round((CANMOV == 0) ? 0 : (VALMOV / CANMOV), 2); } }
        public DateTime? Fecha { get { return GetDate(ANOMOV, MESMOV, DIAMOV); } }        

        public static DTOMovimientosInventarioDBF MapDataRow(DataRow dr)
        {
            
            var res = new DTOMovimientosInventarioDBF();
            res.SECUENCIA = Convert.ToInt64(dr["SECUENCIA"].ToString());
            res.VENCE = Convert.ToDateTime(dr["VENCE"].ToString().Trim());
            res.FECMOV = Convert.ToDateTime(dr["FECMOV"].ToString().Trim());
            res.TIPO = dr["TIPO"].ToString().Trim();
            res.CODIGOBODEGA = dr["CODIGOBODEGA"].ToString().Trim();
            res.CODIGOMATERIAL = dr["CODIGOMATERIAL"].ToString().Trim();
            res.DOCUME = dr["DOCUME"].ToString().Trim();
            res.UNIMED = dr["UNIMED"].ToString().Trim();
            res.OBSMOV = dr["OBSMOV"].ToString().Trim();
            res.DIAMOV = Convert.ToInt32(dr["DIAMOV"].ToString());
            res.MESMOV = Convert.ToInt32(dr["MESMOV"].ToString());
            res.ANOMOV = Convert.ToInt32(dr["ANOMOV"].ToString());
            res.CANMOV = Convert.ToDecimal(dr["CANMOV"].ToString());
            res.VALMOV = Convert.ToDecimal(dr["VALMOV"].ToString());
            return res;
        }
    }
}
