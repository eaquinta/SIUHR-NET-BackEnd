using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTODetailsDBF : SolicitudDespachoDTOBaseDBF
    {
        public IEnumerable<SolicitudDespachoDetalleDTODetailsDBF> Detalle { get; set; }
    }

    public class SolicitudDespachoDetalleDTODetailsDBF : SolicitudDespachoDetalleDTOBaseDBF
    {

    }
}
