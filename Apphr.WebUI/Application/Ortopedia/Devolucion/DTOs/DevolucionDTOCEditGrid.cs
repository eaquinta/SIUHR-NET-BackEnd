using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOCEditGrid : MovimientoDTOBase
    {
        public Int64 HojaGastoId { get; set; }
        public string HGFormulario { get; set; }
    }
}
