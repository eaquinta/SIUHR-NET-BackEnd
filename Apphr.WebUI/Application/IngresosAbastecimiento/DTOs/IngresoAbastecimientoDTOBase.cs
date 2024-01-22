using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.IngresosAbastecimiento.DTOs
{
    public class IngresoAbastecimientoDTOBase
    {
        public int IngresoAbastecimientoId { get; set; }


        public int MaterialId { get; set; }


        [MaxLength(15)]
        [Required]
        public string OrdenCompraId { get; set; }


        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public Decimal Cantidad { get; set; }
        public int ProveedorId {get;set;}
    }
}
