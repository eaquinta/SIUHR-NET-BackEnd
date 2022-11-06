using Apphr.Application.Common;
using Apphr.Domain.EntitiesDBF;
using PagedList;

namespace Apphr.Application.Bodegas.DTOs
{
    public class BodegaDTOIndexDBF
    {
        public IxFilter F { get; set; }
        public IPagedList<BodegaDBF> Result { get; set; }
    }
}
