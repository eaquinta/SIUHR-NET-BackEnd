using System;
using System.ComponentModel.DataAnnotations;

//Apphr.Application.Kardex.DTOs.KardexDTOSaldo
namespace Apphr.Application.Kardex.DTOs
{
    public class KardexDTOSaldo
    {
        public int MaterialId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostoUnitario { get { return decimal.Round((Cantidad == 0) ? 0 : (Valor / Cantidad), 2); } }
        
    }
}
