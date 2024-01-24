namespace Apphr.WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApphrDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Models.ApphrDbContext context)
        {
            //context.AppPermissions.AddOrUpdate(
            //    p => p.Name,
            //    new Models.Entities.AppPermission { Name = "roles.ver" },
            //    new Models.Entities.AppPermission { Name = "roles.crear" },
            //    new Models.Entities.AppPermission { Name = "roles.editar" },
            //    new Models.Entities.AppPermission { Name = "roles.eliminar" },
            //    new Models.Entities.AppPermission { Name = "roles.asignar" }
            //   );
            //context.PacienteEventoTraslados.AddOrUpdate(new Domain.Entities.PacienteEventoTraslado { Fecha = DateTime.Now });
        //    //  This method will be called after migrating to the latest version.

        //    //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //    //  to avoid creating duplicate seed data. E.g.
        //    //
        //    //    context.People.AddOrUpdate(
        //    //      p => p.FullName,
        //    //      new Person { FullName = "Andrew Peters" },
        //    //      new Person { FullName = "Brice Lambson" },
        //    //      new Person { FullName = "Rowan Miller" }
        //    //    );
        //    //
        }
    }
}
