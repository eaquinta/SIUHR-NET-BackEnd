using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTOEgreso
    {
        public int PacienteEventoId { get; set; }
        public PacienteEvento Paciente { get; set; }
        //public int ServicioId { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public int? ServicioId { get; set; }
        public string Usuario { get; set; }
    }
}
