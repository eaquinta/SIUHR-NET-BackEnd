//Apphr.Application.SolicitudMaterialesSala.DTOs.SolicitudMaterialSalaDTORpt
using System;

namespace Apphr.Application.ControlMaterialesSala.DTOs
{
    public class ControlMaterialSalaDTORpt
    {
        public string PacienteNombreCompleto { get; set; }
        public string PacienteRM { get; set; }
        public string Servicio { get; set; }
        public int Cama { get; set; }
        public DateTime Fecha { get; set; }
        public string Cirujano { get; set; }
        public string Observacion { get; set; }
        public string AuxInstrumentista { get; set; }
        public string AuxCirculante { get; set; }
        public string MaterialNombre { get; set; }
        public decimal Cantidad { get; set; }
    }
}
