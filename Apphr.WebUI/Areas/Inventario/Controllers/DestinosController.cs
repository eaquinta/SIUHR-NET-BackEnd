using Apphr.Application.Destinos.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class DestinosController : DbController
    {
        //[HttpPost]
        //public JsonResult IfDestinoIdExist(int? DestinoId)
        //{
        //    if (DestinoId != null)
        //    {
        //        return Json(DestinoIdExist(DestinoId.Value));
        //    }
        //    return Json(false); 
        //}



        //[HttpPost]
        //public JsonResult IfCodigoDepartamentoExist(string CodigoDepartamento)
        //{
        //    if (!string.IsNullOrEmpty(CodigoDepartamento))
        //    {
        //        return Json(CodigoDestinoExist(CodigoDepartamento));
        //    }
        //    return Json(false);
        //}
        //[HttpPost]
        //public JsonResult IfCodigoSeccionExist(string CodigoSeccion)
        //{
        //    if (!string.IsNullOrEmpty(CodigoSeccion))
        //    {
        //        return Json(CodigoDestinoExist(CodigoSeccion));
        //    }
        //    return Json(false);
        //}



        //[HttpPost]
        //public JsonResult IfDepartamentoIdExist(int? DepartamentoId)
        //{
        //    if (DepartamentoId != null)
        //    {
        //        return Json(DestinoIdExist(DepartamentoId.Value));
        //    }
        //    return Json(false);
        //}
        //[HttpPost]
        //public JsonResult IfDepartamentoSeccionIdExist(int? SeccionId)
        //{
        //    if (SeccionId != null)
        //    {
        //        return Json(DestinoIdExist(SeccionId.Value));
        //    }
        //    return Json(false);
        //}

        //private bool CodigoDestinoExist(string id)
        //{
        //    //bool res = false;
        //    if (!string.IsNullOrEmpty(id))
        //        return db.Destinos.Any(u => u.Codigo == id);
        //    else
        //        return false;
        //}


        //private bool DestinoIdExist(int id)
        //{
        //    bool res = false;
        //    res = db.Destinos.Any(u => u.DestinoId == id);
        //    return res;
        //}
        //public ActionResult BuscarDestino(string Search_Data, string Filter_Value, string llamar)
        //{
        //    if (Search_Data == null)
        //        Search_Data = Filter_Value;
        //    ViewBag.FilterValue = Search_Data;
        //    ViewBag.llamar = llamar;
        //    List<Destino> regs;

        //    regs = db.Destinos.Where(s => s.Codigo.ToUpper().Contains(Search_Data.ToUpper()) || s.Descripcion.ToUpper().Contains(Search_Data.ToUpper())).ToList();
        //    var dto = mapper.Map<IEnumerable<DestinoDTOBase>>(regs);
        //    return PartialView(dto);
        //}
        //public async Task<ActionResult> Delete(int? id) //GET
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Destino reg = await db.Destinos.FindAsync(id);
        //    if (reg == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    DestinoDTODelete dto = mapper.Map<DestinoDTODelete>(reg);
        //    return View(dto);
        //}

        //[HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        //public async Task<ActionResult> DeleteConfirmed(int id) // POST
        //{
        //    Destino reg = await db.Destinos.FindAsync(id);
        //    db.Destinos.Remove(reg);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index", new { Toast = "success.delete" });
        //}

        public ActionResult IndexDBF(DestinoDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
        {
            int pageIndex = 1;
            if (dto?.F == null) dto.F = new Application.Common.IxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var regs = this.dbfContext.GetDestinos();

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
            var dto = this.dbfContext.GetDestino(id).FirstOrDefault();
            if (dto == null)
            {
                return HttpNotFound();
            }
            return View(dto);
        }

              
        [AppAuthorization(Permit.View)]
        public ActionResult Index(DestinoDTOIndex dto, int? page) //GET
        {
            IQueryable<Destino> regs;

            int pageIndex = 1;
            if (dto?.F == null) dto.F = new DestinoDTOIxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;            

            regs = (from p in db.Destinos select p);
            if (dto.F != null)
            {
                if (!string.IsNullOrEmpty(dto.F.Buscar))
                    regs = regs.Where(x => x.Codigo.Contains(dto.F.Buscar) || x.Descripcion.Contains(dto.F.Buscar));
            }

            regs = regs.OrderBy(x => x.Codigo);
                        
            var rows = mapper.Map<List<DestinoDTOIxRow>>(regs.ToList());
            dto.Result = (PagedList<DestinoDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);
            return View(dto);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destino reg = await db.Destinos.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            DestinoDTODetails dto = mapper.Map<DestinoDTODetails>(reg);

            return View(dto);
        }

        public async Task<ActionResult> Edit(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destino reg = await db.Destinos.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            DestinoDTOEdit dto = mapper.Map<DestinoDTOEdit>(reg);
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DestinoId,Codigo,Descripcion,JefeServicio")] DestinoDTOEdit dto) // POST
        {
            if (ModelState.IsValid)
            {
                var reg = mapper.Map<Destino>(dto);
                db.Entry(reg).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = dto.DestinoId, Toast = "success.edit" });
            }
            return View(dto);
        }


        public ActionResult Create()  //GET
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "DestinoId")] DestinoDTOCreate dto) //POST
        {
            if (ModelState.IsValid)
            {
                Destino reg = new Destino();

                mapper.Map(dto, reg);

                db.Destinos.Add(reg);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return View(dto);
        }
        #region Js
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsDelete(int id) // POST
        {
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var reg = await db.Destinos.FindAsync(id);
                db.Destinos.Remove(reg);
                await db.SaveChangesAsync();
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult JsGetDestinoByCodigo(string id)
        {
            Destino res = db.Destinos.Where(x => x.Codigo == id).FirstOrDefault();
            if (res == null)
                return Json(new { success = false, data = res }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult JsGetDestinosByFilter(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }
            var result = new List<MaterialDTOSelectItem>();
            if (!string.IsNullOrEmpty(val))
            {
                result = db.Destinos.Where(x => x.Codigo.Contains(val) || x.Descripcion.Contains(val))
                   .Take(autoCompleteSize)
                   .Select(p => new MaterialDTOSelectItem { id = p.Codigo, text = p.Descripcion })
                   .ToList();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult JsCodigoExist(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                codigo = Request.Params[0];
            }
            if (string.IsNullOrEmpty(codigo))
            {
                return Json(false);
            }

            return Json(db.Destinos.Any(u => u.Codigo == codigo));                
        }

        public JsonResult JsSyncDestino(int id)
        {
            try
            {
                var reg = db.Destinos.Find(id);
                var regDbf = dbfContext.GetDestino(reg.Codigo).FirstOrDefault();
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

        public JsonResult JsImportDestino(string CODIGO)
        {
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var DestinoDBF = dbfContext.GetDestino(CODIGO).FirstOrDefault();
                if (!db.Destinos.Any(x => x.Codigo == DestinoDBF.CODIGO))
                {
                    var reg = mapper.Map<Destino>(DestinoDBF);
                    db.Destinos.Add(reg);
                }
                else
                {
                    var reg = db.Destinos.Where(x => x.Codigo == DestinoDBF.CODIGO).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(DestinoDBF, reg);
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

        //public JsonResult GetDestino(string id)
        //{
        //    Destino result = null;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        result = db.Destinos.Find(id);                
        //    }
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetDestinos(string id)
        //{
        //    List<Destino> result = null;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        result = db.Destinos.Where(x => x.Codigo.Contains(id)).ToList();
        //    }
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}    
        #endregion

    }
}