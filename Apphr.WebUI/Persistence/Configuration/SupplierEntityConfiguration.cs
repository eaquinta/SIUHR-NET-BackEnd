//using Apphr.WebUI.Models.Entities;
//using System.Data.Entity.ModelConfiguration;

//namespace Apphr.Infrastructure.Persistence.Configuration
//{
//    public class SupplierEntityConfiguration : EntityTypeConfiguration<Supplier>
//    {
//        public SupplierEntityConfiguration()
//        {
//			Property(u => u.Nit).HasMaxLength(10).IsRequired();
//			Property(u => u.Descripcion).HasMaxLength(40).IsRequired();
//			Property(u => u.Direccion).HasMaxLength(40).IsRequired();
//			Property(u => u.Telefono).HasMaxLength(19);
//			Property(u => u.Contacto).HasMaxLength(30);
//			Property(u => u.DiasCredito);
//			Property(u => u.Email).HasMaxLength(25);
//			Property(u => u.Banco1).HasMaxLength(20);
//			Property(u => u.Cuenta1).HasMaxLength(20);
//			Property(u => u.Banco2).HasMaxLength(20);
//			Property(u => u.Cuenta2).HasMaxLength(20);
//			Property(u => u.Banco3).HasMaxLength(20);
//			Property(u => u.Cuenta3).HasMaxLength(20);
//		}
//    }
//}
