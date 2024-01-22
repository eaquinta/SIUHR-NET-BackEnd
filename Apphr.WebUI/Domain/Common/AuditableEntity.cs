using System;

namespace Apphr.Domain.Common
{
    public abstract class AuditableEntity
    {
        [LogIgnore]
        public DateTime CreatedDate { get; set; }

        [LogIgnore]                                                         // Created
        public int CreatedByUser { get; set; } = -1;

        [LogIgnore]                                                         // LastModifiedDate
        public DateTime? LastModifiedDate { get; set; }

        [LogIgnore]                                                         // LastModifiedBy
        public int? LastModifiedByUser { get; set; }
    }
}
