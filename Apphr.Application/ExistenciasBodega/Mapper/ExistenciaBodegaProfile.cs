using Apphr.Application.ExistenciasBodega.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;

namespace Apphr.Application.ExistenciasBodega.Mapper
{
    public class ExistenciaBodegaProfile : Profile
    {
        public ExistenciaBodegaProfile()
        {
            CreateMap<ExistenciaBodega, ExistenciaBodegaDTOBase>().ReverseMap();
        }
    }
}
