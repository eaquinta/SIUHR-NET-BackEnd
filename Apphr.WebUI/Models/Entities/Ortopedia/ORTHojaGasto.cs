using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTHojasGasto")]
    public class ORTHojaGasto : AuditableEntity
    {
        [Key]                                                           // HojaGastoId
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 HojaGastoId { get; set; }

        [DataType(DataType.Date)]                                       // Fecha
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]                                       // FechaOperacion
        public DateTime FechaOperacion { get; set; }

        [StringLength(10)]                                              // HojaControlSala
        public string HojaControlSala { get; set; }

        public string Observacion { get; set; }                         // Observacion

        [StringLength(100)]                                             // AuxiliarEnfermeriaInstrumentista
        public string AuxiliarEnfermeriaInstrumentista { get; set; }

        [StringLength(100)]                                             // AuxiliarEnfermeriaCirculante
        public string AuxiliarEnfermeriaCirculante { get; set; }

        [ForeignKey("Servicio")]                                        // Servicio
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }

        [ForeignKey("Cirujano")]                                        // Cirujano
        public int CirujanoId { get; set; }
        public ORTCirujano Cirujano { get; set; }

        [ForeignKey("Paciente")]
        public Int64? PacienteId { get; set; }                          // PacienteId
        public ORTPaciente Paciente { get; set; }

        [MaxLength(25)]
        public string Cama { get; set; }                                // Cama

        public int Lineas { get; set; }

        public Int64? DespachoInventarioId { get; set; }
        public DateTime? DespachoInventarioFecha { get; set; }
    }
}
