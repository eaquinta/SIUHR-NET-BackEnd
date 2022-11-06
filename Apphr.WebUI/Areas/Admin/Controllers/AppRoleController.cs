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
using Apphr.WebUI.Controllers;
using Apphr.Application.Roles.DTOs;
using Apphr.WebUI.CustomAttributes;
using Apphr.Domain.Enums;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    
    public class AppRoleController : DbController
    {
        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Index() // GET
        {   
            return View(await db.Roles.ToListAsync());
        }

        [AppAuthorization(Permit.View)]
        public ActionResult Details(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.Roles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<AppRoleDTODetails>(appRole);
            return View(vm);
        }

        // GET: Admin/AppRole/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description")] AppRole appRole) // POST
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(appRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { Toast = "success.create" });
            }

            return View(appRole);
        }

        [AppAuthorization(Permit.Edit)]
        public ActionResult Edit(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.Roles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<AppRoleDTOEdit>(appRole);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description")] AppRole appRole) // POST
        {
            if (ModelState.IsValid)
            {
                db.Entry(appRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { Toast = "success.edit" });
            }
            return View(appRole);
        }

        // GET: Admin/AppRole/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.Roles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            return View(appRole);
        }

        // POST: Admin/AppRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppRole appRole = db.Roles.Find(id);
            db.Roles.Remove(appRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
