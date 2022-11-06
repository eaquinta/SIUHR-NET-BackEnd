using Apphr.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Controladores.DTOs
{
    public class ControladorDTOBase
    {
        public int AccionId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Detalle { get; set; }

        [Display(Name = "Seleccion Multiple de Roles")]
        public List<int> SelectedRoles { get; set; }
        public List<ControladorRolAsignacion> Roles { get; set; }
    }
}
