using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOListaEgresos
    {
        public int MovimientoAbastecimientoId { get; set; }
        public string HojaControlSala { get; set; }
        public string SolicitudMaterialSalaId { get; set; }
        public int MaterialId { get; set; }       
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
