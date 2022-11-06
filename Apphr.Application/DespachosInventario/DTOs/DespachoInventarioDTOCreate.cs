using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOCreate : DespachoInventarioDTOBase
    {
        public List<DespachoInventarioDetalleDTOBase> Detalle { get; set; }
        
    }
}
