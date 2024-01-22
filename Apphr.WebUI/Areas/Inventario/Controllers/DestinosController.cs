using Apphr.Application.Common;
using Apphr.Application.Common.DTOs;
using Apphr.Application.Destinos.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
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
        private DestinoRepository DestinoRep;
        public DestinosController()
        {
            DestinoRep = new DestinoRepository(db);
        }

        public ActionResult IndexDBF(DestinoDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
        {
            int pageIndex = 1;
            if (dto?.F == null) dto.F = new IxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var regs = this.dbfContext.GetDestinos();

            if (regs == null)
            {
                return View("ErrorSiahr");
            }

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

              
        [Can("destino.ver")]
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
            string[] permisosRequeridos = { "destino.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
            var result = new List<DTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(val))
            {
                result = db.Destinos.Where(x => x.Codigo.Contains(val) || x.Descripcion.Contains(val))
                   .Take(autoCompleteSize)
                   .Select(p => new DTOAutocompleteItem { id = p.Codigo, text = p.Descripcion })
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
            return Json(new { result = DestinoRep.ImportIfNotExist(CODIGO) }, JsonRequestBehavior.AllowGet);
            //try
            //{
            //    if (string.IsNullOrEmpty(CODIGO))
            //    {
            //        throw new ArgumentException("Parametro CODIGO de contener algun valor");
            //    }
            //    var DestinoDBF = dbfContext.GetDestino(CODIGO).FirstOrDefault();
            //    if (!db.Destinos.Any(x => x.Codigo == DestinoDBF.CODIGO))
            //    {
            //        var reg = mapper.Map<Destino>(DestinoDBF);
            //        db.Destinos.Add(reg);
            //    }
            //    else
            //    {
            //        var reg = db.Destinos.Where(x => x.Codigo == DestinoDBF.CODIGO).FirstOrDefault();
            //        if (reg != null)
            //        {
            //            mapper.Map(DestinoDBF, reg);
            //        }
            //    }
            //    db.SaveChanges();
            //    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{
            //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            //}
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