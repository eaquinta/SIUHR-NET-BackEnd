using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models.Entities.Ortopedia;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOBase
    {
        public Int64 HojaGastoId { get; set; }                          // HojaGastoId

        [Required]                                                      // HojaControlSala
        [Display(Name = "Hoja Control No.")]
        public string HojaControlSala { get; set; }

        [DataType(DataType.Date)]                                       // Fecha
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]                                       // FechaOperacion
        [Display(Name = "Fecha Operación")]
        public DateTime FechaOperacion { get; set; }

        [Display(Name = "Observación")]                                 // Observacion
        [DataType(DataType.MultilineText)]
        public string Observacion { get; set; }

        [MaxLength(100)]                                                // AuxiliarEnfermeriaInstrumentista
        [Display(Name = "Auxiliar de enfermeria instrumentista")]
        public string AuxiliarEnfermeriaInstrumentista { get; set; }

        [MaxLength(100)]                                                // AuxiliarEnfermeriaCirculante
        [Display(Name = "Auxiliar de enfermeria Circulante")]
        public string AuxiliarEnfermeriaCirculante { get; set; }

        [Display(Name = "Servicio")]
        [UIHint("DropDownBtn")]
        public int ServicioId { get; set; }                             // Servicio
        public Servicio Servicio { get; set; }

        [Display(Name = "Cirujano")]
        [UIHint("DropDownBtn")]
        public int CirujanoId { get; set; }                             // Servicio
        public ORTCirujano Cirujano { get; set; }

        [StringLength(25)]
        public string Cama { get; set; }                                // Cama

        public int Lineas { get; set; }                                 // Lineas

        public Int64? PacienteId { get; set; }                          // PacienteId
        public ORTPaciente Paciente { get; set; }
    }
}
