using Apphr.Application.AccessRules.DTOs;
using Apphr.Domain.Entities;
using AutoMapper;


namespace Apphr.Application.AccessRules.MapperProfiles
{
    public class AccessRuleMapperProfile : Profile
    {
        public AccessRuleMapperProfile()
        {
            CreateMap<AccessRule, AccessRuleDTOEdit>().ReverseMap();
            CreateMap<AccessRule, AccessRuleDTOCreate>().ReverseMap();
            CreateMap<AccessRule, AccessRuleDTOView>().ReverseMap();
        }
    }
}
