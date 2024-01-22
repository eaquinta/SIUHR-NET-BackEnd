using Apphr.Application.Pacientes.DTOs;
using Apphr.WebUI.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Pacientes.Profiles
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<Paciente, PacienteDTOBase>().ReverseMap();
        }
    }
}
