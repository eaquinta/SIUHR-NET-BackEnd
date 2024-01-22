using Apphr.WebUI.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOBase
    {
        [Display(Name = "Devolucion Id.")]
        public Int64 DevolucionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }                                     // Fecha

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        [Display(Name ="Proveedor")]
        [UIHint("DropDown")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

    }
}
