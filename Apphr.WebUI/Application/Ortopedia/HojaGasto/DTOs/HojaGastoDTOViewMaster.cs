using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.Collections.Generic;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOViewMaster : HojaGastoDTOBase
    {
        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}
