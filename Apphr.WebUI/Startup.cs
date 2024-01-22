using Apphr.WebUI.Models;
using Apphr.WebUI.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System.Web;

[assembly: OwinStartup(typeof(Apphr.WebUI.Startup))]
namespace Apphr.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }


        private void CreateUserAndRoles()
        {
            ApphrDbContext context = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1);

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleManager = new ApplicationRoleManager(new AppRoleStore(context));
            //var UserManager = new UserManager<AppUser,int>(new UserStore<AppUser>(context));
            var UserManager = new ApplicationUserManager(new AppUserStore(context));
            var PasswordHash = new PasswordHasher();


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("SysAdmin"))
            {
                // Add Persona
                var Persona = new Persona()
                {
                    FirstName = "Estuardo",
                    LastName = "Quintanilla",
                };
                context.Personas.Add(Persona);

                // Roles
                var role = new AppRole
                {
                    Name = "SysAdmin"
                };
                roleManager.Create(role);

                // Usuario
                var user = new AppUser()
                {
                    UserName = "eaquinta@yahoo.com",
                    Email = "eaquinta@yahoo.com",
                    PasswordHash = PasswordHash.HashPassword("1234"),
                    PersonaId = Persona.PersonId,
                };
                var chkUser = UserManager.Create(user);

                // Agregar Rol al usuario
                if (chkUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "SysAdmin");
                }               
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                //var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                var role = new AppRole
                {
                    Name = "Manager"
                };
                roleManager.Create(role);

            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Employee"))
            {
                //var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                var role = new AppRole
                {
                    Name = "Employee"
                };
                roleManager.Create(role);

            }

        }

    }
}
