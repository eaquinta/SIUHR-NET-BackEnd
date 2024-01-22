using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class InicioAbastecimientoDTOIndex
    {
        public int DestinoId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCodigo { get; set; }
        public string MaterialDescripcion { get; set; }
        public int ProveedorId { get; set; }
        public string ProveedorNit { get; set; }
        public string ProveedorNombre { get; set; }
        public decimal Cantidad { get; set; } = 0;
    }
}
