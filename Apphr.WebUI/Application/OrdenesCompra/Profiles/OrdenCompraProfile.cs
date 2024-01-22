using Apphr.Application.OrdenesCompra.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.OrdenesCompra.Profiles
{
    class OrdenCompraProfile : Profile
    {
        public OrdenCompraProfile()
        {
            CreateMap<OrdenCompra, OrdenCompraDTOBase>().ReverseMap();
            CreateMap<OrdenCompra, OrdenCompraDTODetails>().ReverseMap();
            CreateMap<OrdenCompraDetalle, OrdenCompraDetalleDTOBase>().ReverseMap();
            CreateMap<OrdenCompraDetalle, OrdenCompraDetalleDTODetails>().ReverseMap();
        }
    }
}
