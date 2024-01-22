using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Materiales.DTOs
{
    public class MaterialDTOBaseExt : MaterialDTOBase
    {
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal? Existencia { get; set; }
    }
}
