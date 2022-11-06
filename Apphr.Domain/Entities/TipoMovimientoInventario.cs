using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{

    [Table("TiposMovimientoInventario")]
    public class TipoMovimientoInventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoMovimientoInventarioId { get; set; }
        [MaxLength(25)]
        public string Nombre { get; set; }
        [MaxLength(8)]
        public string NombreCorto { get; set; }
        public int Efecto { get; set; }
    }
}
