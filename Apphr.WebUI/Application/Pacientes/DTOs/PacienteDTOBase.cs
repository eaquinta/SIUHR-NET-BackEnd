using Apphr.Application.Personas.DTOs;

namespace Apphr.Application.Pacientes.DTOs
{
    public class PacienteDTOBase
    {
        public int PacienteId { get; set; }
        public PersonaDTOBase Persona { get; set; }
        public decimal RefPac_NumHC { get; set; }
        public decimal RefPac_NumHCAntiguo { get; set; }
    }
}
