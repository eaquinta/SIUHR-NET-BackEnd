using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Personas.DTOs
{
    public class PersonaDTOEmailConfirm
    {
       [Display(Name ="Nombre")]
        public string  Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Url { get; set; }
    }
}
