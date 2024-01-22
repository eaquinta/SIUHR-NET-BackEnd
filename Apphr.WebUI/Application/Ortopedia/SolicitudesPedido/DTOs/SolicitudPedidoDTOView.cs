using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOView : SolicitudPedidoDTOBase
    {
        [Display(Name = "Tipo Prioridad")]
        public string TipoPrioridadText { get; set; }

        [Display(Name = "Tipo Evento")]
        public string TipoEventoText { get; set; }

        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}
