using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Apphr.Domain.Entities;
using Apphr.WebUI.Controllers;
using Apphr.Application.Controladores.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.CustomAttributes;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    public class ControladoresController : DbController
    {


        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Index() // GET
        {
            return View(await db.Controladores.ToListAsync());
        }

        // GET: Admin/Accions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Controlador accion = await db.Controladores.FindAsync(id);
            if (accion == null)
            {
                return HttpNotFound();
            }
            return View(accion);
        }

        // GET: Admin/Accions/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AccionId,Area,Controller,Action")] Controlador accion)
        {
            if (ModelState.IsValid)
            {
                db.Controladores.Add(accion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(accion);
        }

        // GET: Admin/Accions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewData["RoleList"] = db.Roles
               .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
               .ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Controlador accion = await db.Controladores.Include(x => x.Roles).Where(x=> x.AccionId ==id).FirstOrDefaultAsync();
            if (accion == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<ControladorDTOEdit>(accion);
            vm.SelectedRoles = vm.Roles.Select(r => r.RoleId).ToList();
            return View(vm);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AccionId,Area,Controller,Action,Detalle,SelectedRoles")] ControladorDTOEdit vm)
        {
            if (ModelState.IsValid)
            {

                // validacion de Roles
                if (vm.SelectedRoles != null)
                {
                    var RoleList = await db.Roles.ToListAsync();
                    vm.Roles = RoleList.Where(r => vm.SelectedRoles.Contains(r.Id)).Select(r => new ControladorRolAsignacion() { AccionId = vm.AccionId, RoleId = r.Id }).ToList();
                }
                var reg = await db.Controladores.Include(x => x.Roles).Where(x => x.AccionId == vm.AccionId).FirstOrDefaultAsync();
                mapper.Map(vm, reg);
                //db.Entry(accion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Admin/Accions/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Controlador accion = await db.Controladores.FindAsync(id);
            if (accion == null)
            {
                return HttpNotFound();
            }
            return View(accion);
        }

        // POST: Admin/Accions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Controlador accion = await db.Controladores.FindAsync(id);
            db.Controladores.Remove(accion);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }       
    }
}
