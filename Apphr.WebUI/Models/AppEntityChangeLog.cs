using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class AppEntityChangeLog
    {
        [Key]
        public Guid EntityChangeLogId { get; set; }

        public Guid EntityLogId { get; set; }

        [Required]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }
    }
}