using System;

namespace Apphr.Application.Common.DTOs
{
    public class AuditableEntityDTOBase
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
