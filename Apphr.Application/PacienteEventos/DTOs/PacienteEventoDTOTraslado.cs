using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTOTraslado
    {
        public int PacienteEventoId { get; set; }
        public PacienteEvento Paciente { get; set; }
        //public int ServicioId { get; set; }
        public string Motivo { get; set; }

        [Display(Name = "Fecha Traslado")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Servicio Destino")]
        public int? ServicioDestinoId { get; set; }
        public string Usuario { get; set; }
        [Display(Name = "Cama Destino")]
        public int? CamaDestino { get; set; }
    }
}
