using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Common.Models
{
    public class FieldPermission
    {
        public string FieldName { get; set; }
        public Permit FieldPerimssion { get; set; }
    }
}
