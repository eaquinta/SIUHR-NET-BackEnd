using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOExistenciaInicial
    {
        public int MaterialId { get; set; }
        public int DestinoId { get; set; }
        [Display(Name = "Código")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Descripción")]
        public string MaterialDescripcion { get; set; }
        public decimal Inicial { get; set; } = 0;
    }
}
