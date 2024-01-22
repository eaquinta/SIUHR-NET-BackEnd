using Apphr.Application.Common.DTOs;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOIxFilter : DTOJsIxFilter
    {
        public string Nit { get; set; }
        public string Contacto { get; set; }
        public string Descripcion { get; set; }
    }
}
