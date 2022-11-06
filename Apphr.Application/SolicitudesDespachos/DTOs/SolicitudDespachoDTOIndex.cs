using Apphr.Application.Common;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTOIndex
    {
        public IxFilter F { get; set; }
        public PagedList<SolicitudDespachoDTOBase> Result { get; set; }
    }
}
