using Apphr.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("Proveedores")]
    public class Proveedor : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProveedorId { get; set; }

        [Index("IX_Nit", IsUnique = true)]
        public string Nit { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Contacto { get; set; }

        public int? DiasCredito { get; set; }

        public string Email { get; set; }

        public string Banco1 { get; set; }

        public string Cuenta1 { get; set; }

        public string Banco2 { get; set; }

        public string Cuenta2 { get; set; }

        public string Banco3 { get; set; }

        public string Cuenta3 { get; set; }

        public int? PersonId { get; set; }

        public Persona Contact { get; set; }

        public bool IsActive { get; set; }

    }
}