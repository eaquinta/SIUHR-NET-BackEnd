using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
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

        [JsonIgnore]        
        public List<ControladorRolAsignacion> Roles { get; set; }
    }
}
