using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.AppAreas.DTOs
{
    public class RootDTODashboard
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int Activos { get; set; }
        public int Ingresos { get; set; }
        public int Egresos { get; set; }
        public int Total { get; set; }
    }
}
