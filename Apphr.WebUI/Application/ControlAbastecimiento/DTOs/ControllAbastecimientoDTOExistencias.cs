using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOExistencias
    {
        public string OrdenCompraId { get; set; }
        public decimal CantidadIngreso { get; set; }
        public decimal CantidadEgreso { get; set; }
        public decimal Existencia { get; set; }
    }
}
