using Apphr.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Domain.Entities
{
    public class AccessRule : AuditableEntity
    {
        [Key]
        public int AccessRuleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<AccessRuleRoleAssignment> Roles { get; set; }

        public List<AccessRulePermitAssignment> Permits { get; set; }        
    }
}
