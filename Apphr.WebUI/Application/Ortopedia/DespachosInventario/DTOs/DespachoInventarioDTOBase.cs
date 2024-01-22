using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.DespachosInventario.DTOs
{
    public class DespachoInventarioDTOBase
    {
        [Display(Name = "Despacho No.")]
        public Int64 DespachoInventarioId { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }
    }
}
