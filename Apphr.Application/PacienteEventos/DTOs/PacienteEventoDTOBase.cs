using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTOBase
    {
        public int PacienteEventoId { get; set; }
        public string Procedencia { get; set; }

        public int? Edad { get; set; }

        [Display(Name = "Nombre de Paciente")]
        [Required]
        public string NombrePaciente { get; set; }

        [Display(Name = "Nombre de Familiar")]
        public string NombreFamiliar { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyyTHH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Egreso")]
        public DateTime? FechaEgreso { get; set; }
        public int? Cama { get; set; }
        public string Registro { get; set; }

        //public string RegistroMedicoId { get; set; }
        ////public RegistroMedico RegistroMedico { get; set; }

        public int? FacilitadorId { get; set; }
        ////public Facilitador Facilitador { get; set; }

        public int? ServicioId { get; set; }
        //public Servicio Servicio { get; set; }
        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; }

        [Display(Name = "Síntomas")]
        [Required]
        public string Sintomas { get; set; }

        [Display(Name = "Teléfono")]
        //[Required]
        public string Telefono { get; set; }

        [Display(Name = "Tiene Tarjeta")]
        [UIHint("Bool")]
        public bool TieneTarjeta { get; set; }
        public string Observaciones { get; set; }

        [Display(Name = "Facilitador")]
        public string NombreFacilitador { get; set; }
    }
}
