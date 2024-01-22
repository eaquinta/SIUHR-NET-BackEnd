using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.SolicitudMaterialesSala.DTOs
{
    public class SolicitudMaterialSalaDTOIndex
    {
        public string SolicitudMaterialSalaId { get; set; }
        public string HojaControlSala { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        public string PacienteRM { get; set; }
        public string PacienteNombreCompleto { get; set; }
    }
}
