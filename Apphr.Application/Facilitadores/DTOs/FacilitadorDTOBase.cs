using Apphr.Application.Personas.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Facilitadores.DTOs
{
    public class FacilitadorDTOBase
    {
        public int FacilitadorId { get; set; }       
        public PersonaDTOBase  Persona { get; set; }
    }
}
