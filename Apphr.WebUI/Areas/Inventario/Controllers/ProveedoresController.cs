using CrystalDecisions.CrystalReports.Engine;
using FluentValidation.Results;
//using PagedList;
using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using Apphr.WebUI.Models;
using Apphr.Application.Proveedores.DTOs;
using Apphr.Application.Proveedores.Commands;
using Apphr.Application.Common;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Controllers;
using System.Threading.Tasks;
using PagedList;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class ProveedoresController : DbController
    {
        [HttpPost]
        public JsonResult AjaxMethod(IxDataTable vm)
        {
            IQueryable<Proveedor> regs;
            List<Proveedor> Data = new List<Proveedor>();
            int pageSize, skip, recordsTotal;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            draw = vm.draw;
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var ixColumn = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + ixColumn + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            try
            {

                regs = (from Proveedores in db.Proveedores select Proveedores);
                if (!(string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDir)))
                {
                    regs = regs.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    regs = regs.Where(m => m.Descripcion.Contains(searchValue) || m.Nit.Contains(searchValue) || m.Contacto.Contains(searchValue));
                }
                recordsTotal = regs.Count();

                Data = regs.Skip(skip).Take(pageSize).ToList();

            }
            catch (Exception)
            {
                //throw;
            }
            return Json(new { data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }

        [AppAuthorization(Permit.View)]
        public ActionResult IndexDBF(ProveedorDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
        {
            int pageIndex = 1;
            if (dto?.F == null) dto.F = new IxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var regs = this.dbfContext.GetProveedores();

            if (!String.IsNullOrEmpty(dto?.F?.Buscar))
            {
                regs = regs.Where(s =>
                s.NIT.Contains(dto?.F?.Buscar) ||
                s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
                ).ToList();
            }

            dto.Result = regs.ToPagedList(pageIndex, pageSize);
            return View(dto);

        }

        [AppAuthorization(Permit.View)]
        public ActionResult DetailsDBF(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dto = this.dbfContext.GetProveedor(id).FirstOrDefault();
            if (dto == null)
            {
                return HttpNotFound();
            }
            return View(dto);
        }

        [AppAuthorization(Permit.View)]
        public ActionResult Index(ProveedorDTOIndex dto, int? page) //GET
        {
            IQueryable<Proveedor> regs;

            int pageIndex = 1;
            if (dto?.F == null) dto.F = new ProveedorDTOIxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //            BodegaDTOIndex dto = new BodegaDTOIndex();


            regs = (from p in db.Proveedores select p);
            if (dto.F != null)
            {
                if (!string.IsNullOrEmpty(dto.F.Buscar))
                    regs = regs.Where(x => x.Nit.Contains(dto.F.Buscar) || x.Descripcion.Contains(dto.F.Buscar) || x.Contacto.Contains(dto.F.Buscar));
            }

            regs = regs.OrderBy(x => x.Nit);

            var rows = mapper.Map<List<ProveedorDTOBase>>(regs.ToList());
            dto.Result = (PagedList<ProveedorDTOBase>)rows.ToPagedList(pageIndex, pageSize);
            return View(dto);
        }
        
        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await db.Proveedores.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            var dto = mapper.Map<ProveedorDTODetails>(reg);

            return View(dto);
        }
        

        [AppAuthorization(Permit.Edit)]
        public ActionResult Create()
        {
            var dto = new ProveedorDTOCreate();
            return View(dto);
        }


        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ProveedorId")] ProveedorDTOCreate dto)
        {
            // Pendiente de Verificación
            //if (ProveedorBLL.NitExist(reg.Nit))            
            //    ModelState.AddModelError("Nit", ProveedorBLL.GetExistMessage("Nit"));          
            if (!ModelState.IsValid)
                return View(dto);

            //var e_reg = Mapper.Map<ProveedorDTOCreateView, Proveedor>(reg);
            var reg = mapper.Map<Proveedor>(dto);
            try
            {
                //using (var db = new ApphrDbContext())
                //{                   
                db.Proveedores.Add(reg);
                db.SaveChanges();
                return RedirectToAction("Index");
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }


        [AppAuthorization(Permit.Edit)]
        public ActionResult Edit(int? id)
        {
            ProveedorDTOEdit dto = null;

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var reg = db.Proveedores.Find(id);

                if (reg == null)
                    return HttpNotFound();

                dto = mapper.Map<ProveedorDTOEdit>(reg);
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProveedorDTOEdit dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            ProveedorValidator validator = new ProveedorValidator();
            ValidationResult result = validator.Validate(dto);
            if (!result.IsValid)
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View(dto);
            }
            try
            {

                var reg = db.Proveedores.Find(dto.ProveedorId);
                mapper.Map(dto, reg);
                db.SaveChanges();

                return RedirectToAction("Index", new { Search_Data = dto.Nit });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }

        }

        [AppAuthorization(Permit.Delete)]
        public async Task<ActionResult> Delete(int? id, string message) //GET
        {
            //if (string.IsNullOrEmpty(message))
            //{
            ViewBag.Message = message;
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await db.Proveedores.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            var dto = mapper.Map<ProveedorDTODetails>(reg);
            return View(dto);
        }

        [AppAuthorization(Permit.Delete)]
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id) // POST
        {
            try
            {
                var reg = await db.Proveedores.FindAsync(id);
                db.Proveedores.Remove(reg);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { Toast = "success.delete" });
            }
            catch (Exception)
            {
                var message = "al eliminar este registro, puede estar referenciado en algún documento.";
                return RedirectToAction("Delete", new { id , message });
            }
            
        }

        public JsonResult GetProveedorNombre(string id)
        {
            Proveedor res;
            using (var db = new ApphrDbContext())
            {
                res = db.Proveedores.Find(id);
                if (res == null)
                    return Json(new { estatus = false, Nombre = "" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { estatus = true, Nombre = res.Descripcion }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReporteProveedores()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/repProveedores.rpt")));
            using (var db = new ApphrDbContext())
            {
                rd.SetDataSource(db.Proveedores.Select(s => new ProveedorDTOReport
                {
                    Nit = s.Nit,
                    Descripcion = s.Descripcion,
                    Direccion = s.Direccion,
                    DiasCredito = s.DiasCredito ?? 0
                }).ToList());
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);

            rd.Dispose();
            rd.Close();
            return new FileStreamResult(str, "application/pdf");
        }

        #region Js


        [HttpPost]
        public JsonResult JsProveedorNitExist(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }
            if (string.IsNullOrEmpty(val))
            {
                return Json(false);
            }

            return Json(db.Proveedores.Any(u => u.Nit == val));
        }

        [HttpPost]
        public JsonResult JsProveedorNitExistOpt(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }
            if (string.IsNullOrEmpty(val))
            {
                return Json(true);
            }
            return Json(db.Proveedores.Any(u => u.Nit == val));
        }


        public JsonResult JsGetProveedorByNit(string val)
        {
            var res = db.Proveedores.Where(x => x.Nit == val).FirstOrDefault();
            //if (res != null)
            //    return Json(new { success = false, data = res }, JsonRequestBehavior.AllowGet);
            //else
            return Json(new { success = (res != null), data = res }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult JsGetProveedoresByFilter(string val)
        {
            var result = new List<ProveedorDTOACItem>();
            if (!string.IsNullOrEmpty(val))
            {
                result = db.Proveedores.Where(x => x.Nit.Contains(val) || x.Descripcion.Contains(val))
                   .Take(autoCompleteSize)
                   .Select(p => new ProveedorDTOACItem { Value = p.Nit, Text = p.Descripcion })
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
                var reg = await db.Proveedores.FindAsync(id);
                db.Proveedores.Remove(reg);
                await db.SaveChangesAsync();
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult JsSyncProveedor(int id)
        {
            try
            {
                var reg = db.Proveedores.Find(id);
                var regDbf = dbfContext.GetProveedor(reg.Nit).FirstOrDefault();
                if (regDbf == null)
                {
                    throw new ArgumentException("No se encontro registro");
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

        public JsonResult JsImportProveedor(string NIT)
        {
            try
            {
                if (string.IsNullOrEmpty(NIT))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var ProveedorDBF = dbfContext.GetProveedor(NIT).FirstOrDefault();
                if (!db.Proveedores.Any(x => x.Nit == ProveedorDBF.NIT))
                {
                    var reg = mapper.Map<Proveedor>(ProveedorDBF);
                    db.Proveedores.Add(reg);
                }
                else
                {
                    var reg = db.Proveedores.Where(x => x.Nit == ProveedorDBF.NIT).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(ProveedorDBF, reg);
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
