using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Common.Models
{
    public class SADCPacienteDTOBase
    {
        public decimal pac_numhc { get; set; }
        public decimal pac_numhcantiguo { get; set; }
        public string per_primernombre { get; set; }
        public string Per_SegundoNombre { get; set; }
        public string Per_PrimerApellido { get; set; }
        public string Per_SegundoApellido { get; set; }
        public string Per_ApellidoCasada { get; set; }
        public string NombreCompleto
        {
            get
            {
                return per_primernombre + " " + Per_SegundoNombre + " " + Per_PrimerApellido + " " + Per_SegundoApellido + " " + Per_ApellidoCasada;
            }
        }
    }
}
