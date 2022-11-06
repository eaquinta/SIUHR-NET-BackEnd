using Apphr.Application.Personas.DTOs;
using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Facilitadores.DTOs
{
    public class FacilitadorDTOBuscarPersona
    {
        public PersonaDTOFiltro Filtro { get; set; }
        public IEnumerable<Persona> Lista { get; set; }

        public FacilitadorDTOBuscarPersona()
        {
            Filtro = new PersonaDTOFiltro();
            Lista = new List<Persona>();
        }
    }
}
