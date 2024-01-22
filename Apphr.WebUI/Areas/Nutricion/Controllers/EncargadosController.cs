using Apphr.Application.Encargados.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Controllers;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Nutricion.Controllers
{
    public class EncargadosController : DbController
    {        

        // GET: Nutricion/Encargadoes
        public async Task<ActionResult> Index()
        {
            return View(await db.Encargados.ToListAsync());
        }

        
        public async Task<ActionResult> View(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encargado reg = await db.Encargados.FindAsync(id);
            var vm = mapper.Map<EncargadoDTOView>(reg);
            if (reg == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // GET: Nutricion/Encargadoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nutricion/Encargadoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EncargadoId,Nombre")] Encargado encargado)
        {
            if (ModelState.IsValid)
            {
                db.Encargados.Add(encargado);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(encargado);
        }

        // GET: Nutricion/Encargadoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encargado reg = await db.Encargados.FindAsync(id);
            var vm = mapper.Map<EncargadoDTOEdit>(reg);
            if (reg == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Nutricion/Encargadoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EncargadoId,Nombre")] Encargado encargado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encargado).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(encargado);
        }

        // GET: Nutricion/Encargadoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encargado encargado = await db.Encargados.FindAsync(id);
            if (encargado == null)
            {
                return HttpNotFound();
            }
            return View(encargado);
        }

        // POST: Nutricion/Encargadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Encargado encargado = await db.Encargados.FindAsync(id);
            db.Encargados.Remove(encargado);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }     
    }
}
