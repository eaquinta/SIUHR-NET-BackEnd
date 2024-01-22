using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOListaIngresos
    {
        public int MaterialId { get; set; }
        public int IngresoAbastecimientoId { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
