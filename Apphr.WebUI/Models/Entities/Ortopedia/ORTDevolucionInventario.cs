using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{ 
    [Table("ORTDevoluciones")]
    public class ORTDevolucionInventario : AuditableEntity
    {
        [Key]                                                                   // AjusteInventarioId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 DevolucionId { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }

        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

    }
}
