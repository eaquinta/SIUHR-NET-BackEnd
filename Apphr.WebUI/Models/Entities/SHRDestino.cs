using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    public class SHRDestino
    {
        [Key]                                                   // DestinoId (*)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DestinoId { get; set; }        

        [Display(Name = "Código")]                              // [CODIGO] Codigo
        [StringLength(5)]
        public string Codigo { get; set; }

        [Display(Name = "Descripción")]                         // [DESCRI] Descripción
        [StringLength(30)]
        public string Descripcion { get; set; }

        [StringLength(2)]                                       // [ADMINI] Administracion
        public string ADMINI { get; set; }                      

        [Display(Name = "Jefe Servicio")]                       // [JEFSER] JefeServicio
        [StringLength(25)]
        public string JefeServicio { get; set; }
    }
}
