using Apphr.Application.Common.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Application.AppPermissions.DTOs;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Admin.Controllers
{
    public class AppPermissionController : DbController
    {       
        [Can("permisos.ver")]
        public ActionResult Index()
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        public async Task<ActionResult> JsViewMaster(int? id)                               // GET 
        {
            var reg = await db.AppPermissions
                .Where(x => x.PermissionId == id)
                .FirstOrDefaultAsync();

            if (reg == null)
                return PartialView("_RegisterNotFound");
            var dto = new AppPermissionDTOView()
            {
                PermissionId = reg.PermissionId,
                Name = reg.Name
            };

            return PartialView("_ViewMaster", dto);
        }


        [HttpPost]
        public JsonResult JsGetDataTable(DTOJqueryDatatableParam param)                     // GET 
        {
            List<AppPermission> Data;

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
                var regs = from p in db.AppPermissions select p;

                if (!(string.IsNullOrEmpty(sortColumnName) && string.IsNullOrEmpty(sortColumnDir)))
                    regs = regs.OrderBy(sortColumnName + " " + sortColumnDir);

                if (!string.IsNullOrEmpty(searchValue))

                    regs = regs.Where(m => m.PermissionId.ToString().Contains(searchValue) || m.Name.Contains(searchValue));

                recordsTotal = regs.Count();

                Data = regs.Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { draw = draw, data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }

        public async Task<ActionResult> JsCEditMaster(int? id)                              // GET 
        {
            string[] permisosRequeridos = { "permisos.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            

            if (id == null)
                return PartialView("_CEditMaster", new AppPermissionDTOCEdit { });


            var reg = await db.AppPermissions.Where(x => x.PermissionId == id).FirstOrDefaultAsync();
            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new AppPermissionDTOCEdit()
            {
                PermissionId = reg.PermissionId,
                Name = reg.Name
            };


            return PartialView("_CEditMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(AppPermissionDTOCEdit dto)               // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.PermissionId == 0)
                ListPermit.Add("permisos.ver");
            else
                ListPermit.Add("permisos.editar");

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


                if (dto.PermissionId == 0)
                { // INSERT

                    // Validación Adicional
                    //if (db.Personas.Any(x => x.I == dto.Nombre))
                    //    return Json(new { success = false, message = "El Cirujano ya esta registrado." });

                    var reg = new AppPermission() {
                        Name = dto.Name,
                    };
                    db.AppPermissions.Add(reg);
                    await db.SaveChangesAsync();

                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                { // UPDATE

                    var reg = db.AppPermissions.Find(dto.PermissionId);
                    reg.Name = dto.Name;
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
        public async Task<JsonResult> JsDeleteMaster(int id)                                // POST 
        {
            string[] permisosRequeridos = { "permisos.eliminar" };
            bool hasPermit = await Utilidades .Can(permisosRequeridos, userId);
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
                        var reg = await db.AppPermissions
                            .Where(x => x.PermissionId == id)
                            .FirstOrDefaultAsync();

                        db.AppPermissions.Remove(reg);

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
    }
}