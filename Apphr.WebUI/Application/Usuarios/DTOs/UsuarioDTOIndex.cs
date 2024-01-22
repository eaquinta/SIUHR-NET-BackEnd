using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Usuarios.DTOs
{
    public class UsuarioDTOIndex
    {
        public UsuarioDTOIxFilter F { get; set; }
        public IEnumerable<UsuarioDTOIxRow> Result { get; set; }       

    }    
}
