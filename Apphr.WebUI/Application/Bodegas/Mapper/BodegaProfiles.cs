using Apphr.Application.Bodegas.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.Bodegas.Mapper
{
    class BodegaProfiles : Profile
    {
        public BodegaProfiles()
        {
            CreateMap<Bodega, BodegaDTOCEdit>().ReverseMap();
            CreateMap<Bodega, BodegaDTOCreate>().ReverseMap();
            CreateMap<Bodega, BodegaDTODelete>().ReverseMap();
            CreateMap<Bodega, BodegaDTOView>().ReverseMap();
            CreateMap<Bodega, BodegaDTOBase>().ReverseMap();

            CreateMap<Bodega, BodegaDBF>().ReverseMap()
                .ForMember(x => x.Nombre, o => o.MapFrom(s => s.CODIGO))
                .ForMember(x => x.Procedencia, o => o.MapFrom(s => s.PROCED))
                .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRI));
        }
    }
}
