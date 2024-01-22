using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    public class ORTDespachoInventario : AuditableEntity
    {
        [Key]                                                                   // DespachoInventarioId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 DespachoInventarioId { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }

        public int Lineas { get; set; }                                         // Lineas

    }
}
