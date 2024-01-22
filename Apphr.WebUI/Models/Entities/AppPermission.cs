using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("AppPermissions")]
    public class AppPermission : AuditableEntity
    {
        [Key]
        public int PermissionId { get; set; }

        [StringLength(100)]
        public String Name { get; set; }

        //public virtual ICollection<AppRole> AppRoles { get; set; } // = new HashSet<AppRole>();

        //[InverseProperty("AppPermission")]
        //public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; }        
    }
}