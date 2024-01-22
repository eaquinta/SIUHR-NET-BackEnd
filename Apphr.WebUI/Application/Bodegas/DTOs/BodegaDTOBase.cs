using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Bodegas.DTOs
{
    public class BodegaDTOBase
    {
        public int BodegaId { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Procedencia")]
        public string Procedencia { get; set; }
    }
}
