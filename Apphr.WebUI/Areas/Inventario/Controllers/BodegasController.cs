using Apphr.Application.Bodegas.DTOs;
using Apphr.Domain.Entities;
using Apphr.WebUI.Controllers;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using System;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.CustomAttributes;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class BodegasController : DbController
    {
        [AppAuthorization(Permit.View)]
        public ActionResult Index(BodegaDTOIndex dto, int? page) //GET
        {
            IQueryable<Bodega> regs;

            int pageIndex = 1;
            if (dto?.F == null) dto.F = new BodegaDTOIxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //            BodegaDTOIndex dto = new BodegaDTOIndex();


            regs = (from p in db.Bodegas select p);
            if (dto.F != null)
            {
                if (!string.IsNullOrEmpty(dto.F.Buscar))
                    regs = regs.Where(x => x.Nombre.Contains(dto.F.Buscar) || x.Descripcion.Contains(dto.F.Buscar));
            }


                dto.Result = regs.OrderBy(x => x.Nombre).ToPagedList(pageIndex, pageSize);
            return View(dto);
        }



        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega reg = await db.Bodegas.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            BodegaDTODetails dto = mapper.Map<BodegaDTODetails>(reg);

            return View(dto);
        }



        [AppAuthorization(Permit.Edit)]
        public async Task<ActionResult> Edit(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega reg = await db.Bodegas.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            BodegaDTOEdit dto = mapper.Map<BodegaDTOEdit>(reg);
            return View(dto);
        }
        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BodegaId,Nombre,Descripcion")] BodegaDTOEdit dto) // POST
        {
            if (ModelState.IsValid)
            {
                var reg = mapper.Map<Bodega>(dto);
                db.Entry(reg).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = dto.BodegaId, Toast = "success.edit" });
            }
            return View(dto);
        }


        [AppAuthorization(Permit.Edit)]
        public ActionResult Create()  //GET
        {            
            return View();
        }
        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BodegaDTOCreate dto) //POST
        {
            if (ModelState.IsValid)
            {
                Bodega reg = new Bodega();
             
                mapper.Map(dto, reg);

                db.Bodegas.Add(reg);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return View(dto);
        }

        //public async Task<ActionResult> Delete(int? id) //GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Bodega reg = await db.Bodegas.FindAsync(id);
        //    if (reg == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    BodegaDTODelete dto = mapper.Map<BodegaDTODelete>(reg);
        //    return View(dto);
        //}

        //[HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]        
        //public async Task<ActionResult> DeleteConfirmed(int id) // POST
        //{
        //    Bodega reg = await db.Bodegas.FindAsync(id);
        //    db.Bodegas.Remove(reg);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index", new { Toast = "success.delete" });
        //}

        public ActionResult IndexDBF(BodegaDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
        {
            int pageIndex = 1;
            if (dto?.F == null) dto.F = new Application.Common.IxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var regs = this.dbfContext.GetBodegas();

            if (!String.IsNullOrEmpty(dto?.F?.Buscar))
            {
                regs = regs.Where(s =>
                s.CODIGO.Contains(dto?.F?.Buscar) ||
                s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
                ).ToList();
            }

            dto.Result = regs.ToPagedList(pageIndex, pageSize);
            return View(dto);

        }

        public ActionResult DetailsDBF(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dto = this.dbfContext.GetBodega(id).FirstOrDefault();
            if (dto == null)
            {
                return HttpNotFound();
            }
            return View(dto);
        }

        #region Js
        //public JsonResult GetBodega(string id)
        //{
        //    Bodega result = null;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        result = db.Bodegas.Where(x => x.Nombre == id).FirstOrDefault();
        //    }
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult JsNombreExist(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                codigo = Request.Params[0];
            }
            if (string.IsNullOrEmpty(codigo))
            {
                return Json(false);
            }

            return Json(db.Bodegas.Any(u => u.Nombre == codigo));
        }

        public JsonResult JsGetBodegaByNombre(string nombre)
        {
            Bodega res = db.Bodegas.Where(x => x.Nombre == nombre).FirstOrDefault();
            if (res == null)
                return Json(new { success = false, data = res }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);

        }

        public  JsonResult JsGetBodegasByFilter(string val)
        {
            var result = new List<BodegaDTOACItem>();
            if (!string.IsNullOrEmpty(val))
            {
                result = db.Bodegas.Where(x => x.Nombre.Contains(val) || x.Descripcion.Contains(val))
                   .Take(autoCompleteSize)
                   .Select(p => new BodegaDTOACItem { Value = p.Nombre, Text = p.Descripcion })
                   .ToList();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsDelete(int? id)
        {
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var reg = await db.Bodegas.FindAsync(id);
                db.Bodegas.Remove(reg);
                await db.SaveChangesAsync();
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }


        public JsonResult JsSyncBodega(int id)
        {
            try
            {
                var reg = db.Bodegas.Find(id);
                var regDbf = dbfContext.GetBodega(reg.Nombre).FirstOrDefault();
                if (regDbf == null)
                {
                    throw new ArgumentException("No se encontro registro en siahr");
                }
                mapper.Map(regDbf, reg);
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsImportBodega(string CODIGO)
        {
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var BodegaDBF = dbfContext.GetBodega(CODIGO).FirstOrDefault();
                if (!db.Bodegas.Any(x => x.Nombre == BodegaDBF.CODIGO))
                {
                    Bodega reg = mapper.Map<Bodega>(BodegaDBF);
                    db.Bodegas.Add(reg);
                }
                else
                {
                    var reg = db.Bodegas.Where(x => x.Nombre == BodegaDBF.CODIGO).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(BodegaDBF, reg);
                    }
                }
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


    }
}