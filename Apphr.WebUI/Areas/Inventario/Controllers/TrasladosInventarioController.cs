using Apphr.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    public class TrasladosInventarioController : DbController
    {        
        public ActionResult Index() // GET
        {
            return View();
        }
    }
}