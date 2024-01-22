using System.ComponentModel.DataAnnotations;

namespace Apphr.WebUI.Application.AppRoles.DTOs
{
    public class AppRoleDTOBase
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}