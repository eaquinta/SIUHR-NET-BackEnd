//Apphr.Application.Kardex.DTOs.KardexDTOrptKardex
using System;

namespace Apphr.Application.Kardex.DTOs
{
    public class KardexDTOrptKardex : KardexDTOBase
    {
        public string MaterialCodigo { get; set; }
        public string MaterialDescripcion { get; set; }
        public string RefPac_NumHCAntiguo { get; set; }
        public string DestinoCodigo { get; set; }
        public string PersonaName { get; set; }
        //public string DocumentoReferencia { get; set; }        
        public decimal ValorAnterior { get; set; }
        public decimal CantidadAnterior { get; set; }
        public decimal CostoUnitarioAnterior { get; set; }
        public decimal ValorActual { get; set; }
        public decimal CantidadActual { get; set; } 
        public decimal CostoUnitarioActual { get; set; }
        public string DocLin
        {
            get
            {
                return Convert.ToInt16(Documento.Substring(5,6)).ToString()+"/"+Line.ToString();
            }
        }
        public string MovimientoObservacion
        {
            get
            {
                string res = "";
                switch (TipoDocumento)
                {
                    case "Ing.Inv.":
                        res = "425115-6 295 " + DocumentoReferencia;
                        break;
                    case "Des.Inv.":
                        res = DestinoCodigo + " " + DocumentoReferencia + " " + (RefPac_NumHCAntiguo == null ? "" : "RM:" + RefPac_NumHCAntiguo) + " " + PersonaName;
                        break;
                    default:
                        
                        break;
                }
                return res;
            }
        }
    }
}
