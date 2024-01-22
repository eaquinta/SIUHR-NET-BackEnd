using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Facilitadores")]
    public class Facilitador
    {
        [Key]
        [ForeignKey("Persona")]
        public int FacilitadorId { get; set; }
        public Persona Persona { get; set; }
    }
}