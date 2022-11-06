using Apphr.Application.Common;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOIxFilter : IxFilter
    {
        public string Nit { get; set; }
        public string Contacto { get; set; }
        public string Descripcion { get; set; }
    }
}
