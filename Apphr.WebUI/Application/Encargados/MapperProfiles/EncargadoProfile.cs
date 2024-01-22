using Apphr.Application.Encargados.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.Encargados.MapperProfiles
{
    public class EncargadoMapperProfile : Profile
    {
        public EncargadoMapperProfile()
        {
            CreateMap<Encargado, EncargadoDTOEdit>().ReverseMap();
            CreateMap<Encargado, EncargadoDTOCreate>().ReverseMap();
            CreateMap<Encargado, EncargadoDTOView>().ReverseMap();
        }
    }
}
