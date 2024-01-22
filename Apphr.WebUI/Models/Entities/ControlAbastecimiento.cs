using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("ControlAbastecimiento")]
    public class ControlAbastecimiento
    {
        //[Key]
        //[Column(Order = 0)]
        //public int DestinoId { get; set; }
        [Key]
        //[Column(Order = 1)]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        //public decimal? Inicial { get; set; } = 0;
    }
}
