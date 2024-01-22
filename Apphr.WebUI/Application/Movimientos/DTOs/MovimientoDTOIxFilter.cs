using Apphr.Application.Common.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Movimientos.DTOs
{
    public class MovimientoDTOIxFilter : DTOJsIxFilter
    {
        [Display(Name = "De Fecha")]
        [DataType(DataType.Date)]
        public DateTime? DFec { get; set; }

        [Display(Name = "A Fecha")]
        [DataType(DataType.Date)]
        public DateTime? AFec { get; set; }

        [UIHint("DropDown")]
        [Display(Name = "Bodega")]

        public string CodigoBodega { get; set; }

        [UIHint("DropDown")]
        [Display(Name = "Material")]
        public string CodigoMaterial { get; set; }

        public int Year { get; set; }
    }
}
