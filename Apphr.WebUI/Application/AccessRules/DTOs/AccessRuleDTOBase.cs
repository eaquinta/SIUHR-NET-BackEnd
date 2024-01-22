using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.AccessRules.DTOs
{
    public class AccessRuleDTOBase
    {
        public int AccessRuleId { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Seleccion Multiple de Roles")]
        public List<int> SelectedRoles { get; set; }

        public List<Permit> SelectedPermits { get; set; }

        public List<AccessRuleRoleAssignment> Roles { get; set; }

        public List<AccessRulePermitAssignment> Permits { get; set; }
    }
}
