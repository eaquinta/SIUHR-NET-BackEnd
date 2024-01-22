//Apphr.Application.Ortopedia.Devolucion.DTOs.DevolucionDTOReporte

using System;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOReporte
    {
        public string Formulario { get; set; }
        public DateTime FechaCirugia { get; set; }     
        public string PacienteNombre { get; set; }
        public string PacienteRegistro { get; set; }
        public string MaterialCodigo { get; set; }
        public string MaterialNombre  { get; set; }
        public decimal Cantidad  { get; set; }
        public string ProveedorNombre  { get; set; }        
    }
}
