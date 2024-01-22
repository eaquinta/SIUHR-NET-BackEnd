using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("ControladorRolAsignaciones")]
    public class ControladorRolAsignacion
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Controlador")]
        public int AccionId { get; set; }
        public Controlador Controlador { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public AppRole Role { get;  set; }
    }
}
