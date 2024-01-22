using Apphr.Domain.Common;
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
    [Table("Personas")]
    public class Persona : AuditableEntity
    {
        [Key]
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string MatriName { get; set; }

        public string MarriedName { get; set; }

        public DateTime? Birth { get; set; }

        public Gender? Gender { get; set; }

        [LogIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name { get; private set; }

        public string Image { get; set; }

        public DateTime? ImageDate { get; set; }

        public EstadoCivil? EstadoCivil { get; set; }

        public Religion? Regligion { get; set; }

        public Etnia? Etnia { get; set; }

        public string PersonIdRef { get; set; }
        public string PersonIdRefOrigen { get; set; }

        public bool IsActive { get; set; }

        //public ICollection<Facilitador> Facilitadores { get; set; }
    }
}