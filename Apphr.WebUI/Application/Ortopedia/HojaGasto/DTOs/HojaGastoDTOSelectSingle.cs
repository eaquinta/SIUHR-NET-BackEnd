using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOSelectSingle
    {
        [Display(Name = "Hoja de Gasto")]
        [UIHint("DropDown")]
        public Int64 HojaGastoId { get; set; }

        public Int64 DevolucionId { get; set; }
        public int ProveedorId { get; set; }
        public DateTime Fecha { get; set; }
        //public Int64 DespachoInventarioId { get; set; }
    }
}