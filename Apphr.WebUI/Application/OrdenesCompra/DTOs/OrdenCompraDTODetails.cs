using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.OrdenesCompra.DTOs
{
    public class OrdenCompraDTODetails : OrdenCompraDTOBase
    {
        public List<OrdenCompraDetalleDTODetails> Detalle { get; set; }
    }
    public class OrdenCompraDetalleDTODetails : OrdenCompraDetalleDTOBase
    {
    }
}
