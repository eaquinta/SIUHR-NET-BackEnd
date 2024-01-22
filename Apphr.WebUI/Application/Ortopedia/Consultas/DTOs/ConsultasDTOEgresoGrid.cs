using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultasDTOEgresoGrid : MovimientoDTOBase
    {
        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
        public string CirujanoNombre { get; set; }

        [Display(Name = "Registro médico")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
    }
}
