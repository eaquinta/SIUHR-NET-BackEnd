using Apphr.Application.Common;
using Apphr.Domain.EntitiesDBF;
using PagedList;

namespace Apphr.Application.Materiales.DTOs
{
    public class MaterialDTOIndexDBF
    {
        public IxFilter F { get; set; }
        public IPagedList<MaterialDBF> Result { get; set; }
    }
}
