using Apphr.Application.Common.DTOs;
using Apphr.Application.Ortopedia.Cirujano.DTOs;
using Apphr.WebUI.Models.Entities.Ortopedia;
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

namespace Apphr.WebUI.Areas.Ortopedia.Controllers
{
    [Authorize]
    [LogAction]
    public class CirujanoController : DbController
    {
        [Can("cirujano.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsIndexGrid(string Buscar, int? Page)           // GET 
        {
            IQueryable<ORTCirujano> regs;
            int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

            regs = (from p in db.ORTCirujanos select p);

            if (Buscar != null)
            {
                if (!string.IsNullOrEmpty(Buscar))
                    regs = regs.Where(x => x.Nombre.Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
            }
            var res = await regs
                .OrderBy(x => x.Nombre)
                .Select(s => new CirujanoDTOIxGrid
                {
                    CirujanoId = s.CirujanoId,
                    Nombre = s.Nombre
                })
                .ToListAsync();

            var dto = (PagedList<CirujanoDTOIxGrid>)res.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        public async Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "cirujano.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new CirujanoDTOCreate();
            //ViewBag.ListTipoPrioridad = TipoPrioridad.GetSelectList();
            //ViewBag.ListTipoEvento = TipoEvento.GetSelectList();
            dto.CirujanoId = 0;
            //dto.Anio = DateTime.Now.Year;
            //dto.Fecha = DateTime.Now;

            return PartialView("_CreateMaster", dto);
        }

        public async Task<ActionResult> JsGetCEditForm(int? id)
        {
            string[] permisosRequeridos = { "cirujano.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var reg = db.ORTCirujanos.Where(x => x.CirujanoId == id).FirstOrDefault();
            var dto = new CirujanoDTOCEdit()
            {
                CirujanoId = reg.CirujanoId,
                Nombre = reg.Nombre,
            };
            return PartialView("_CEditMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(CirujanoDTOCreate dto)               // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.CirujanoId == 0)
                ListPermit.Add("cirujano.crear");
            else
                ListPermit.Add("cirujano.editar");

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
                if (dto.CirujanoId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    if (db.ORTCirujanos.Any(x => x.Nombre == dto.Nombre))                    
                        return Json(new { success = false, message = "El Cirujano ya esta registrado." });
                    

                    var reg = new ORTCirujano()
                    {
                        Nombre = dto.Nombre
                    };

                    db.ORTCirujanos.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTCirujanos
                        .Where(x => x.CirujanoId == dto.CirujanoId)
                        .FirstOrDefaultAsync();

                    reg.Nombre = dto.Nombre;

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
            string[] permisosRequeridos = { "cirujano.eliminar" };
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
                        var reg = await db.ORTCirujanos
                            .Where(x => x.CirujanoId == id)
                            .FirstOrDefaultAsync();
                        //var det = await db.ORTMovimientos
                        //    .Where(x => x.SolicitudPedidoId == id)
                        //    .ToListAsync();

                        //foreach (var item in det)
                        //{
                        //    db.ORTMovimientos.Remove(item);
                        //}

                        db.ORTCirujanos.Remove(reg);

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

        public async Task<ActionResult> JsViewMaster(int? id)                           // GET 
        {
            var reg = await db.ORTCirujanos
                .Where(x => x.CirujanoId == id)
                .FirstOrDefaultAsync();

            var dto = new CirujanoDTOView()
            {
                Nombre = reg.Nombre,
            };

            return PartialView("_ViewMaster", dto);
        }

        public async Task<JsonResult> JsGetByFilter(string f, string tipo = "AC")
        {
            Object res = null;
            var result = from p in db.ORTCirujanos select p;
            if (!string.IsNullOrEmpty(f))
            {
                result = result.Where(x => x.Nombre.Contains(f));
            }

            result = result.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = await result
                    .Select(p => new DTOAutocompleteItem { 
                        id = p.CirujanoId.ToString(),
                        text = p.Nombre 
                    })
                    .ToListAsync();
            }
            if (tipo == "S")
            {
                res = await result
                    .Select(s => new DTOSelect2 {
                        id = s.CirujanoId.ToString(),
                        text = s.Nombre,
                        html = "<div style=\"font-weight: bold;\">" + s.Nombre + "</div>"
                    })
                    .ToListAsync();
            }
           
            return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetById(int id)
        {
            var res = await db.ORTCirujanos.Where(x => x.CirujanoId == id).FirstOrDefaultAsync();
            return Json(new { success = (res != null), data = res }, JsonRequestBehavior.AllowGet);
        }

    }
}