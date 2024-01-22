using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Entities
{
    [Table("PacienteEventoTraslados")]
    public class PacienteEventoTraslado : AuditableEntity
    {
        [Key]
        public int PacienteEventoTrasladoId { get; set; }
       // [ForeignKey("PacienteEvento")]
        public int PacienteEventoId { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }
}
