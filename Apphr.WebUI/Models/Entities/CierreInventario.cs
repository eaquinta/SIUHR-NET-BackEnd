using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Entities
{
    [Table("CierresInventario")]
    public class CierreInventario
    {
        [Key]
        public Int64 CierreInventarioId { get; set; }
        [Index("IX_MaterialFecha", Order = 1, IsUnique = true)]
        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }
        [Index("IX_MaterialFecha", Order = 2, IsUnique = true)]
        [ForeignKey("Material")]
        public int  MaterialId { get; set; }
        public  Material Material { get; set; }
        [Index("IX_MaterialFecha", Order = 3, IsUnique = true)]
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }
    }
}
