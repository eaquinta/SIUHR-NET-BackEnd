using Apphr.Application.Common.Models;
using System.Collections.Generic;

namespace Apphr.Application.Common.Models
{
    public class ViewConfig
    {
        public string PageTitle { get; set; }
        public IEnumerable<ToastTemplate> ToastTemplates { get; set; }
    }
}
