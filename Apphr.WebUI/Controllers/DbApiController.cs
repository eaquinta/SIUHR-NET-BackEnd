using Apphr.Infrastructure.Persistence.DBF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Apphr.WebUI.Controllers
{
    public class DbApiController : ApiController
    {
        protected DBFContext dbfContext;
        protected string dbfPath = ConfigurationManager.AppSettings["SiahrPath"].ToString();
        public DbApiController()
        {
            dbfContext = new DBFContext(dbfPath);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                if (this.dbfContext != null)
                {
                    this.dbfContext.Dispose();
                    this.dbfContext = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}