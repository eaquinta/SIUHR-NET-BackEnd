using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.Collections.Generic;

namespace Apphr.Application.Ortopedia.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOViewMaster : DespachoInventarioDTOBase
    {
        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}
