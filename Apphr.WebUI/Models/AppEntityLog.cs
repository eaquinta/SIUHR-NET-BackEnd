using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.WebUI.Models
{
    public class AppEntityLog
    {
        [Key]
        public Guid EntityLogId { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public string TableName { get; set; }

        //public string ActionID { get; set; }

        [Required]
        public string RecordID { get; set; }

        //[Required]
        //public string ColumnName { get; set; }

        //public string OriginalValue { get; set; }

        //public string NewValue { get; set; }

        [Required]
        public string Created_by { get; set; }

        [Required]
        public DateTime Created_date { get; set; }
    }
}