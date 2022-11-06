using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("Controladores")]
    public class Controlador
    {
        [Key]
        public int AccionId { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Detalle { get; set; }
        public List<ControladorRolAsignacion> Roles { get; set; }
    }
}
