using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDetalleDTOBaseDBF
    {
        public string MaterialCodigo { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CantidadSolicitada { get; set; }
        public decimal CantidadDespachada { get; set; }
    }
}
