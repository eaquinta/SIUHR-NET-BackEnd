using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{
    public class AccessRuleRoleAssignment
    {
        [Key]
        [Column(Order = 0)]
        public int AccessRuleId { get; set; }
        public AccessRule AccessRule { get; set; }
        [Key]
        [Column(Order = 1)]
        public int AppRoleId { get; set; }
        public AppRole  Role { get; set; }
    }
}
