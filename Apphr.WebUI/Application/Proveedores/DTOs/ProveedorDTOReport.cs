using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOReport
    {
        public string Nit { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public int DiasCredito { get; set; }
    }
}
