using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities //Apphr.Domain.Entities
{
    //[Table("AppRoles")]
    public class AppRole : IdentityRole<int, AppUserRole>, IRole<int>
    {   
        public AppRole() 
        {
        }
        public AppRole(string name) : this()
        {
            this.Name = name;
        }
        public AppRole(string name, string description) : this(name)
        {
            this.Description = description;
        }
        public string Description { get; set; }

        [JsonIgnore] // Ignorar esta propiedad al serializar
        public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; }

        //[InverseProperty("AppRole")]
        //public virtual ICollection<AppPermission> Permissions { get; set; } //= new HashSet<AppPermission>();
        //public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; }
    }
}
