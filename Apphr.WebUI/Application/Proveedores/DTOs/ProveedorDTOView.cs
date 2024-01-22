using System;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOView : ProveedorDTOBase//, AuditableEntityDTOBase
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }

}