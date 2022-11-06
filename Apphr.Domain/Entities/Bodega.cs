using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{
    [Table("Bodegas")]
    public class Bodega
    {
        [Key]
        public int BodegaId { get; set; }

        [Display(Name = "Nombre")]        
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Procedencia")]
        public string Procedencia { get; set; }

        //[Display(Name = "BodegaId")]
        //public string BODMAT { get; set; }
    }
}
