using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Nutricion.Controllers
{
    public class HomeController : Controller
    {
        // GET: Nutricion/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}