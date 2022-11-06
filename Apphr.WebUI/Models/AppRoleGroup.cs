using Apphr.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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