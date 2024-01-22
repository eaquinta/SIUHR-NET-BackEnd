using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.AjustesInventario.DTOs
{
    public class AjusteInventarioDTOBase
    {
        [Display(Name = "Ajuste No.")]
        public Int64 AjusteInventarioId { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }
    }
}
