using Apphr.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("MovimientosAbastecimiento")]
    public class MovimientoAbastecimiento : AuditableEntity
    {
        [Key]
        public int MovimientoAbastecimientoId { get; set; }

        [MaxLength(3)]
        public string TipoMovimiento { get; set; }
        public int Efecto { get; set; }
        //[Key]
        //[Column(Order = 0)]
        public int DestinoId { get; set; }
        //[Key]
        //[Column(Order = 1)]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        [ForeignKey("Proveedor")]
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }


        // ------------------- Ingresos 
        [MaxLength(15)]        
        public string SolicitudPedidoId { get; set; }

        [MaxLength(15)]
        public string OrdenCompraId { get; set; }


        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime? FechaIngreso { get; set; }

        // ------------------- Egressos

        [MaxLength(15)]
        public string SolicitudMaterialSalaId { get; set; }

        public int? SolicitudMaterialSalaDetalleId { get; set; }

        [Display(Name = "Fecha Egreso")]
        [DataType(DataType.Date)]
        public DateTime? FechaEgreso { get; set; }


        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        public Decimal Cantidad { get; set; }
    }
}