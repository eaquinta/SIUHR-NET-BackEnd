using Apphr.Application.Bodegas.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.EntitiesDBF;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Bodegas.Mapper
{
    class BodegaProfiles : Profile
    {
        public BodegaProfiles()
        {
            CreateMap<Bodega, BodegaDTOEdit>().ReverseMap();
            CreateMap<Bodega, BodegaDTOCreate>().ReverseMap();
            CreateMap<Bodega, BodegaDTODelete>().ReverseMap();
            CreateMap<Bodega, BodegaDTODetails>().ReverseMap();
            CreateMap<Bodega, BodegaDTOBase>().ReverseMap();

            CreateMap<Bodega, BodegaDBF>().ReverseMap()
                .ForMember(x => x.Nombre, o => o.MapFrom(s => s.CODIGO))
                .ForMember(x => x.Procedencia, o => o.MapFrom(s => s.PROCED))
                .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRI));
        }
    }
}
