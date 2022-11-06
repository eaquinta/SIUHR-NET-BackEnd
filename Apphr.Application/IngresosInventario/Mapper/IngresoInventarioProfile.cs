using Apphr.Application.IngresosInventario.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.IngresosInventario.Mapper
{
    public class IngresoInventarioProfile : Profile
    {
        public IngresoInventarioProfile()
        {
            CreateMap<IngresoInventario, IngresoInventarioDTOCEdit>().ReverseMap();
            CreateMap<IngresoInventario, IngresoInventarioDTOIxRow>().ReverseMap();
            CreateMap<IngresoInventario, IngresoInventarioDTODetails>().ReverseMap();
            CreateMap<IngresoInventario, IngresoInventarioDTOBase>().ReverseMap();
            CreateMap<IngresoInventarioDetalle, IngresoInventarioDetalleDTOBase>().ReverseMap();
            CreateMap<IngresoInventarioDetalle, IngresoInventarioDetalleDTOCEdit>().ReverseMap();
            CreateMap<IngresoInventarioDetalle, IngresoInventarioDetalleDTODetails>().ReverseMap();
        }
    }
}
