using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Personas.DTOs
{
    public class PersonaDTOFiltro
    {
        [Display(Name = "Primer Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string MiddleName { get; set; }

        [Display(Name = "Tercer Nombre")]
        public string ThirdName { get; set; }

        [Display(Name = "Primer Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string MatriName { get; set; }

        [Display(Name = "Apellido de Casada")]
        public string MarriedName { get; set; }
    }
}
