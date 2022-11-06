using Apphr.Domain.Entities;
using PagedList;

namespace Apphr.Application.Bodegas.DTOs
{
    public class BodegaDTOIndex
    {
        public BodegaDTOIxFilter F { get; set; }
        public IPagedList<Bodega> Result { get; set; }
    }
}
