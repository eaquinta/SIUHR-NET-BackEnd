using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Kardex.DTOs
{
    public class KardexDTOCierre
    {
        public int BodegaId { get; set; }
        [Remote("JsNombreExist", "Bodegas",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        [Display(Name = "Nombre Bodega")]
        [Required]
        public string BodegaNombre { get; set; }
        [Display(Name = "Descipción")]
        public string BodegaDescripcion { get; set; }
    }
}
