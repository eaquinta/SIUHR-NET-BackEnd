using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.InicialAbastecimiento.DTOs
{
    public class InicialAbastecimientoDTOBase
    {
        public int MaterialId { get; set; }
        public int ProveedorId { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
    }
}
