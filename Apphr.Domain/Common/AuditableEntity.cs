using System;

namespace Apphr.Domain.Common
{
    public abstract class AuditableEntity
    {
        [LogIgnore]
        public DateTime CreatedDate { get; set; }

        [LogIgnore]
        public string CreatedBy { get; set; }

        [LogIgnore]
        public DateTime? LastModifiedDate { get; set; }

        [LogIgnore]
        public string LastModifiedBy { get; set; }
    }
}
