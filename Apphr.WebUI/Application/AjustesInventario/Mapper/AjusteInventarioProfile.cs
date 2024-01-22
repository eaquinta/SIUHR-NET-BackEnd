using Apphr.Application.AjustesInventario.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.AjustesInventario.Mapper
{
    public class AjusteInventarioProfile : Profile
    {
        public AjusteInventarioProfile()
        {
            CreateMap<AjusteInventario, AjusteInventarioDTOBase>().ReverseMap();
            CreateMap<AjusteInventario, AjusteInventarioDTOCEdit>().ReverseMap();

            CreateMap<AjusteInventarioDetalle, AjusteInventarioDetalleDTOBase>().ReverseMap();
            CreateMap<AjusteInventarioDetalle, AjusteInventarioDetalleDTOCEdit>().ReverseMap();
        }
    }
}
