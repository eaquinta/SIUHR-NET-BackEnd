using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.Collections.Generic;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOViewMaster : DevolucionDTOBase
    {
        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}
