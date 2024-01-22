using Apphr.Application.Common;
using Apphr.Application.Common.DTOs;
using Apphr.Application.Proveedores.Commands;
using Apphr.Application.Proveedores.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models;
using Apphr.WebUI.Models.Repository;
using CrystalDecisions.CrystalReports.Engine;
using FluentValidation.Results;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class ProveedoresController : DbController
    {
        private ProveedorRepository ProveedorRep;
        public ProveedoresController()
        {
            ProveedorRep = new ProveedorRepository(db);
        }
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

        [Can("proveedor.ver")]
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
            if (regs == null)
            {
                return View("ErrorSiahr");
            }

            if (!String.IsNullOrEmpty(dto?.F?.Buscar))
            {
                regs = regs.Where(s =>
                s.NIT.Contains(dto?.F?.Buscar) ||
                s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
                ).ToList();
            }

            dto.Result = regs.ToPagedList(pageIndex, pageSize);
            ViewBag.Anio = dbfContext.GetYear();
            return View(dto);

        }

        [Can("proveedor.ver")]
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

        [Can("proveedor.ver")]
        public ActionResult Index() //GET
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? page)     // GET
        {
            IQueryable<Proveedor> regs;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            regs = (from p in db.Proveedores select p);

            if (Buscar != null)
            {
                if (!string.IsNullOrEmpty(Buscar))
                    regs = regs.Where(x => x.Nit.Contains(Buscar) || x.Descripcion.Contains(Buscar) || x.Contacto.Contains(Buscar));
            }

            regs = regs.OrderBy(x => x.Nit);

            var rows = regs.Select(x => new ProveedorDTOIxGrid
            {
                ProveedorId = x.ProveedorId,
                Nit = x.Nit,
                Descripcion = x.Descripcion,
                Direccion = x.Direccion,
                Telefono = x.Telefono,
                Contacto = x.Contacto,
                Email = x.Email,
            }).ToList();

            var dto = (PagedList<ProveedorDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        [Can("proveedor.ver")]
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

            var dto = mapper.Map<ProveedorDTOView>(reg);

            return View(dto);
        }
        

        [Can("proveedor.editar")]
        public ActionResult Create()
        {
            var dto = new ProveedorDTOCreate();
            return View(dto);
        }


        [Can("proveedor.editar")]
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


        [Can("proveedor.editar")]
        public ActionResult Edit(int? id)
        {
            ProveedorDTOCEdit dto = null;

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var reg = db.Proveedores.Find(id);

                if (reg == null)
                    return HttpNotFound();

                dto = mapper.Map<ProveedorDTOCEdit>(reg);
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [Can("proveedor.editar")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProveedorDTOCEdit dto)
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

            var dto = mapper.Map<ProveedorDTOView>(reg);
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
            //using (var db = new ApphrDbContext())
            //{
                res = db.Proveedores.Find(id);
                if (res == null)
                    return Json(new { estatus = false, Nombre = "" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { estatus = true, Nombre = res.Descripcion }, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult ReporteProveedores()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/repProveedores.rpt")));
            //using (var db = new ApphrDbContext())
            //{
                rd.SetDataSource(db.Proveedores.Select(s => new ProveedorDTOReport
                {
                    Nit = s.Nit,
                    Descripcion = s.Descripcion,
                    Direccion = s.Direccion,
                    DiasCredito = s.DiasCredito ?? 0
                }).ToList());
            //}
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

        public async Task<JsonResult> JsGetById(int? id)
        {
            var res = await db.Proveedores.Where(x => x.ProveedorId == id).FirstOrDefaultAsync();
            return Json(new { success = (res != null), data = res }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> JsGetByFilter(string f, string tipo = "AC")
        {
            var result = from r in db.Proveedores select r;
            Object res = null;

            if (!string.IsNullOrEmpty(f))
            {
                result = result.Where(x => x.Nit.Contains(f) || x.Descripcion.Contains(f));
            }

            result = result.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = await result.Select(p => new ProveedorDTOACItem { Value = p.Nit, Text = p.Descripcion })
                    .ToListAsync();
            }
            if (tipo == "S")
            {
                res = await result.Select(s => new DTOSelect2
                {
                    id = s.ProveedorId.ToString(),
                    text = s.Nit,
                    html = "<div style=\"font-weight: bold;\">" + s.Nit + "</div><div style=\"font-size: 0.75em;\">" + s.Descripcion + "</div>"
                })
                .ToListAsync();
            }
            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsDelete(int? id)
        {
            string[] permisosRequeridos = { "proveedor.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
            return Json(new { result = ProveedorRep.ImportIfNotExist(NIT) }, JsonRequestBehavior.AllowGet);            
        }

        public ActionResult ImportProveedores(bool? update)
        {
            if (update == null)
            {
                update = false;
            }
            dbfContext.SetYear(DateTime.Now.Year);
            var dbf = dbfContext.GetProveedores().ToList();
            //db.Database.ExecuteSqlCommand($"DELETE FROM Materiales");
            //db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('[dbo].[Materiales]', RESEED, 0)");
            foreach (var item in dbf)
            {
                try
                {
                    if (!db.Proveedores.Any(x => x.Nit == item.NIT))
                    {
                        var reg = new Proveedor()
                        {
                            Nit = item.NIT,
                            Descripcion = item.DESCRI,
                            Direccion = item.DIRECC,
                            Telefono = item.TELEFO,
                            Contacto = item.CONTAC,
                            DiasCredito = item.CREDIT                            
                        };
                        //reg.SetRenglon();
                        //reg.SetGrupo();
                        db.Proveedores.Add(reg);
                    }
                    else
                    {
                        if (update.Value)
                        {
                            var upd = db.Proveedores.Where(x => x.Nit == item.NIT).FirstOrDefault();
                            upd.Descripcion = item.DESCRI;
                            upd.Direccion = item.DIRECC;
                            upd.Telefono = item.TELEFO;
                            upd.Contacto = item.CONTAC;
                            upd.DiasCredito = item.CREDIT;
                            upd.Descripcion = item.DESCRI;                            
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Console.Write(item.NIT);
                }
            }
            ViewBag.Registros = db.Proveedores.Count();
            return View();
        }
        #endregion



        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(ProveedorDTOCEdit dto)               // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.ProveedorId == 0)
                ListPermit.Add("proveedor.crear");
            else
                ListPermit.Add("proveedor.editar");

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
                if (dto.ProveedorId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    //if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}

                    var reg = new Proveedor()
                    {
                        Nit = dto.Nit,
                        Descripcion = dto.Descripcion,
                    };

                    db.Proveedores.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.Proveedores
                        .Where(x => x.ProveedorId == dto.ProveedorId)
                        .FirstOrDefaultAsync();

                    reg.Nit = dto.Nit;
                    reg.Descripcion = dto.Descripcion;
                    reg.Direccion = dto.Direccion;
                    reg.Contacto = dto.Contacto;
                    reg.Telefono = dto.Telefono;
                    reg.Email = dto.Email;
                    reg.DiasCredito = dto.DiasCredito;
                    reg.Banco1 = dto.Banco1;
                    reg.Cuenta1 = dto.Cuenta1;
                    reg.Banco2 = dto.Banco2;
                    reg.Cuenta2 = dto.Cuenta2;
                    reg.Banco3 = dto.Banco3;
                    reg.Cuenta3 = dto.Cuenta3;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        public async Task<ActionResult> JsViewMaster(int? id)                           // GET 
        {
            var reg = await db.Proveedores
                .Where(x => x.ProveedorId == id)
                .FirstOrDefaultAsync();

            var dto = new ProveedorDTOView()
            {
                Nit = reg.Nit,
                Descripcion = reg.Descripcion,
                Direccion = reg.Direccion,
                Contacto = reg.Contacto,
                Telefono = reg.Telefono,
                Email = reg.Email,
                DiasCredito = reg.DiasCredito,
                Banco1 = reg.Banco1,
                Banco2 = reg.Banco2,
                Banco3 = reg.Banco3,
                Cuenta1 = reg.Cuenta1,
                Cuenta2 = reg.Cuenta2,
                Cuenta3 = reg.Cuenta3,
            };

            return PartialView("_ViewMaster", dto);
        }

        public async Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "proveedor.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new ProveedorDTOCreate();
            dto.ProveedorId = 0;

            return PartialView("_CreateMaster", dto);
        }

        public async Task<ActionResult> JsGetCEditForm(int? id)
        {
            string[] permisosRequeridos = { "proveedor.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var reg = db.Proveedores.Where(x => x.ProveedorId == id).FirstOrDefault();
            var dto = new ProveedorDTOCEdit()
            {
                ProveedorId = reg.ProveedorId,
                Nit = reg.Nit,
                Descripcion = reg.Descripcion,
                Direccion = reg.Direccion,
                Contacto = reg.Contacto,
                Telefono = reg.Telefono,
                Email = reg.Email,
                Banco1 = reg.Banco1,
                Cuenta1 = reg.Cuenta1,
                Banco2 = reg.Banco2,
                Cuenta2 = reg.Cuenta2,
                Banco3 = reg.Banco3,
                Cuenta3 = reg.Cuenta3,
            };
            return PartialView("_CEditMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST 
        {
            string[] permisosRequeridos = { "proveedor.eliminar" };
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
                        var reg = await db.Proveedores
                            .Where(x => x.ProveedorId == id)
                            .FirstOrDefaultAsync();
                        

                        db.Proveedores.Remove(reg);

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
