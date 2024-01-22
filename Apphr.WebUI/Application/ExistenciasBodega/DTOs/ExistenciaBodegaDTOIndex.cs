using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ExistenciasBodega.DTOs
{
    public class ExistenciaBodegaDTOIndex : ExistenciaBodegaDTOBase
    {
        public string BodegaNombre { get; set; }
        public string MaterialNombre { get; set; }
        public string ProveedorNit { get; set; }
        public string ProveedorDescripcion { get; set; }
    }
}
