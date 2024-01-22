using Apphr.Application.SolicitudMaterialesSala.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.SolicitudMaterialesSala.Mapper
{
    public class SolicitudMaterialSalaProfile : Profile
    {
        public SolicitudMaterialSalaProfile()
        {
            CreateMap<SolicitudMaterialSala, SolicitudMaterialSalaDTOBase>().ReverseMap();
            CreateMap<SolicitudMaterialSala, SolicitudMaterialSalaDTOCEdit>().ReverseMap();
            CreateMap<SolicitudMaterialSala, SolicitudMaterialSalaDTODetails>().ReverseMap();
            CreateMap<SolicitudMaterialSalaDetalle, SolicitudMaterialSalaDetalleDTOBase>().ReverseMap();
            CreateMap<SolicitudMaterialSalaDetalle, SolicitudMaterialSalaDetalleDTOCEdit>().ReverseMap();
            CreateMap<SolicitudMaterialSalaDetalle, SolicitudMaterialSalaDetalleDTODetails>().ReverseMap();
        }
    }
}
