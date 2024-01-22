using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.Collections.Generic;

namespace Apphr.Application.Ortopedia.AjustesInventario.DTOs
{
    public class AjusteInventarioDTOViewMaster : AjusteInventarioDTOBase
    {
        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}