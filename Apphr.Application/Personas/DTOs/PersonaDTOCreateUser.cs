using Apphr.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Personas.DTOs
{
    public class PersonaDTOCreateUser
    {
        public int? PersonaId { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Strings))]
        [EmailAddress]
        [Required] //(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Msg))]
        public string Email { get; set; }
    }
}
