using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTOIxFilter
    {
        [Display(Name = "Nombre Paciente")]
        public string Nombre { get; set; }
        [Display(Name = "Apellido Paciente")]
        public string Apellido { get; set; }

        [Display(Name = "Servicio")]
        [UIHint("DropDown")]
        public int? ServicioId { get; set; }

        [Display(Name = "Pacientes activos")]
        [UIHint("DropDown")]
        public string Activo { get; set; }
    }
}
