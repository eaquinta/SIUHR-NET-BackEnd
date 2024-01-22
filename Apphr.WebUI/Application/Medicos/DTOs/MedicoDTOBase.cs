using Apphr.Application.Personas.DTOs;

namespace Apphr.Application.Medicos.DTOs
{
    public class MedicoDTOBase
    {
        public int MedicoId { get; set; }
        public PersonaDTOBase Persona { get; set; }
    }
}
