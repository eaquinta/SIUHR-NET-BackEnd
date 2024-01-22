using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Consultas.DTOs
{
    public class ConsultasDTOSaldoIxFilter : ConsultaDTOBaseIxFilter
    {
        [Display(Name = "Saldos Por")]
        [UIHint("DropDown")]
        public int TipoExistencia { get; set; }
    }
}
