using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.EgresosAbastecimiento.DTOs
{
    public class EgresoAbastecimientoDTOBase
    {
        public int EgresoAbastecimientoId { get; set; }


        [Display(Name = "Hoja de Control")]
        [MaxLength(15)]
        public string SolicitudMaterialSalaId { get; set; }


        public int MaterialId { get; set; }


        [MaxLength(15)]
        public string OrdenCompraId { get; set; }


        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        public Decimal Cantidad { get; set; }
    }
}
