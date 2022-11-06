using System.Collections.Generic;

namespace Apphr.Application.SolicitudMaterialesSala.DTOs
{
    public class SolicitudMaterialSalaDTODetails : SolicitudMaterialSalaDTOBase
    {
        public string PacienteRM { get; set; }
        public string PacienteNombreCompleto { get;set;}
        public IEnumerable<SolicitudMaterialSalaDetalleDTODetails> Detalle {get;set;}
    }
    public class SolicitudMaterialSalaDetalleDTODetails : SolicitudMaterialSalaDetalleDTOBase
    {

    }
}
