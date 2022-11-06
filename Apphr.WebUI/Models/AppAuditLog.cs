using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class AppAuditLog
    {
        [Key]
        public Guid AuditLogID { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Duration { get; set; }
        public string IpAddress { get; set; }
        public string Client { get; set; }
        public string Browser { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}