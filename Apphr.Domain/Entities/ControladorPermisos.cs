using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("ControladorPermisos")]
    public class ControladorPermiso
    {
        [Key]
        public int ControladorPermisoId { get; set; }
        [ForeignKey("Controlador")]
        public int AccionId { get; set; }
        public Controlador Controlador { get; set; }
        public string Nombre { get; set; }
        public Permit Permiso { get; set; }
    }
}
