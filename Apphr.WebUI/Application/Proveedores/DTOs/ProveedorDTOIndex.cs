using PagedList;
using Apphr.Application.Common;
using Apphr.WebUI.Models.Entities;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOIndex
    {
        public ProveedorDTOIxFilter F { get; set; }
        public IPagedList<ProveedorDTOBase> Result { get; set; }
    }
}
