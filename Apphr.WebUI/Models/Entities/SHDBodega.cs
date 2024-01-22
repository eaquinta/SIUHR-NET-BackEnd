using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("SHDBodegas")]
    public class SHDBodega
    {
        [Key]                                               // BodegaId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BodegaId { get; set; }

        [Index("IX_Bodega", Order = 1, IsUnique = true)]   // AdministracionId (<<)
        [ForeignKey("Administracion")]
        public int AdministracionId { get; set; }
        public SHDAdministracion Administracion { get; set; }

        [StringLength(5)]                                   // [CODIGO]
        [Index("IX_Bodega", Order = 2, IsUnique = true)]
        public string Codigo { get; set; }

        
        [StringLength(40)]                                  // [DESCRI]
        public string Descripcion { get; set; }
        
    }
}
