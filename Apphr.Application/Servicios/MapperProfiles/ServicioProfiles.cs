using Apphr.Application.Servicios.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.Servicios.MapperProfiles
{
    class ServicioProfiles : Profile
    {
        public ServicioProfiles()
        {
            CreateMap<Servicio, ServicioDTOEdit>().ReverseMap();
            CreateMap<Servicio, ServicioDTOCreate>().ReverseMap();
            CreateMap<Servicio, ServicioDTODetails>().ReverseMap();
            CreateMap<Servicio, ServicioDTOBase>().ReverseMap();
        }
    }
}
