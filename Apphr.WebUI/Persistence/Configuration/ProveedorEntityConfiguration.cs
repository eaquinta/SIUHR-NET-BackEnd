using Apphr.WebUI.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Apphr.Infrastructure.Persistence.Configuration
{
    public class ProveedorEntityConfiguration : EntityTypeConfiguration<Proveedor>
    {
        public ProveedorEntityConfiguration()
        {
            //ToTable("Proveedor");
            //HasKey(x => x.ProveedorId);
            //.HasColumnName("NIT")
            Property(u => u.Nit).HasMaxLength(10).IsRequired();
            //.HasColumnName("DESCRI")
            Property(u => u.Descripcion).HasMaxLength(40).IsRequired();
            //.HasColumnName("DIRECC")
            Property(u => u.Direccion).HasMaxLength(40).IsRequired();
            //.HasColumnName("TELEFO")
            Property(u => u.Telefono).HasMaxLength(19);
            //.HasColumnName("CONTAC")
            Property(u => u.Contacto).HasMaxLength(30);
            //.HasColumnName("CREDIT")
            Property(u => u.DiasCredito);
            //.HasColumnName("EMAIL")
            Property(u => u.Email).HasMaxLength(25);
            //.HasColumnName("BANCO1")
            Property(u => u.Banco1).HasMaxLength(20);
            //.HasColumnName("CUENT1")
            Property(u => u.Cuenta1).HasMaxLength(20);
            //.HasColumnName("BANCO2")
            Property(u => u.Banco2).HasMaxLength(20);
            //.HasColumnName("CUENT2")
            Property(u => u.Cuenta2).HasMaxLength(20);
            //.HasColumnName("BANCO3")
            Property(u => u.Banco3).HasMaxLength(20);
            //.HasColumnName("CUENT3")
            Property(u => u.Cuenta3).HasMaxLength(20);
            //Property(u => u.Created).IsOptional();
            //Property(u => u.LastUpdate).IsOptional();
            //Property(u => u.CreatedBy).IsOptional().HasMaxLength(25);

        }
    }
}
