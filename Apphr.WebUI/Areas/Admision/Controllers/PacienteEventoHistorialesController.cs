using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Controllers;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Admision.Controllers
{
    public class PacienteEventoHistorialesController : DbController
    {
        //private readonly ApphrDbContext db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1);

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
    }
}
