using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOBaseDBF
    {
        public string Orden { get; set; }
        public string Nit { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
