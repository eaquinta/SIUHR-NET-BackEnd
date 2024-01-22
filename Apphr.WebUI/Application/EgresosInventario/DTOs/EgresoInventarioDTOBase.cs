using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.EgresosInventario.DTOs
{
    public class EgresoInventarioDTOBase
    {
        public string EgresoInventarioId { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime Fecha { get; set; }
    }
}
