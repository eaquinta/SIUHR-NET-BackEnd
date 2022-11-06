﻿using Apphr.Application.RegistrosMedicos.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.RegistrosMedicos.MapperProfiles
{
    public class RegistroMedicoProfile : Profile
    {

        public RegistroMedicoProfile()
        {
            CreateMap<RegistroMedico, RegistroMedicoDTOEdit>().ReverseMap();
            CreateMap<RegistroMedico, RegistroMedicoDTOCreate>().ReverseMap();
            CreateMap<RegistroMedico, RegistroMedicoDTOView>().ReverseMap();
        }

    }
}
