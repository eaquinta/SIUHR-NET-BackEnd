using Apphr.Application.AppAreas.DTOs;
using Apphr.WebUI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Controllers
{
    [Authorize]
    public class HomeController : DbController
    {        
        public ActionResult Index()
        {
            var FecSemIni = DateTime.Now.Date.AddDays(1-(int)DateTime.Now.DayOfWeek);
            var FecSemFin = FecSemIni.AddDays(7).AddSeconds(-1);

            var vm = new RootDTODashboard()
            {
                FechaInicial = FecSemIni,
                FechaFinal = FecSemFin,
                Activos = db.PacienteEventos.Where(x => x.FechaEgreso == null).Count(),
                Ingresos = db.PacienteEventos.Where(x => x.FechaIngreso >= FecSemIni && x.FechaIngreso <= FecSemFin).Count(),
                Egresos = db.PacienteEventos.Where(x => x.FechaEgreso >= FecSemIni && x.FechaEgreso <= FecSemFin).Count(),
                Total = db.PacienteEventos.Count()
            };
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NoAutorizado()
        {
            return View();
        }
    }
}