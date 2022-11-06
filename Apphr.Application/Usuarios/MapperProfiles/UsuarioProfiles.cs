using Apphr.Application.Usuarios.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Usuarios.MapperProfiles
{
    class UsuarioProfiles : Profile
    {
        public UsuarioProfiles()
        {
            CreateMap<AppUser, UsuarioDTOEdit>().ReverseMap();
            //CreateMap<AppUser, UsuarioDTOCreate>().ReverseMap();
            CreateMap<AppUser, UsuarioDTODetails>().ReverseMap();
            CreateMap<AppUser, UsuarioDTOBase>().ReverseMap();
        }
    }
}
