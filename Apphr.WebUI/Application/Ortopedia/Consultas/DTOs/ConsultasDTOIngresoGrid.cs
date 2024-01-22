using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultasDTOIngresoGrid : MovimientoDTOBase
    {
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }

        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Orden Compra #")]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }

        public string OCData { get { return NumeroOC + " (" + AnioOC + ") " + FechaOC.ToString("dd/MM/yyyy"); } }
        public string SPData { get { return NumeroSP + " (" + AnioSP + ") " + FechaSP.ToString("dd/MM/yyyy"); } }


        [Display(Name = "Solicitud Pedido #")]        
        public int NumeroSP { get; set; }

        [Display(Name = "Fecha S.P.")]
        [DataType(DataType.Date)]
        public DateTime FechaSP { get; set; }

        [Display(Name = "Año S.P.")]
        public int AnioSP { get; set; }

        public string MaterialSiges { get; set; }
        public string TipoOC { get; set; }
        public decimal? CantidadSolicitada { get; set; }

        public string TipoEvento { get; set; }

    }
}
