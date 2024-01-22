using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Encargados.DTOs
{
    public class EncargadoDTOBase
    {
        public int EncargadoId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nombre debe ser menor a 50 caracteres.")]
        public string Nombre { get; set; }
    }
}
