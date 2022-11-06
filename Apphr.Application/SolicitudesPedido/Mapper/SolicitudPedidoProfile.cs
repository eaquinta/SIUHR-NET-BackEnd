using Apphr.Application.SolicitudesPedido.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.SolicitudesPedido.Mapper
{
    public class SolicitudPedidoProfile : Profile
    {
        public SolicitudPedidoProfile()
        {
            CreateMap<SolicitudPedido, SolicitudPedidoDTOBase>().ReverseMap();
            CreateMap<SolicitudPedido, SolicitudPedidoDTODetails>().ReverseMap();
            CreateMap<SolicitudPedido, SolicitudPedidoDTOCEdit>().ReverseMap();
            CreateMap<SolicitudPedidoDetalle, SolicitudPedidoDTOBaseDT>().ReverseMap();
        }
    }
}
