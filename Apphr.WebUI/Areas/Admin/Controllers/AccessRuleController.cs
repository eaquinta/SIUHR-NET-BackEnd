using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Controllers;
using Apphr.Application.AccessRules.DTOs;
using Apphr.WebUI.CustomAttributes;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [AppAuthorization]
    public class AccessRuleController : DbController
    {        
        
        public async Task<ActionResult> Index() // GET
        {
            return View(await db.AccessRules.ToListAsync());
        }

        
        public async Task<ActionResult> Details(int? id) // GET 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AccessRule reg = await db.AccessRules.Include(x => x.Roles).Include("Roles.Role").Include(x => x.Permits).Where(r => r.AccessRuleId == id).FirstOrDefaultAsync();

            if (reg == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<AccessRuleDTOView>(reg);

            vm.SelectedRoles = vm.Roles.Select(r => r.AppRoleId).ToList();
            vm.SelectedPermits = vm.Permits.Select(r => r.PermitId).ToList();

            return View(vm);
        }

         
        public ActionResult Create() // GET
        {
            ViewData["RoleList"] = db.Roles
               .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
               .ToList();
            return View();
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AccessRuleId, Name, Description, SelectedRoles, SelectedPermits")] AccessRuleDTOCreate vm ) // POST
        {
            if (ModelState.IsValid)
            {
                // validacion de Roles
                if (vm.SelectedRoles != null)
                {
                    var RoleList = await db.Roles.ToListAsync();
                    vm.Roles = RoleList.Where(r => vm.SelectedRoles.Contains(r.Id)).Select(r => new AccessRuleRoleAssignment() { AccessRuleId = vm.AccessRuleId, AppRoleId = r.Id }).ToList();
                }
                // validacion de permisos
                if (vm.SelectedPermits != null)
                    vm.Permits = vm.SelectedPermits.Select((p) => new AccessRulePermitAssignment() { PermitId = p, AccessRuleId = vm.AccessRuleId }).ToList();

                var reg = new AccessRule();
                mapper.Map(vm, reg);

                db.AccessRules.Add(reg);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vm);
        } 

        
        public async Task<ActionResult> Edit(int? id) // GET
        {
            ViewData["RoleList"] = db.Roles
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
                .ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AccessRule reg = await db.AccessRules
                .Include(x => x.Roles)
                .Include("Permits")
                .Where(r => r.AccessRuleId == id)
                .FirstOrDefaultAsync();

            if (reg == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<AccessRuleDTOEdit>(reg);
            vm.SelectedRoles = vm.Roles.Select(r => r.AppRoleId).ToList();
            vm.SelectedPermits = vm.Permits.Select(r => r.PermitId).ToList();

            return View(vm);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AccessRuleId, Name, Description, SelectedRoles, SelectedPermits")] AccessRuleDTOEdit vm) // POST
        {
            if (ModelState.IsValid)
            {
                // validacion de Roles
                if (vm.SelectedRoles != null) { 
                var RoleList = await db.Roles.ToListAsync();
                vm.Roles = RoleList.Where(r => vm.SelectedRoles.Contains(r.Id)).Select(r => new AccessRuleRoleAssignment() { AccessRuleId = vm.AccessRuleId, AppRoleId = r.Id }).ToList();
                }
                // validacion de permisos
                if (vm.SelectedPermits != null)
                vm.Permits = vm.SelectedPermits.Select((p) => new AccessRulePermitAssignment() { PermitId  = p, AccessRuleId = vm.AccessRuleId }).ToList();

                var reg = db.AccessRules
                    .Include(x => x.Roles)
                    .Include("Roles.Role")
                    .Include(x => x.Permits)
                    .Where(r => r.AccessRuleId == vm.AccessRuleId)
                    .FirstOrDefault();                

                mapper.Map(vm, reg);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Admin/AccessRule/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessRule accessRule = await db.AccessRules.FindAsync(id);
            if (accessRule == null)
            {
                return HttpNotFound();
            }
            return View(accessRule);
        }

        // POST: Admin/AccessRule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccessRule accessRule = await db.AccessRules.FindAsync(id);
            db.AccessRules.Remove(accessRule);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }       
    }
}