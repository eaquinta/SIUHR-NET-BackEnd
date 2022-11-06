using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTODetail : PacienteEventoDTOBase
    {
        public Servicio Servicio { get; set; }
        public ICollection<PacienteEventoHistorial> Historial { get; set; }
    }
}
