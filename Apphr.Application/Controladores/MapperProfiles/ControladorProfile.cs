using Apphr.Application.Controladores.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Controladores.MapperProfiles
{
    public class ControladorProfile : Profile
    {
        public ControladorProfile()
        {
            CreateMap<Controlador, ControladorDTOEdit>().ReverseMap();
            //CreateMap<Controlador, ControladorDTOCreate>().ReverseMap();
            //CreateMap<Controlador, ControladorDTOView>().ReverseMap();
        }
    }
}