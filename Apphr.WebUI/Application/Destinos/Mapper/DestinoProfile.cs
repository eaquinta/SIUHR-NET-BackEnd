using Apphr.Application.Destinos.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.EntitiesDBF;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Destinos.Mapper
{
    class DestinoProfile : Profile
    {
        public DestinoProfile()
        {
            CreateMap<Destino, DestinoDTOBase>().ReverseMap();
            CreateMap<Destino, DestinoDTOCreate>().ReverseMap();
            CreateMap<Destino, DestinoDTOEdit>().ReverseMap();
            CreateMap<Destino, DestinoDTODelete>().ReverseMap();
            CreateMap<Destino, DestinoDTODetails>().ReverseMap();
            CreateMap<Destino, DestinoDTOIxRow>().ReverseMap();

            CreateMap<Destino, DestinoDBF>().ReverseMap()
                .ForMember(x => x.Codigo, o => o.MapFrom(s => s.CODIGO))
                .ForMember(x => x.JefeServicio, o => o.MapFrom(s => s.JEFSER))
                .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRI));
        }
    }
}
