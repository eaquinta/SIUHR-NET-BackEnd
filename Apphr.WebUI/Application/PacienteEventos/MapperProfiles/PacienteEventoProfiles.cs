using Apphr.Application.PacienteEventos.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.PacienteEventos.MapperProfiles
{
    public class PacienteEventoProfiles : Profile
    {
        public PacienteEventoProfiles()
        {
            CreateMap<PacienteEvento, PacienteEventoDTOEdit>().ReverseMap();
            CreateMap<PacienteEvento, PacienteEventoDTOCreate>().ReverseMap();
            CreateMap<PacienteEvento, PacienteEventoDTODetail>().ReverseMap();
            CreateMap<PacienteEvento, PacienteEventoDTOBase>().ReverseMap();            
            CreateMap<PacienteEvento, PacienteEventoDTOTraslado>().ReverseMap();            
            CreateMap<PacienteEvento, PacienteEventoDTOEgreso>().ReverseMap();            
        }
    }
}