using Apphr.Application.Common;
using Apphr.Domain.EntitiesDBF;
using PagedList;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOIndexDBF
    {
        public IxFilter F { get; set; }
        public IPagedList<ProveedorDBF> Result { get; set; }
    }
}
