using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOIndex
    {
        public DespachoInventarioDTOIxFilter F { get; set; }
        public PagedList<DespachoInventarioDTOIxRow> Result { get; set; }
    }
}
