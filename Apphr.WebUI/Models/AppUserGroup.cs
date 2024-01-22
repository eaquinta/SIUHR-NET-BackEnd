using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class AppUserGroup
    {
        [Key]
        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual AppGroup Group { get; set; }
    }
}