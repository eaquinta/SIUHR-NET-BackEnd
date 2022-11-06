using Apphr.Application.SolicitudesDespachos.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.SolicitudesDespachos.Mapper
{
    class SolicitudDespachoProfile : Profile
    {
        public SolicitudDespachoProfile()
        {
            CreateMap<SolicitudDespacho, SolicitudDespachoDTOBase>().ReverseMap();
            CreateMap<SolicitudDespacho, SolicitudDespachoDTOCEdit>().ReverseMap();
            CreateMap<SolicitudDespacho, SolicitudDespachoDTOCEditMS>().ReverseMap();
            CreateMap<SolicitudDespacho, SolicitudDespachoDTODetails>().ReverseMap();

            CreateMap<SolicitudDespachoDetalle, SolicitudDespachoDTDTOBase>().ReverseMap();
            CreateMap<SolicitudDespachoDetalle, SolicitudDespachoDetalleDTODetails>().ReverseMap();            
            CreateMap<SolicitudDespachoDetalle, SolicitudDespachoChildDTOCEdit>().ReverseMap();            
        }
    }
}
