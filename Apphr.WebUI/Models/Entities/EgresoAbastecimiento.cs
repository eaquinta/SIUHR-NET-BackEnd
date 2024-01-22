using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("EgresosAbastecimiento")]
    public class EgresoAbastecimiento 
    {
        [Key]
        public int EgresoAbastecimientoId { get; set; }



        [MaxLength(15)]
        public string SolicitudMaterialSalaId { get; set; }

        public int SolicitudMaterialSalaDetalleId { get; set; }

        public int MaterialId { get; set; }


        [MaxLength(15)]
        public string OrdenCompraId { get; set; }


        [Display(Name = "Fecha Egreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        public Decimal Cantidad { get; set; }
    }
}
