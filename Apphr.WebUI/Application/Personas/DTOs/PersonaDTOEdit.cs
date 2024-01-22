using Apphr.Application.Usuarios.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Personas.DTOs
{
    public class PersonaDTOEdit : PersonaDTOBase
    {
      //  [Display(Name = "UserName")]
        //[]
        public int? UserId { get; set; }
        public UsuarioDTOEdit Usuario { get; set; }
    }
}
