using System.Collections.Generic;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTODetails : SolicitudDespachoDTOBase
    {
        public List<SolicitudDespachoDetalleDTODetails> Detalle { get; set; }
    }

    public class SolicitudDespachoDetalleDTODetails : SolicitudDespachoDTDTOBase { }
}
