using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Apphr.WebUI.Models;
using Apphr.Domain.Entities;

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
            ApphrDbContext context = new ApphrDbContext();

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleManager = new ApplicationRoleManager(new AppRoleStore(context));
            //var UserManager = new UserManager<AppUser,int>(new UserStore<AppUser>(context));
            var UserManager = new ApplicationUserManager(new AppUserStore(context));
            var PasswordHash = new PasswordHasher();


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("SysAdmin"))
            {

                // first we create Admin rool    
                //var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                var role = new AppRole
                {
                    Name = "SysAdmin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new AppUser()
                {
                    UserName = "eaquinta@yahoo.com",
                    Email = "eaquinta@yahoo.com",
                    PasswordHash = PasswordHash.HashPassword("1234")
                };


                var chkUser = UserManager.Create(user);

                //Add default User to Role Admin    
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
