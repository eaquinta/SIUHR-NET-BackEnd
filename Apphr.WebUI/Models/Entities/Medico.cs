using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Medicos")]
    public class Medico
    {
        [Key]
        [ForeignKey("Persona")]
        public int MedicoId { get; set; }
        public Persona Persona { get; set; }
    }
}
