using Apphr.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("SHDExistenciaTotal")]
    public class SHDExistenciaTotal : AuditableEntity
    {
        [Key]                                                           // Id (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Index("IX_ExistenciaTotal", Order = 1, IsUnique = true)]       // AdministracionId (<<)
        [ForeignKey("Administracion")]
        public int AdministracionId { get; set; }
        public SHDAdministracion Administracion { get; set; }

        [ForeignKey("Material")]                                        // IdMaterial (<<)
        public int IdMaterial { get; set; }
        public SHRMaterial Material { get; set; }

        [StringLength(14)]                                              // [CODIGO]
        [Index("IX_ExistenciaTotal", Order = 2, IsUnique = true)]
        public string CodigoMaterial { get; set; }

        [StringLength(5)]                                               // [CODIGI]
        [Index("IX_ExistenciaTotal", Order = 3, IsUnique = true)]
        public string CodigiMaterial { get; set; }        

        public decimal Cantidad { get; set; }                           // [CANTI]

        public decimal Valor { get; set; }                              // [VALOR]
    }
}
