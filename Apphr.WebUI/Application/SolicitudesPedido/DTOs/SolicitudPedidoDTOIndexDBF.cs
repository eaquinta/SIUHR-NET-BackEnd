using Apphr.Application.Common;
using Apphr.Domain.EntitiesDBF;
using PagedList;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOIndexDBF
    {
        public IxFilter F { get; set; }
        public IPagedList<SolicitudPedidoDBF> Result {get;set;}
    }
}
