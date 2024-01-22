using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTPacientes")]
    public class ORTPaciente : AuditableEntity
    {
        [Key]                                                                   // PacienteId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 PacienteId { get; set; }

        [StringLength(250)]                                                     // Nombre
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        // Datos de Sistema SADC        
        public Int64? RefPac_NumHC { get; set; }
        public Int64? RefPac_NumHCAntiguo { get; set; }
    }
}
