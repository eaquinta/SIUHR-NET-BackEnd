using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Entities
{
    [Table("InicialAbastecimiento")]
    public class InicialAbastecimiento
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        public decimal Cantidad { get; set; } = 0;
    }
}
