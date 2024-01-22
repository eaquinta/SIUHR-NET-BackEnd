using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Destinos.DTOs
{
    public class DestinoDTOBase
    {
        public int DestinoId { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public string ADMINI { get; set; }

        [Display(Name = "Jefe Servicio")]
        public string JefeServicio { get; set; }
    }
}
