using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("PacienteEventos")]
    public class PacienteEvento : AuditableEntity
    {
        [Key]
        public int PacienteEventoId { get; set; }
        public string Procedencia { get; set; }
        public string NombrePaciente { get; set; }
        public int? Edad { get; set; }
        public string NombreFamiliar { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaEgreso { get; set; }
        public int? Cama { get; set; }
        public string Registro { get; set; }

        [ForeignKey("RegistroMedico")]
        public string RegistroMedicoId { get; set; }
        public RegistroMedico RegistroMedico { get; set; }

        //[ForeignKey("Facilitador")]
        public int? FacilitadorId { get; set; }
        //public Facilitador Facilitador { get; set; }

        [ForeignKey("Servicio")]
        public int? ServicioId { get; set; }
        public Servicio Servicio { get; set; }
        public string Diagnostico { get; set; }
        public string Sintomas { get; set; }        
        public string Telefono { get; set; }
        public bool TieneTarjeta { get; set; }
        public string Observaciones { get; set; }

        public ICollection<PacienteEventoHistorial> Historial { get; set; }
    }
}