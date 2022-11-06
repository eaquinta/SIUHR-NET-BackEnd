using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{
    [Table("Encargados")]
    public class Encargado : AuditableEntity
    {
        [Key]
        public int EncargadoId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Nombre debe ser menor a 50 caracteres.")]
        public string Nombre { get; set; }

        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
