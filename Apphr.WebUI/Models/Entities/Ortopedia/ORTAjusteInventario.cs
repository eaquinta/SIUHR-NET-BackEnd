using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    public class ORTAjusteInventario : AuditableEntity
    {
        [Key]                                                                   // AjusteInventarioId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 AjusteInventarioId { get; set; }

        public DateTime Fecha { get; set; }                                     // Fecha

        public string Observacion { get; set; }                                 // Observcion

        public int Lineas { get; set; }                                         // Lineas
    }
}
