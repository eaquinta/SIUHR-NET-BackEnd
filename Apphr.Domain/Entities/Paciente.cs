using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        [ForeignKey("Persona")]
        public int PacienteId { get; set; }
        public Persona Persona { get; set; }

        // Datos de Sistema SADC        
        public decimal? RefPac_NumHC { get; set; }        
        public decimal? RefPac_NumHCAntiguo { get; set; }
    }
}
