using Apphr.WebUI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Apphr.WebUI.Models
{
    public class AppRoleGroup
    {
        [Key]
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }

        public virtual AppRole Role { get; set; }
        public virtual AppGroup Group { get; set; }
    }

}