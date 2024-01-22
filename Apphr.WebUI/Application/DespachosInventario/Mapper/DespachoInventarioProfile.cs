using Apphr.Application.DespachosInventario.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.DespachosInventario.Mapper
{
    class DespachoInventarioProfile : Profile
    {
        public DespachoInventarioProfile()
        {
            CreateMap<DespachoInventario, DespachoInventarioDTOBase>().ReverseMap();
            CreateMap<DespachoInventario, DespachoInventarioDTOCEdit>().ReverseMap();                        
            CreateMap<DespachoInventario, DespachoInventarioDTODetails>().ReverseMap();
            CreateMap<DespachoInventario, DespachoInventarioDTOIxRow>().ReverseMap();

            CreateMap<DespachoInventarioDetalle, DespachoInventarioDetalleDTOBase>().ReverseMap();
            CreateMap<DespachoInventarioDetalle, DespachoInventarioDetalleDTODetails>().ReverseMap();
            CreateMap<DespachoInventarioDetalle, DespachoInventarioDetalleDTOCEdit>().ReverseMap();
        }
    }
}
