using AutoMapper;
using System.Reflection;

namespace Apphr.Application
{
    public static class AutoMapperConfig
    {
        public static IMapper _mapper { get; set; }
        public static void Register()
        {
            var mapperConfig = new MapperConfiguration(
                config =>
                {
                    config.AddMaps(Assembly.GetExecutingAssembly());
                }
            );
            _mapper = mapperConfig.CreateMapper();
        }
    }
}