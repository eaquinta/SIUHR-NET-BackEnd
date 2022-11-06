using AutoMapper;
using Apphr.Application.Personas.DTOs;
using Apphr.Domain.Entities;

namespace Apphr.Application.Personas.MapperProfiles
{
    public class PersonaMapperProfile : Profile
    {
        public PersonaMapperProfile()
        {
            CreateMap<Persona, PersonaDTOEdit>().ReverseMap();
            CreateMap<Persona, PersonaDTOCreate>().ReverseMap();
            CreateMap<Persona, PersonaDTOView>().ReverseMap();
            CreateMap<Persona, PersonaDTOBase>().ReverseMap();
        }
    }
}
