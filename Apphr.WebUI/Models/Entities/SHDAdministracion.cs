using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("SHDAdministraciones")]
    public class SHDAdministracion
    {
        [Key]                                                   // AdministracionId (*)
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdministracionId { get; set; }

        [StringLength(10)]                                      // Codigo
        public string Nombre { get; set; }

        [StringLength(100)]                                     // Descripcion
        public string Descripcion { get; set; }

        //[StringLength(255)]                                     // AccessRoles
        public int AccessRole { get; set; }                 

        //[NotMapped]
        //public List<int> AccessRolesList
        //{
        //    get
        //    {
        //        return JsonConvert.DeserializeObject<List<int>>(AccessRoles);
        //    }
        //    set
        //    {
        //        AccessRoles = JsonConvert.SerializeObject(value);
        //    }
        //}
    }
}
