using Apphr.Application.AppAreas.DTOs;
using Apphr.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Admision.Controllers
{
    public class HomeController : DbController
    {        
        public ActionResult Index() // GET
        {
            var FecSemIni = DateTime.Now.Date.AddDays(1 - (int)DateTime.Now.DayOfWeek);
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
    }
}
