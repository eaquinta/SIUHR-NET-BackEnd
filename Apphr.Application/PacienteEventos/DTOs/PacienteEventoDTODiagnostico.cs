using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTODiagnostico
    {
        public int PacienteEventoId { get; set; }
        public PacienteEvento Paciente { get; set; }

        [Display(Name = "Diagnóstico Anterior")]
        public string DiagnosticoOrignal { get; set; }

        [Display(Name = "Nuevo Diagnóstico")]
        [Required]
        public string DiagnosticoNuevo { get; set; }

        [Display(Name = "Fecha Traslado")]
        public DateTime Fecha { get; set; }

        public int? UserId { get; set; }
        public AppUser Usuario { get; set; }
    }
}
