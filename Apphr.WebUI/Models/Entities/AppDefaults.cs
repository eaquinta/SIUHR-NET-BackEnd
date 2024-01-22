using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("AppDefaults")]
    public class AppDefault
    {
        [Key]
        public int AppDefaultId { get; set; }


        [Index("IX_Name", IsUnique = true)]
        [MaxLength(25), Required]
        public string Name { get; set; }


        public string Value { get; set; }
    }
}
