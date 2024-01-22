using Apphr.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    [Table("ORTCirujanos")]
    public class ORTCirujano : AuditableEntity
    {
        [Key]                                                                   // CirujanoId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CirujanoId { get; set; }
        
        [StringLength(100)]                                                     // Nombre
        public string Nombre { get; set; }
    }
}
