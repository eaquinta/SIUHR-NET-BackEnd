using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
