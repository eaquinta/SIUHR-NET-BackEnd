using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    public class AccessRulePermitAssignment
    {
        [Key]
        [Column(Order = 0)]
        public int AccessRuleId {get;set;}
        public AccessRule AccessRule { get; set; }

        [Key]
        [Column(Order = 1)]
        public Permit PermitId { get; set; }
    }
}
