using Apphr.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Bodegas")]
    public class Bodega : AuditableEntity
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
