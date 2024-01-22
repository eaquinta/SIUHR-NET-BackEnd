using Apphr.Domain.Common;
using Apphr.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("RegistrosMedicos")]
    public class RegistroMedico : AuditableEntity
    {
        //[Key]
        //[ScaffoldColumn(false)]
        //public Guid RegMedId { get; set; }

        [Key]
        [ScaffoldColumn(false)]
        [StringLength(20)]
        public string RegistroMedicoId { get; set; }

        [ForeignKey("Persona")]
        public int? PersonId { get; set; }
                
        public Persona Persona { get; set; }     


    }
}
