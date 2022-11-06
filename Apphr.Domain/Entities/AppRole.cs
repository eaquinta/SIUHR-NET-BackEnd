using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Apphr.Domain.Entities
{
    //[Table("AppRoles")]
    public class AppRole : IdentityRole<int, AppUserRole>, IRole<int>
    {
        //public AppRole() : base() { }

        //public AppRole(string name, string description): base(name)
        //{
        //    this.Name = name;
        //}
        //
        public AppRole() { }
        public AppRole(string name) : this()
        {
            this.Name = name;
        }
        public AppRole(string name, string description) : this(name)
        {
            this.Description = description;
        }
        public string Description { get; set; }
    }
}
