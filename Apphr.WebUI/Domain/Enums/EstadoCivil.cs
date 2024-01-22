using System.ComponentModel.DataAnnotations;

namespace Apphr.Domain.Enums
{
    public enum EstadoCivil
    {
        [Display(Name = "Ninguno")]
        Ninguno,
        [Display(Name = "Soltero(a)")]
        Soltero,
        [Display(Name = "Casado(a)")]
        Casado,
        [Display(Name = "Union")]
        Union
    }
}