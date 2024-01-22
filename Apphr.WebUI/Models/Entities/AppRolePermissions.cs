using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;

namespace Apphr.WebUI.Models.Entities
{
    [Table("AppRolePermissions")]
    public class AppRolePermission
    {
        [Key, Column(Order = 0)]        
        public int AppRoleId { get; set; }

        [Key, Column(Order = 1)]        
        public int AppPermissionId { get; set; }

        [ForeignKey("AppRoleId")]        
        public virtual AppRole AppRole { get; set; } 

        [ForeignKey("AppPermissionId")]        
        public virtual AppPermission AppPermission { get; set; }

    }
}