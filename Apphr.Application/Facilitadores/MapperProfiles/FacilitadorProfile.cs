using Apphr.Application.Facilitadores.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.Facilitadores.MapperProfiles
{
    public class FacilitadorProfile : Profile
    {
        public FacilitadorProfile()
        {
            CreateMap<Facilitador, FacilitadorDTOEdit>().ReverseMap();
            CreateMap<Facilitador, FacilitadorDTOCreate>().ReverseMap();
            CreateMap<Facilitador, FacilitadorDTODetails>().ReverseMap();
            CreateMap<Facilitador, FacilitadorDTOBase>().ReverseMap();
            CreateMap<Facilitador, FacilitadorDTODelete>().ReverseMap();
        }
    }
}
