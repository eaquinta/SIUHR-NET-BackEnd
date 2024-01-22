using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Controllers;
using Apphr.Application.Controladores.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Common;
using System;
using PagedList;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    public class ControladoresController : DbController
    {


        [Can("controlador.ver")]
        public ActionResult Index() // GET
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
            //return View(await db.Controladores.ToListAsync());
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? page)     // GET
        {
            IQueryable<Controlador> regs;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            regs = (from p in db.Controladores.Include(i => i.Roles).Include("Roles.Role") select p);

            if (Buscar != null)
            {
                if (!string.IsNullOrEmpty(Buscar))
                    regs = regs.Where(x => x.Detalle.Contains(Buscar));
            }

            regs = regs.OrderBy(x => x.Detalle);

            var rows = regs.Select(x => new ControladorDTOIxGrid
            {
                AccionId = x.AccionId,
                Detalle = x.Detalle,
                Action = x.Action,
                Area = x.Area,
                Controller = x.Controller,
                ListRoleNames = x.Roles.Select(s => s.Role.Name).ToList(),
                RolesList = ""
            }).ToList();

            rows.ForEach(x => x.RolesList = String.Join(", ", x.ListRoleNames));

            var dto = (PagedList<ControladorDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
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

        public async Task<ActionResult> JsViewMaster(int? id)                                 // GET 
        {
            var reg = await db.Controladores
                .Where(x => x.AccionId == id)
                .FirstOrDefaultAsync();

            var dto = new ControladorDTOView()
            {
                AccionId = reg.AccionId,
                Controller = reg.Controller,
                Area = reg.Area,
                Detalle = reg.Detalle,
            };

            return PartialView("_ViewMaster", dto);
        }

        public async  Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "controlador.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);   
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new ControladorDTOCreate();
            dto.AccionId = 0;

            return PartialView("_CreateMaster", dto);
        }

        public async Task<ActionResult> JsGetCEditForm(int? id)
        {
            string[] permisosRequeridos = { "controlador.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }

            var reg = db.Controladores
                .Include(x => x.Roles)
                .Where(x => x.AccionId == id)
                .FirstOrDefault();
            ViewData["RoleList"] = db.Roles
              .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
              .ToList();

            var dto = new ControladorDTOCEdit()
            {
                AccionId = reg.AccionId,
                Action = reg.Action,
                Area = reg.Area,
                Controller = reg.Controller,
                Detalle = reg.Detalle,
                Roles = reg.Roles,
                SelectedRoles = reg.Roles.Select(r => r.RoleId).ToList()
            };

            return PartialView("_CEditMaster", dto);
        }

        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST 
        {
            string[] permisosRequeridos = { "controlador.eliminar" };
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
                        var reg = await db.Controladores
                            .Where(x => x.AccionId == id)
                            .FirstOrDefaultAsync();


                        db.Controladores.Remove(reg);

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


        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(ControladorDTOCEdit dto)               // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.AccionId == 0)
                ListPermit.Add("controlador.crear");
            else
                ListPermit.Add("controlador.editar");

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
                if (dto.AccionId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    //if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}

                    var reg = new Controlador()
                    {
                        AccionId = dto.AccionId,
                        Action = dto.Action,
                        Controller = dto.Controller,
                        Detalle = dto.Detalle,
                        Roles = dto.Roles,
                        Area = dto.Area
                    };

                    db.Controladores.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.Controladores
                        .Include(x => x.Roles)
                        .Where(x => x.AccionId == dto.AccionId)
                        .FirstOrDefaultAsync();
                    
                    reg.Action = dto.Action;
                    reg.Controller = dto.Controller;
                    reg.Detalle = dto.Detalle;                    
                    reg.Area = dto.Area;
                    // validacion de Roles
                    if (dto.SelectedRoles != null)
                    {
                        var RoleList = await db.Roles.ToListAsync();
                        reg.Roles = RoleList
                            .Where(x => dto.SelectedRoles.Contains(x.Id))
                            .Select(s => new ControladorRolAsignacion() { AccionId = dto.AccionId, RoleId = s.Id })
                            .ToList();
                    }

                    await db.SaveChangesAsync();
                    //var data = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(reg));
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = new { ActionId = reg.AccionId } }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}
