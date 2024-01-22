using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities.Ortopedia
{
    public class ORTIngresoInventario : AuditableEntity
    {
        [Key]                                                                   // IngresoInventarioId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 IngresoInventarioId { get; set; }

        [ForeignKey("OrdenCompra")]
        public Int64 OrdenCompraId { get; set; }

        public ORTOrdenCompra OrdenCompra { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }

        public int Lineas { get; set; }                                         // Lineas
    }
}
