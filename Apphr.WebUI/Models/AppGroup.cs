using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class AppGroup
    {
        public AppGroup() { }


        public AppGroup(string name) : this()
        {
            this.Roles = new List<AppRoleGroup>();
            this.Name = name;
        }


        [Key]
        [Required]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual ICollection<AppRoleGroup> Roles { get; set; }
    }
}