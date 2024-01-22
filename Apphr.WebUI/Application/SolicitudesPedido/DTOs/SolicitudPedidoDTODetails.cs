using System.Collections.Generic;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTODetails : SolicitudPedidoDTOBase
    {
		public List<SolicitudPedidoDetalleDTOBase> Detalle { get; set; }
	}
}
