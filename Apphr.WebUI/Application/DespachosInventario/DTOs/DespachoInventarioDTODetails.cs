using System.Collections.Generic;

namespace Apphr.Application.DespachosInventario.DTOs
{
    public class DespachoInventarioDTODetails : DespachoInventarioDTOBase 
    {
        public IEnumerable<DespachoInventarioDetalleDTODetails> Detalle { get; set; }
    }

    public class DespachoInventarioDetalleDTODetails : DespachoInventarioDetalleDTOBase
    {
    }
}
