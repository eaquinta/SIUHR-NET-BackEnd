using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOCreate :HojaGastoDTOBase
    {
        [Required]
        [Display(Name = "Registro médico")]
        [Remote("JsPacienteRMExist", "Paciente", "Ortopedia",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "PacienteRM_NotExist")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }
    }
}
