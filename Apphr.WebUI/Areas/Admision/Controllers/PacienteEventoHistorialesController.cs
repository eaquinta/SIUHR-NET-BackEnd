using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Apphr.Domain.Entities;
using Apphr.WebUI.Models;

namespace Apphr.WebUI.Areas.Admision.Controllers
{
    public class PacienteEventoHistorialesController : Controller
    {
        private readonly ApphrDbContext db = new ApphrDbContext();

        // GET: Admision/PacienteEventoHistoriales
        public async Task<ActionResult> Index()
        {
            var pacienteEventoHistorials = db.PacienteEventoHistoriales; //.Include(p => p.Servicio);
            return View(await pacienteEventoHistorials.ToListAsync());
        }

        // GET: Admision/PacienteEventoHistoriales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEventoHistorial pacienteEventoHistorial = await db.PacienteEventoHistoriales.FindAsync(id);
            if (pacienteEventoHistorial == null)
            {
                return HttpNotFound();
            }
            return View(pacienteEventoHistorial);
        }

        // GET: Admision/PacienteEventoHistoriales/Create
        public ActionResult Create()
        {
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            return View();
        }

        // POST: Admision/PacienteEventoHistoriales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PacienteEventoHistoryId,PacienteEventoId,ServicioId,Fecha,Diagnostico,ProcedimientosRealizados,ProcedimientosPorRealizar,EstadoObservacion")] PacienteEventoHistorial pacienteEventoHistorial)
        {
            if (ModelState.IsValid)
            {
                db.PacienteEventoHistoriales.Add(pacienteEventoHistorial);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", pacienteEventoHistorial.ServicioOrigen);
            return View(pacienteEventoHistorial);
        }

        // GET: Admision/PacienteEventoHistoriales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEventoHistorial pacienteEventoHistorial = await db.PacienteEventoHistoriales.FindAsync(id);
            if (pacienteEventoHistorial == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", pacienteEventoHistorial.ServicioOrigen);
            return View(pacienteEventoHistorial);
        }

        // POST: Admision/PacienteEventoHistoriales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PacienteEventoHistoryId,PacienteEventoId,ServicioId,Fecha,Diagnostico,ProcedimientosRealizados,ProcedimientosPorRealizar,EstadoObservacion")] PacienteEventoHistorial pacienteEventoHistorial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacienteEventoHistorial).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", pacienteEventoHistorial.ServicioOrigen);
            return View(pacienteEventoHistorial);
        }

        // GET: Admision/PacienteEventoHistoriales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEventoHistorial pacienteEventoHistorial = await db.PacienteEventoHistoriales.FindAsync(id);
            if (pacienteEventoHistorial == null)
            {
                return HttpNotFound();
            }
            return View(pacienteEventoHistorial);
        }

        // POST: Admision/PacienteEventoHistoriales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PacienteEventoHistorial pacienteEventoHistorial = await db.PacienteEventoHistoriales.FindAsync(id);
            db.PacienteEventoHistoriales.Remove(pacienteEventoHistorial);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
