using Apphr.Domain.Common.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Bodegas.DTOs
{
    public class BodegaDTOBaseDBF
    {
        [Display(Name = "Código")]
        [MaxLength(5), Required]
        //[UpperCase]
        public string CODIGO { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(40), Required]
        public string DESCRI { get; set; }

        [Display(Name = "Procedencia")]
        //[HasValidValues("H,P,D,M,R", ErrorMessage = "Debe ser H")]
        [IncludeOnlyChar("H,P,D,M,R"), MaxLength(1), Required]
        public string PROCED { get; set; }

        [MaxLength(5)]
        [Display(Name = "Bodega Materiales")]
        public string BODMAT { get; set; }
    }
}
