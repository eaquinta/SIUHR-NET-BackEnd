using Apphr.Application.Personas.DTOs;
using Apphr.WebUI.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Usuarios.DTOs
{
    public class UsuarioDTOBase
    {
        public int Id { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        //public int? PersonaId { get; set; }

        //public PersonaDTOBase Persona { get; set; }

        public List<int> SelectedRoles { get; set; }

        public List<AppUserRole> Roles { get; set; }
    }
}
