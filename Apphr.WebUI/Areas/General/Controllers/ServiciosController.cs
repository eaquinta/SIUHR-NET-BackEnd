using Apphr.Application.Common.Models;
using Apphr.Application.Servicios.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.General.Controllers
{
    [Authorize]
    public class ServiciosController : DbController
    {
        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Index(List<string> Toast) // GET
        {
            C.ToastTemplates = GetToastList(Toast);
            var Result = await db.Servicios.ToListAsync();
            var vm = new ServicioDTOIndex() { Result = Result, C = C };

            return View(vm);
        }
                
        public async Task<ActionResult> Details(int? id, List<string> Toast) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio reg = await db.Servicios.FindAsync(id);            
            if (reg == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<ServicioDTODetails>(reg);
            // Toast
            ViewData["Toast"] = GetToastList(Toast);
            // Tast
            return View(vm);
        }

        
        public ActionResult Create() // GET
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServicioId,Nombre,Area")] Servicio servicio) // POST
        {
            if (ModelState.IsValid)
            {
                db.Servicios.Add(servicio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { Toast = "success.create" });
            }

            return View(servicio);
        }

        public async Task<ActionResult> Edit(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = await db.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServicioId,Nombre,Area")] Servicio servicio) // POST
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = servicio.ServicioId, Toast = "success.edit" });
            }
            return View(servicio);
        }

        // GET: Nutricion/Servicios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = await db.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) // POST
        {
            Servicio servicio = await db.Servicios.FindAsync(id);
            db.Servicios.Remove(servicio);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { Toast = "success.delete"});
        }        
    }
}
