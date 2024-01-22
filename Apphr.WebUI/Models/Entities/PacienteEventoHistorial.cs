using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Entities
{
    public class PacienteEventoHistorial
    {
        [Key]
        public int PacienteEventoHistoryId { get; set; }
        public int PacienteEventoId { get; set; }

        public DateTime Fecha { get; set; }
        public PacienteEventoTipo Tipo { get; set; }

        [ForeignKey("ServicioOrigen")]
        public int? ServicioOrigenId { get; set; }
        public Servicio ServicioOrigen { get; set; }

        [ForeignKey("ServicioDestino")]
        public int? ServicioDestinoId { get; set; }
        public Servicio ServicioDestino { get; set; }


        public int? CamaOrigen { get; set; }
        public int? CamaDestino { get; set; }


        public string DiagnosticoAnterior { get; set; }
        public string DiagnosticoNuevo { get; set; }

        public string EstadoObservacion { get; set; }

        [ForeignKey("Usuario")]
        public int? UserId { get; set; }
        public AppUser Usuario { get; set; }
    }
}
