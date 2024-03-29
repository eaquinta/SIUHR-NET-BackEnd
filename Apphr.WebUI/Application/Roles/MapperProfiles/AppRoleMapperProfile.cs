﻿using Apphr.Application.Roles.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;

namespace Apphr.Application.Roles.MapperProfiles
{
    public class AppRoleMapperProfile  : Profile
    {
        public AppRoleMapperProfile()
        {
            CreateMap<AppRole, AppRoleDTOEdit>().ReverseMap();
            CreateMap<AppRole, AppRoleDTOCreate>().ReverseMap();
            CreateMap<AppRole, AppRoleDTODetails>().ReverseMap();
        }
    }
}
