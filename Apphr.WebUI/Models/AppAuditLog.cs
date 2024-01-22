using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.WebUI.Models
{
    public class AppAuditLog
    {
        [Key]
        public Guid AuditLogID { get; set; }

        [Required]
        //public string UserName { get; set; }  //C20230613
        public int UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Duration { get; set; }
        public string IpAddress { get; set; }
        //public string Client { get; set; } //C20230613
        public string Browser { get; set; }
        public string Url { get; set; }

        [StringLength(10)]
        public string Method { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}