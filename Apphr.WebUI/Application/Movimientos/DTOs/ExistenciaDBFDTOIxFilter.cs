using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Movimientos.DTOs
{
    public class ExistenciaDBFDTOIxFilter
    {
        public int Year { get; set; }
        [Display(Name = "Hasta la Fecha")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime AFec { get; set; }

        [Required]
        public string Bodega { get; set; }
        [Display(Name ="Codigo de Material (opcional)")]
        public string Material { get; set; }
        public bool isDetail { get; set; }
    }
}
