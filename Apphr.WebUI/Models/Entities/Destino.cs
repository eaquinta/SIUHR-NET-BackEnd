using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Destinos")]
    public class Destino
    {
        [Key]
        public int DestinoId { get; set; }

        [Index("IX_Codigo", IsUnique = true)]
        [MaxLength(10)]
        public string Codigo { get; set; }        
        public string Descripcion { get; set; }
        public string ADMINI { get; set; }
        public string JefeServicio { get; set; }
    }
}
