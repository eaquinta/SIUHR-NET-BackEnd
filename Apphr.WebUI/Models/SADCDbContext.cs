using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class SADCDbContext : DbContext
    {
        public SADCDbContext() : base("SADCConn")
        {
        }
    }
}