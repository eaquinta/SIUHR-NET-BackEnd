using AutoMapper;
using Apphr.Application.Personas.DTOs;
using Apphr.WebUI.Models.Entities;

namespace Apphr.Application.Personas.MapperProfiles
{
    public class PersonaMapperProfile : Profile
    {
        public PersonaMapperProfile()
        {
            CreateMap<Persona, PersonaDTOCEdit>().ReverseMap();
            CreateMap<Persona, PersonaDTOCreate>().ReverseMap();
            CreateMap<Persona, PersonaDTOView>().ReverseMap();
            CreateMap<Persona, PersonaDTOBase>().ReverseMap();
        }
    }
}
