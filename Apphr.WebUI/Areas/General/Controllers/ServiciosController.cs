using Apphr.Application.Common.DTOs;
using Apphr.Application.Servicios.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.General.Controllers
{
    [Authorize]
    public class ServiciosController : DbController
    {
        [Can("servicio.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? Page)                     // GET 
        {
            IQueryable<Servicio> regs;
            int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

            regs = (from p in db.Servicios select p);

            if (!string.IsNullOrEmpty(Buscar))
                regs = regs.Where(x => x.Nombre.Contains(Buscar));

            regs = regs.OrderBy(x => x.Nombre);

            var rows = regs.Select(x => new ServicioDTOIxGrid
            {
                ServicioId = x.ServicioId,
                Nombre = x.Nombre,
                Area = x.Area,
            }).ToList();

            var dto = (PagedList<ServicioDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        public async Task<ActionResult> JsViewMaster(int? id)                           // GET 
        {
            var reg = await db.Servicios
                .Where(x => x.ServicioId == id)
                .FirstOrDefaultAsync();

            var dto = new ServicioDTOView()
            {
                Nombre = reg.Nombre,
                Area = reg.Area,
            };

            return PartialView("_ViewMaster", dto);
        }

        public async Task<ActionResult> JsGetCEditForm(int? id)                                     // GET 
        {
            string[] permisosRequeridos = { "servicio.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            if (id == null)
            {
                return PartialView("_CEditMaster", new ServicioDTOCEdit { ServicioId = 0 });
            }
            var reg = db.Servicios.Where(x => x.ServicioId == id).FirstOrDefault();

            if (reg == null)
            {
                return PartialView("_RegisterNotFound");
            }

            var dto = new ServicioDTOCEdit()
            {
                ServicioId = reg.ServicioId,
                Nombre = reg.Nombre,
                Area = reg.Area,
            };

            return PartialView("_CEditMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(ServicioDTOCEdit dto)                // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.ServicioId == 0)
                ListPermit.Add("servicio.crear");
            else
                ListPermit.Add("servicio.editar");

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
                if (dto.ServicioId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    if (db.Servicios.Any(x => x.Nombre == dto.Nombre))
                        return Json(new { success = false, message = "Este Servicio ya esta registrada." });

                    var reg = new Servicio()
                    {
                        Nombre = dto.Nombre,
                        Area = dto.Area
                    };

                    db.Servicios.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.Servicios
                        .Where(x => x.ServicioId == dto.ServicioId)
                        .FirstOrDefaultAsync();

                    reg.Nombre = dto.Nombre;
                    reg.Area = dto.Area;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST 
        {
            string[] permisosRequeridos = { "servicio.eliminar" };
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
                        var reg = await db.Servicios
                            .Where(x => x.ServicioId == id)
                            .FirstOrDefaultAsync();


                        db.Servicios.Remove(reg);

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

        public async Task<JsonResult> JsGetByFilter(string f, string tipo = "AC")
        {
            Object res = null;
            var result = from p in db.Servicios select p;
            if (!string.IsNullOrEmpty(f))
            {
                result = result.Where(x => x.Nombre.Contains(f));
            }

            result = result.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = await result
                    .Select(p => new DTOAutocompleteItem
                    {
                        id = p.ServicioId.ToString(),
                        text = p.Nombre
                    })
                    .ToListAsync();
            }
            if (tipo == "S")
            {
                res = await result
                    .Select(s => new DTOSelect2
                    {
                        id = s.ServicioId.ToString(),
                        text = s.Nombre,
                        html = "<div style=\"font-weight: bold;\">" + s.Nombre + "</div>"
                    })
                    .ToListAsync();
            }          

            return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);
        }    

        public async Task<JsonResult> JsGetById(int id)
        {
            var res = await db.Servicios.Where(x => x.ServicioId == id).FirstOrDefaultAsync();
            return Json(new { success = (res != null), data = res }, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> Details(int? id, List<string> Toast) // GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Servicio reg = await db.Servicios.FindAsync(id);            
        //    if (reg == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var vm = mapper.Map<ServicioDTODetails>(reg);
        //    // Toast
        //    ViewData["Toast"] = GetToastList(Toast);
        //    // Tast
        //    return View(vm);
        //}


        //public ActionResult Create() // GET
        //{
        //    return View();
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ServicioId,Nombre,Area")] Servicio servicio) // POST
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Servicios.Add(servicio);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index", new { Toast = "success.create" });
        //    }

        //    return View(servicio);
        //}

        //public async Task<ActionResult> Edit(int? id) // GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Servicio servicio = await db.Servicios.FindAsync(id);
        //    if (servicio == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(servicio);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ServicioId,Nombre,Area")] Servicio servicio) // POST
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(servicio).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Details", new { id = servicio.ServicioId, Toast = "success.edit" });
        //    }
        //    return View(servicio);
        //}

        //// GET: Nutricion/Servicios/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Servicio servicio = await db.Servicios.FindAsync(id);
        //    if (servicio == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(servicio);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id) // POST
        //{
        //    Servicio servicio = await db.Servicios.FindAsync(id);
        //    db.Servicios.Remove(servicio);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index", new { Toast = "success.delete"});
        //}        
    }
}
