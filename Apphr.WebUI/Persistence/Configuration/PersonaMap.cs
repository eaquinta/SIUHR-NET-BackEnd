using Apphr.WebUI.Models.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Apphr.Infrastructure.Persistence.Configuration
{
    public class PersonaMap : EntityTypeConfiguration<Persona>
    {
        public PersonaMap()
        {
            //ToTable("Personas");
            //this.HasKey(x => x.Id);
            Property(p => p.FirstName).HasMaxLength(250).IsRequired();
            //HasIndex(i => i.FirstName);
            Property(p => p.MiddleName).HasMaxLength(60);
            //HasIndex(i => i.MiddleName);
            Property(p => p.ThirdName).HasMaxLength(60);
            //HasIndex(i => i.ThirdName);
            Property(p => p.LastName).HasMaxLength(60);
            //HasIndex(i => i.LastName);
            Property(p => p.MatriName).HasMaxLength(60);
            //HasIndex(i => i.MatriName);
            Property(p => p.MarriedName).HasMaxLength(60);
            //HasIndex(i => i.MarriedName);
        }
    }
}
