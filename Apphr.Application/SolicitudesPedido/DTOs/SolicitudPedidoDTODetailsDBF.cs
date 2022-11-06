using Apphr.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTODetailsDBF : BaseDTODBF
    {
        
        public string Solicitud { get; set; }
        public string Correlativo { get; set; }

        [Display(Name = "Fecha Solicitud")]
        public DateTime? Fecha { get { return GetDate(ANOSOL, MESSOL, DIASOL); } }
        public string Tipo { get; set; }
        public string Departamento { get; set; }

        [Display(Name = "Elaboró")]
        public string Elaboro { get; set; }

        [Display(Name = "Solicitó")]
        public string Solicito { get; set; }
        public string Jefe { get; set;}
        public string Gerente { get; set; }
        public string Director { get; set; }
        public string Observaciones { get; set; }

        public int? ANOSOL { get; set; }
        public int? MESSOL { get; set; }
        public int? DIASOL { get; set; }
        public List<SolicitudPedidoDTODetailsL2DBF> Detalle { get; set; }
    }

    public class SolicitudPedidoDTODetailsL2DBF
    {
        public string Material { get; set; }
        public string Descripcion { get; set; }
        
        [Display(Name = "Cantidad")]
        public decimal? Cantidad { get; set; }
        public decimal? Valor { get; set; }
    }
}

