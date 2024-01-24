using Apphr.Application.Common.DTOs;
//using Apphr.Application.Roles.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;
using Apphr.WebUI.Application.AppRoles.DTOs;
using Apphr.WebUI.Application.AppPermissions.DTOs;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    
    public class AppRoleController : DbController
    {
        [Can("roles.ver")]
        public ActionResult Index() // GET
        {
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View();
        }

        public async Task<ActionResult> JsViewMaster(int? id)                               // GET 
        {
            var reg = await db.Roles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new AppRoleDTOView()
            {
                Id = reg.Id,
                Name = reg.Name,
                Description = reg.Description
            };

            return PartialView("_ViewMaster", dto);
        }

        [HttpPost]
        public JsonResult JsGetDataTable(DTOJqueryDatatableParam param)                     // GET 
        {
            List<AppRoleDTODataTable> Data;

            var draw = param.draw;
            var start = param.start;
            var length = param.length;
            var sortColumnName = param.columns[param.order[0].column].name;
            var sortColumnDir = param.order[0].dir;
            var searchValue = param.search.value;
            int pageSize = param.length;
            int skip = param.start;
            int recordsTotal = 0;


            try
            {
                var regs = from p in db.Roles select p;

                if (!(string.IsNullOrEmpty(sortColumnName) && string.IsNullOrEmpty(sortColumnDir)))
                    regs = regs.OrderBy(sortColumnName + " " + sortColumnDir);

                if (!string.IsNullOrEmpty(searchValue))

                    regs = regs.Where(m => m.Id.ToString().Contains(searchValue) || m.Name.Contains(searchValue) || m.Description.Contains(searchValue));

                recordsTotal = regs.Count();

                Data = regs.Skip(skip).Take(pageSize).Select(s => new AppRoleDTODataTable { Id = s.Id, Description = s.Description, Name = s.Name }).ToList();                

            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { draw = draw, data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }

        public async Task<ActionResult> JsCEditMaster(int? id)                              // GET 
        {
            string[] permisosRequeridos = { "roles.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }


            if (id == null)
                return PartialView("_CEditMaster", new AppRoleDTOCEdit { });


            var reg = await db.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new AppRoleDTOCEdit()
            {
                Id = reg.Id,
                Name = reg.Name,
                Description = reg.Description
            };


            return PartialView("_CEditMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(AppRoleDTOCEdit dto)               // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.Id == 0)
                ListPermit.Add("roles.ver");
            else
                ListPermit.Add("roless.editar");

            bool hasPermit = await Utilidades.Can(ListPermit.ToArray(), userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
                }


                if (dto.Id == 0)
                { // INSERT

                    // Validación Adicional
                    //if (db.Personas.Any(x => x.I == dto.Nombre))
                    //    return Json(new { success = false, message = "El Cirujano ya esta registrado." });

                    var reg = new AppRole()
                    {
                        Name = dto.Name,
                        Description = dto.Description
                    };
                    db.Roles.Add(reg);
                    await db.SaveChangesAsync();

                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                { // UPDATE

                    var reg = db.Roles.Find(dto.Id);
                    reg.Name = dto.Name;
                    reg.Description = dto.Description;
                    await db.SaveChangesAsync();
                    var data = new {Id = reg.Id, Name = reg.Name };

                    return Json(new { success = true, message = Resources.Msg.success_edit, data = data }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        public async Task<ActionResult> JsAsingPermitsMaster(int? id)
        {
            string[] permisosRequeridos = { "roles.asignar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }

            // Recupera el rol
            var reg = await db.Roles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();    

            if (reg == null)
                return PartialView("_RegisterNotFound");


            var role = new AppRoleDTOBase()
            {
                Id = reg.Id,
                Name = reg.Name,
                Description = reg.Description
            };

            // Recupera todos los permisos disponibles
            List<AppPermissionDTOAssign> allPermits = await db.AppPermissions.OrderBy(x => x.Name).Select(x => new AppPermissionDTOAssign
            {
                PermissionId = x.PermissionId,
                Name = x.Name,
                 Assigned = false
            }).ToListAsync();

            List<AppPermission> permitsAssignedToRole = new List<AppPermission>();

            // Recupera los permisos del Rol
            var queryPermissions = from r in db.Roles
                              join rp in db.AppRolePermissions on r.Id equals rp.AppRoleId 
                              where r.Id == role.Id
                              select rp.AppPermissionId;

            var l = queryPermissions.ToList();

            // _permitRepository.GetAllPermits();
            //List<AppPermission> permitsAssignedToRole = await db.AppRolePermissions.Where(x => x.AppRoleId == reg.Id).Select(s => s.AppPermission).ToListAsync();
            //List<AppPermission> x = from r in db.Roles
            //                        join rp in db.AppRolePermissions on r.Id equals rp.AppRoleId into p
            //                        select 
            //List<AppPermission> permitsAssignedToRole = db.Roles.Where(x => x.Id == reg.Id).FirstOrDefault().Permissions.ToList();

            foreach (var permit in allPermits)
            {
                //permit.Assigned = permitsAssignedToRole.Any(p => p.PermissionId == permit.PermissionId);
                permit.Assigned = l.Any(p => p == permit.PermissionId);

                if (permit.Name.Contains(".ver"))
                {
                    permit.Icon = "<i class=\"fa fa-eye\"></i>";
                }
                if (permit.Name.Contains(".crear"))
                {
                    permit.Icon = "<i class=\"fa fa-plus\"></i>";
                }
                if (permit.Name.Contains(".editar"))
                {
                    permit.Icon = "<i class=\"fa fa-edit\"></i>";
                }
                if (permit.Name.Contains(".eliminar"))
                {
                    permit.Icon = "<i class=\"fa fa-trash-alt\"></i>";
                }
            }

            AppRoleDTOPermit dto = new AppRoleDTOPermit
            {
                Role = role,
                Permisos = allPermits                 
            };

            return PartialView("_AssignPermits", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsAssignMaster(AppRoleDTOPermit dto)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Obtener el rol desde la base de datos
                    var reg = await db.Roles
                        .Where(x => x.Id == dto.Role.Id)
                        .FirstOrDefaultAsync();


                    if (reg != null)
                    {
                        // Obtener los permisos desde la base de datos
                        //var dbAllPermits = await db.AppPermissions.ToListAsync();
                        //var dbRolePermits = await (from r in db.Roles
                        //                           join rp in db.AppRolePermissions on r.Id equals rp.AppRoleId into RolesPermission
                        //                           from x in RolesPermission
                        //                           select x.AppPermission).ToListAsync();
                        // Eliminar los permisos existentes que no están en los permisos seleccionados

                        var rolePermissionsToDelte = db.AppRolePermissions.Where(x => x.AppRoleId == reg.Id);
                        db.AppRolePermissions.RemoveRange(rolePermissionsToDelte);
                        db.SaveChanges();

                        var selectedPermits = dto.Permisos.ToList();
                        //foreach (var dbRolePermit in dbRolePermits)
                        //{
                        //    if (!selectedPermits.Any(p => p.PermissionId == dbRolePermit.PermissionId))
                        //    {
                        //        var toDelete = reg.AppRolePermissions.Where(x => x.AppRoleId == reg.Id && x.AppPermissionId == dbRolePermit.PermissionId).FirstOrDefault();
                        //        db.AppRolePermissions.Remove(toDelete);
                        //    }
                        //    else
                        //    {
                                
                               
                        //    }

                        //}

                        foreach (var permit in selectedPermits)
                        {
                            if (permit.Assigned)
                            {
                                var toAdd = new AppRolePermission()
                                {
                                    AppPermissionId = permit.PermissionId,
                                    AppRoleId = reg.Id
                                };

                                db.AppRolePermissions.Add(toAdd);
                            }
                        }
                        db.SaveChanges();

                    }
                    transaction.Commit();

                    return Json(new { success = true, message = Resources.Msg.success_create/*, data = reg*/ }, JsonRequestBehavior.DenyGet);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
                }
            }
            
        }


        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                                // POST 
        {
            string[] permisosRequeridos = { "permisos.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId); 
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                using (DbContextTransaction t = db.Database.BeginTransaction())
                {
                    try
                    {
                        var reg = await db.Roles
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

                        db.Roles.Remove(reg);

                        await db.SaveChangesAsync();
                        t.Commit();
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                }
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }














        //public async Task<JsonResult> JsSavePermits(AppRoleDTOPermit dto)
        //{
        //    // validacion de Roles
        //    if (dto.Permisos != null)
        //    {
        //        var PermisosList = await db.AppPermissions.ToListAsync();
        //        //var permisos = PermisosList.Where(x => dto.Permisos.Contains(x.PermissionId)).Select(s => new AppPermission()
        //        //{
        //        //    PermissionId = s.PermissionId,
        //        //    Name = s.Name
        //        //}).ToList();
        //        var reg = await db.Roles.Include(i => i.Permissions).Where(x => x.Id == dto.Role.Id).FirstOrDefaultAsync();
        //        //reg.Permissions = permisos;
        //        //reg

        //        ////    //return Json({ });
        //    }
        //}













        //[Can(.".ver")]
        //public ActionResult Details(int? id) // GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AppRole appRole = db.Roles.Find(id);
        //    if (appRole == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var vm = mapper.Map<AppRoleDTODetails>(appRole);
        //    return View(vm);
        //}

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

        //[Can(.".editar")]
        //public ActionResult Edit(int? id) // GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AppRole appRole = db.Roles.Find(id);
        //    if (appRole == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var vm = mapper.Map<AppRoleDTOEdit>(appRole);
        //    return View(vm);
        //}

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
