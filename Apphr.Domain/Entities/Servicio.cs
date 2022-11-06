using Apphr.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(25, ErrorMessage = "Nombre debe ser menor a 25 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        //[StringLength(25)]
        [Display(Name = "Area")]
        [UIHint("DropDown")]
        public Area Area { get; set; }
    }
}
