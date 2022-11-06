using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("ExistenciasBodega")]
    public class ExistenciaBodega
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExistenciaBodegaId { get; set; }
        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        [ForeignKey("Proveedor")]
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        [MaxLength(20)]
        public string Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Valor { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Maximo { get; set; }
        public decimal? Pendiente { get; set; }
    }
}
