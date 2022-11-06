using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Controllers
{
    public class AuditLogController : Controller
    {
        // GET: AuditLog
        public ActionResult Index()
        {
            return View();
        }
    }
}