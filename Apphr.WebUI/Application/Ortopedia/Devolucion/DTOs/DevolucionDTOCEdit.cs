using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOCEdit : DevolucionDTOBase
    {
        [Display(Name ="Nit")]
        public string ProveedorNit { get; set; }

        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
    }
}
