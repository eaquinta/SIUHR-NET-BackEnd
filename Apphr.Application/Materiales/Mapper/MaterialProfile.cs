using Apphr.Application.Materiales.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.EntitiesDBF;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Materiales.Mapper
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDTOEdit>().ReverseMap();
            CreateMap<Material, MaterialDTODelete>().ReverseMap();
            CreateMap<Material, MaterialDTOCreate>().ReverseMap();
            CreateMap<Material, MaterialDTODetails>().ReverseMap();
            CreateMap<Material, MaterialDTOBase>().ReverseMap();
            CreateMap<Material, MaterialDTOBaseExt>().ReverseMap();
            CreateMap<Material, MaterialDTOIxRow>().ReverseMap();
            CreateMap<Material, MaterialDBF>().ReverseMap()
                .ForMember(x => x.Codigo, o => o.MapFrom(s => s.CODIGO))
                .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRI))
                .ForMember(x => x.UnidadMedida, o => o.MapFrom(s => s.UNIMED))
                .ForMember(x => x.Precio, o => o.MapFrom(s => s.PRECIO));
        }
    }
}
