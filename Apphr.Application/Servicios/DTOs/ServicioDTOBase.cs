using Apphr.Application.Common.DTOs;
using Apphr.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Servicios.DTOs
{
    public class ServicioDTOBase : ActionResultSingleDTOBase
    {
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(25, ErrorMessage = "Nombre debe ser menor a 25 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        //[StringLength(25)]
        [Display(Name = "Area")]
        public Area Area { get; set; }
    }
}
