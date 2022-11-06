using AutoMapper;
using Apphr.Application.Proveedores.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.EntitiesDBF;

namespace Siahr.Application.Proveedores.Profiles
{
    public class ProveedorMapperProfile : Profile
    {
        public ProveedorMapperProfile()
        {
            CreateMap<Proveedor, ProveedorDTOEdit>().ReverseMap();
            CreateMap<Proveedor, ProveedorDTOBase>().ReverseMap();
            //.ForMember(x => x.CreatedDate, y => y.Ignore())
            //.ForMember(x => x.CreatedBy, y => y.Ignore())
            //.ForMember(x => x.LastModifiedDate, y => y.Ignore())
            //.ForMember(x => x.LastModifiedBy, y => y.Ignore());
            CreateMap<Proveedor, ProveedorDTOCreate>().ReverseMap();
            CreateMap<Proveedor, ProveedorDTODetails>().ReverseMap();
            CreateMap<Proveedor, ProveedorDBF>().ReverseMap()
                .ForMember(x => x.Nit, o => o.MapFrom(s => s.NIT))
                .ForMember(x => x.Descripcion, o => o.MapFrom(s => s.DESCRI))
                .ForMember(x => x.Direccion, o => o.MapFrom(s => s.DIRECC))
                .ForMember(x => x.DiasCredito, o => o.MapFrom(s => s.CREDIT))
                .ForMember(x => x.Contacto, o => o.MapFrom(s => s.CONTAC))
                //.ForMember(x => x.Email, o => o.MapFrom(s => s.e))
                .ForMember(x => x.Telefono, o => o.MapFrom(s => s.TELEFO));
        }
    }
}
