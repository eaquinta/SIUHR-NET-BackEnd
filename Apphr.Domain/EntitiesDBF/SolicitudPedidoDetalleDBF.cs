﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class SolicitudPedidoDetalleDBF
    {
        public string NUMSOL { get; set; }        
        public string MATSOL { get; set; }
        public string CODIGI { get; set; }
        public string UMESOL { get; set; }       

        [Display(Name = "Cantidad")]
        public decimal? CANSOL { get; set; }
        public decimal? VALSOL { get; set; }
        public static SolicitudPedidoDetalleDBF MapDataRow(DataRow dr)
        {
            return new SolicitudPedidoDetalleDBF()
            {
                NUMSOL = dr["NUMSOL"].ToString(),
                MATSOL = dr["MATSOL"].ToString().Trim(),
                CODIGI = dr["CODIGI"].ToString().Trim(),
                UMESOL = dr["UMESOL"].ToString(),
                CANSOL = Convert.ToDecimal(dr["CANSOL"].ToString()),
                VALSOL = Convert.ToDecimal(dr["VALSOL"].ToString()),
            };
        }
    }
}
