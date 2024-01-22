using System;

namespace Apphr.Application.Common.Models
{
    public class SADCPacienteDTOBase
    {
        public Int64 pac_numhc { get; set; }
        public Int64 pac_numhcantiguo { get; set; }
        public string per_primernombre { get; set; }
        public string Per_SegundoNombre { get; set; }
        public string Per_PrimerApellido { get; set; }
        public string Per_SegundoApellido { get; set; }
        public string Per_ApellidoCasada { get; set; }
        public DateTime Per_FechaNacimiento { get; set; }
        public string NombreCompleto
        {
            get
            {
                return per_primernombre + " " + Per_SegundoNombre + " " + Per_PrimerApellido + " " + Per_SegundoApellido + " " + Per_ApellidoCasada;
            }
        }
    }
}
