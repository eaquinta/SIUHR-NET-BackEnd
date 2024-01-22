using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    public class HomeController : Controller
    {
        // GET: Inventario/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Siahad()
        {
            return View();
        }
    }
}