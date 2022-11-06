using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class SolicitudPedidoDBF :  BaseDBF
    {
        [Display(Name = "Solicitud")]
        public string NUMSOL { get; set; }

        [Display(Name = "Correlativo")]
        public string CORSOL { get; set; }

        [Display(Name = "Fecha Solicitud")]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get { return GetDate(ANOSOL, MESSOL, DIASOL); } }
        public int? ANOSOL { get; set; }
        public int? MESSOL { get; set; }
        public int? DIASOL { get; set; }

        [Display(Name = "Departamento")]
        public string DEPSOL { get; set; }

        [Display(Name = "Observaciones")]
        public string OBSSOL { get; set; }

        [Display(Name = "Tipo")]
        public string TIPSOL { get; set; }

        [Display(Name = "Elaboró")]
        public string OTRSOL { get; set; }

        [Display(Name = "Solicitante.")]
        public string SOLSOL { get; set; }

        [Display(Name = "Jefe Depto.")]        
        public string JEFSOL { get; set; }
 
        [Display(Name = "Gerente")]
        public string GERSOL { get; set; }
        
        [Display(Name = "Director")]        
        public string DIRSOL { get; set; }
        public string CPROVE { get; set; }
        public List<SolicitudPedidoDetalleDBF> Detalle { get; set; }

        public static SolicitudPedidoDBF MapDataRow(DataRow dr)
        {
            return new SolicitudPedidoDBF()
            {                
                NUMSOL = dr["NUMSOL"].ToString(),
                CORSOL = dr["CORSOL"].ToString(),
                DIASOL = Convert.ToInt32(dr["DIASOL"].ToString()),
                MESSOL = Convert.ToInt32(dr["MESSOL"].ToString()),
                ANOSOL = Convert.ToInt32(dr["ANOSOL"].ToString()),
                DEPSOL = dr["DEPSOL"].ToString(),
                OBSSOL = dr["OBSSOL"].ToString(),
                SOLSOL = dr["SOLSOL"].ToString(),
                OTRSOL = dr["OTRSOL"].ToString(),
                JEFSOL = dr["JEFSOL"].ToString(),
                GERSOL = dr["GERSOL"].ToString(),
                DIRSOL = dr["DIRSOL"].ToString(),
                TIPSOL = dr["TIPSOL"].ToString(),
                CPROVE = dr["CPROVE"].ToString(),
            };
        }

    }
}
