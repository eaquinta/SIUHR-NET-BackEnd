using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultasDTOMovimientosIxFilter : ConsultaDTOBaseIxFilter
    {
        [UIHint("DropDown")]
        [Display(Name = "Paciente")]
        public int? PacienteId { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombre { get; set; }
       

        [UIHint("DropDown")]
        [Display(Name = "Cirujano")]
        public int? CirujanoId { get; set; }


        [UIHint("DropDown")]
        [Display(Name = "Servicio")]
        public int? ServicioId { get; set; }       

    }
}
