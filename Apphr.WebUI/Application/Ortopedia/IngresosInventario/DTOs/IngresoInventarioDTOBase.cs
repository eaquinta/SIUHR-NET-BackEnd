using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOBase
    {
        [Display(Name = "Ingreso No.")]
        public Int64 IngresoInventarioId { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public Int64 OrdenCompraId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }
    }
}
