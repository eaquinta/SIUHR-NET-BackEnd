using Apphr.Application.Common;
using PagedList;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOIndex
    {
        public IxFilter F { get; set; }
        public PagedList<SolicitudPedidoDTOBase> Result { get; set; }
    }
}
