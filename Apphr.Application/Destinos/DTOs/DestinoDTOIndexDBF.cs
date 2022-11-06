using Apphr.Application.Common;
using Apphr.Domain.EntitiesDBF;
using PagedList;

namespace Apphr.Application.Destinos.DTOs
{
    public class DestinoDTOIndexDBF
    {
        public IxFilter F { get; set; }
        public IPagedList<DestinoDBF> Result { get; set; }
    }
}
