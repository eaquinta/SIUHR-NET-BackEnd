using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Usuarios.DTOs
{
    public class UsuarioDTOIxFilter
    {
        [Display(Name = "Buscar")]
        public string Buscar { get; set; }
    }
}
