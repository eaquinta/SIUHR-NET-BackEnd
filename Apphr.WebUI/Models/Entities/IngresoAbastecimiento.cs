using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("IngresosAbastecimiento")]
    public class IngresoAbastecimiento
    { 
        [Key]
        public int IngresoAbastecimientoId { get; set; }


        public int MaterialId { get; set; }


        [MaxLength(15)]
        public string OrdenCompraId { get; set; }


        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        public Decimal Cantidad { get; set; }
    }
}
