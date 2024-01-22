using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Common.DTOs
{
    public class DTOIxFilterBodMat
    {
        public int? Page { get; set; }
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


        [Display(Name = "Código Material")]
        [Remote("JsMaterialCodigoExist", "Materiales",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
        public int MaterialId { get; set; }
    }
}
