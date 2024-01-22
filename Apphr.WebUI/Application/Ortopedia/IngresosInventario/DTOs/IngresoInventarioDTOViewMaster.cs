using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOViewMaster : IngresoInventarioDTOBase
    {
        [Display(Name = "Orden Compra #")]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime? FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }

        [Display(Name = "Solicitud Pedido #")]
        [Required]
        public int NumeroSP { get; set; }

        [Display(Name = "Fecha S.P.")]
        [DataType(DataType.Date)]
        public DateTime? FechaSP { get; set; }

        [Display(Name = "Año S.P.")]
        public int AnioSP { get; set; }
        public List<MovimientoDTOBase> Detalle { get; set; }
    }
}
