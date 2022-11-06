using Apphr.Application.Common;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Destinos.DTOs
{
    public class DestinoDTOIndex 
    {
        public IxFilter F { get; set; }
        public PagedList<DestinoDTOIxRow> Result { get; set; }
    }
}
