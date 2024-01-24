using Apphr.Application.Bodegas.DTOs;
using Apphr.Application.Common.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class BodegasController : DbController
    {
        private MaterialRepository MaterialRep;
        private BodegaRepository BodegaRep;

        public BodegasController()
        {
            MaterialRep = new MaterialRepository(db);
            BodegaRep = new BodegaRepository(db);
        }

        #region SQL
        [Can("bodega.ver")]
        public ActionResult Index()                                                         //GET
        {
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? page)                         // GET
        {
            IQueryable<Bodega> regs;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            regs = (from p in db.Bodegas select p);

            if (!string.IsNullOrEmpty(Buscar))
                regs = regs.Where(x => x.Nombre.Contains(Buscar) || x.Descripcion.Contains(Buscar));

            regs = regs.OrderBy(x => x.Nombre);

            var rows = regs.Select(x => new BodegaDTOIxGrid
            {
                BodegaId = x.BodegaId,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
            }).ToList();

            //var rows = mapper.Map<List<BodegaDTOIxGrid>>(regs.ToList());
            var dto = (PagedList<BodegaDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        public async Task<ActionResult> JsViewMaster(int? id)                               // GET 
        {
            var reg = await db.Bodegas
                .Where(x => x.BodegaId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                return PartialView("_RegisterNotFound");
            }

            var dto = new BodegaDTOView()
            {
                Descripcion = reg.Descripcion,
                Nombre = reg.Nombre,
                Procedencia = reg.Procedencia,
            };

            return PartialView("_ViewMaster", dto);
        }
        public async Task<ActionResult> JsCEditMaster(int? id)                              // GE 
        {
            string[] permisosRequeridos = { "bodega.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            if (id == null)
            {
                return PartialView("_CEditMaster", new BodegaDTOCEdit { BodegaId = 0 });
            }
            var reg = await db.Bodegas.Where(x => x.BodegaId == id).FirstOrDefaultAsync();
            if (reg == null)
            {
                return PartialView("_RegisterNotFound");
            }
            var dto = new BodegaDTOCEdit()
            {
                BodegaId = reg.BodegaId,
                Nombre = reg.Nombre,
                Descripcion = reg.Descripcion,
            };
            return PartialView("_CEditMaster", dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(BodegaDTOCEdit dto)                      // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.BodegaId == 0)
                ListPermit.Add("bodega.crear");
            else
                ListPermit.Add("bodega.editar");

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
                if (dto.BodegaId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    if (db.Bodegas.Any(x => x.Nombre == dto.Nombre))                    
                        return Json(new { success = false, message = "Esta Bodega ya esta registrada." });
                    

                    var reg = new Bodega()
                    {
                        Nombre = dto.Nombre,
                        Descripcion = dto.Descripcion,
                    };

                    db.Bodegas.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.Bodegas
                        .Where(x => x.BodegaId == dto.BodegaId)
                        .FirstOrDefaultAsync();

                    reg.Nombre = dto.Nombre;
                    reg.Descripcion = dto.Descripcion;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST 
        {
            string[] permisosRequeridos = { "bodega.eliminar" };
            
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
                        var reg = await db.Bodegas
                            .Where(x => x.BodegaId == id)
                            .FirstOrDefaultAsync();


                        db.Bodegas.Remove(reg);

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
        #endregion

        #region DBF
        [Can("bodega.ver")]
        public ActionResult IndexDBF(int? Anio)                                             // GET
        {
            var dto = new BodegaDTOIxFilterDBF { Anio = (Anio == null) ? AnioActual : Anio.Value };
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View("Dbf/Index",dto);
        }
        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndexDBF(string Buscar, int Anio, int? Page)                      // GET 
        {
            //IQueryable<Servicio> regs;
            int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;
            dbfContext.SetYear(Anio);
            var regs = dbfContext.GetBodegas();
            if (regs == null)
                return View("ErrorSiahr");

            if (!string.IsNullOrEmpty(Buscar))
                regs = regs.Where(s => s.CODIGO.ToUpper().Contains(Buscar.ToUpper()) || s.DESCRI.ToUpper().Contains(Buscar.ToUpper()))
                    .ToList();
            else
                regs = regs.ToList();
            var regso = regs.OrderBy(x => x.CODIGO);

            var rows = regso.Select(x => new BodegaDTOIxGridDBF
            {
                CODIGO = x.CODIGO,
                DESCRI = x.DESCRI,
            }).ToList();

            var dto = (PagedList<BodegaDTOIxGridDBF>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            ViewBag.Anio = Anio;
            return PartialView("Dbf/_IndexGrid", dto);
        }
        public ActionResult JsViewMasterDBF(string id, int Anio)                            // GET 
        {
            dbfContext.SetYear(Anio);
            var reg = this.dbfContext.GetBodega(id).FirstOrDefault();
            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new BodegaDTOViewDBF()
            {
                CODIGO = reg.CODIGO,
                DESCRI = reg.DESCRI,
                PROCED = reg.PROCED,
                BODMAT = reg.BODMAT,
            };

            return PartialView("Dbf/_ViewMaster", dto);
        }
        public ActionResult JsCEditMasterDBF(string id, int Anio, string Mode)                          // GET 
        {
            dbfContext.SetYear(Anio);

            if (string.IsNullOrEmpty(id))
            {   // INSERT
                return PartialView("Dbf/_CEditMaster", new BodegaDTOCEditDBF { _Mode = Mode});
            }
            else
            {
                var reg = this.dbfContext.GetBodega(id).FirstOrDefault();
                if (reg == null)
                    return PartialView("_RegisterNotFound");

                var dto = new BodegaDTOCEditDBF
                {
                    _Mode = Mode,
                    BODMAT = reg.BODMAT,
                    CODIGO = reg.CODIGO,
                    DESCRI = reg.DESCRI,
                    PROCED = reg.PROCED,
                };

                return PartialView("Dbf/_CEditMaster", dto);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMasterDBF(string id, int Anio)                                      // POST 
        {
            string[] permisosRequeridos = { "bodega.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                dbfContext.SetYear(Anio);
                dbfContext.DltBodega(new BodegaDBF { CODIGO = id });
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMasterDBF(BodegaDTOCEditDBF dto, int Anio)                            // POST 
        {
            List<string> ListPermit = new List<string>();

            if (string.IsNullOrEmpty(dto.CODIGO))
                ListPermit.Add("bodega.crear");
            else
                ListPermit.Add("bodega.editar");

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
                dbfContext.SetYear(Anio);
                if (dto._Mode == "INS")
                {
                    // INSERT
                    // Validación Adicional
                    //if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}

                    var reg = new BodegaDBF()
                    {
                        CODIGO = dto.CODIGO,
                        DESCRI = dto.DESCRI,
                        PROCED = dto.PROCED,
                        BODMAT = dto.BODMAT
                    };

                    dbfContext.AddBodega(reg);

                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = new BodegaDBF()
                    {
                        CODIGO = dto.CODIGO,
                        DESCRI = dto.DESCRI,
                        PROCED = dto.PROCED,
                        BODMAT = dto.BODMAT
                    };

                    dbfContext.UpdBodega(reg);
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult JsGetBodegasFilterDBF(string f, string tipo, int? anio)
        {
            Object res = null;

            dbfContext.SetYear(anio);
            var regs = dbfContext.GetBodegas();

            if (!string.IsNullOrEmpty(f))
                regs = regs.Where(x => x.CODIGO.ToUpper().Contains(f.ToUpper()));

            regs = regs.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = regs.Select(p => new DTOAutocompleteItem { id = p.CODIGO, text = p.DESCRI })
                    .ToList();
            }
            if (tipo == "S")
            {
                res = regs.Select(s => new DTOSelect2
                {
                    id = s.CODIGO,
                    text = s.DESCRI,
                    html = "<div style=\"font-weight: bold;\">" + s.CODIGO + "</div><div style=\"font-size: 0.75em;\">" + s.DESCRI + "</div>"
                })
                .ToList();
            }

            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Js

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

        public JsonResult JsGetByFilter(string f, string tipo = "AC")
        {
            Object res = null;
            var regs = from r in db.Bodegas select r;

            if (!string.IsNullOrEmpty(f))
                regs = regs.Where(x => x.Nombre.Contains(f) || x.Descripcion.Contains(f));

            regs = regs.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = regs.Select(p => new BodegaDTOACItem { Value = p.Nombre, Text = p.Descripcion })
                   .ToList();
            }
            if ( tipo == "S")
            {
                res = regs.Select(p => new DTOSelect2 {
                    id = p.BodegaId.ToString(),
                    text = p.Nombre,
                    html = "<div style=\"font-weight: bold;\">" + p.Nombre + "</div><div style=\"font-size: 0.75em;\">" + p.Descripcion + "</div>"
                });
            }
                
            //var result = new List<BodegaDTOACItem>();
            //if (!string.IsNullOrEmpty(val))
            //{
            //    result = db.Bodegas.Where(x => x.Nombre.Contains(val) || x.Descripcion.Contains(val))
            //       .Take(autoCompleteSize)
            //       .Select(p => new BodegaDTOACItem { Value = p.Nombre, Text = p.Descripcion })
            //       .ToList();
            //}
            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetById(int id)
        {
            try
            {
                var reg = await db.Bodegas.FindAsync(id);
                return Json(new { success = true, result = true, data = reg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = false }, JsonRequestBehavior.AllowGet);
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


        public ActionResult ImportBodegas(bool? update)
        {
            if (update == null)
            {
                update = false;
            }
            dbfContext.SetYear(DateTime.Now.Year);
            var dbf = dbfContext.GetBodegas().ToList();
            //db.Database.ExecuteSqlCommand($"DELETE FROM Bodegas");
            //db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('[dbo].[Bodegas]', RESEED, 0)");
            foreach (var item in dbf)
            {
                try
                {
                    if (!db.Bodegas.Any(x => x.Nombre == item.CODIGO))
                    {
                        Bodega reg = new Bodega()
                        {
                            Nombre = item.CODIGO,
                            Descripcion = item.DESCRI,
                            Procedencia = item.PROCED
                        };
                        db.Bodegas.Add(reg);
                    }
                    else
                    {
                        if (update.Value)
                        {
                            var upd = db.Bodegas.Where(x => x.Nombre == item.CODIGO).FirstOrDefault();
                            upd.Nombre = item.CODIGO;
                            upd.Descripcion = item.DESCRI;
                            upd.Procedencia = item.PROCED;
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Console.Write(item.CODIGO);
                    //throw;
                }
            }
            ViewBag.Registros = db.Bodegas.Count();
            return View();
        }

        public async Task<JsonResult> JsGetByCodigo(string id, int? BodegaId)
        {
            try
            {
                var reg = await MaterialRep.GetMaterialByCodigoAsync(id);   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }

                var dto = new
                {
                    MaterialId = reg.MaterialId,
                    Codigo = reg.Codigo,
                    Descripcion = reg.Descripcion,
                    UnidadMedida = reg.UnidadMedida,
                    Precio = reg.Precio,
                    Minimo = reg.Minimo ?? 0,
                    Existencia = BodegaRep.GetExistencia(BodegaId ?? 0, reg.MaterialId)
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion













        //[Can(.".ver")]
        //public async Task<ActionResult> Details(int? id)
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

        //    BodegaDTOView dto = mapper.Map<BodegaDTOView>(reg);

        //    return View(dto);
        //}



        //[Can(.".editar")]
        //public async Task<ActionResult> Edit(int? id) // GET
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

        //    BodegaDTOCEdit dto = mapper.Map<BodegaDTOCEdit>(reg);
        //    return View(dto);
        //}
        //[Can(.".editar")]
        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "BodegaId,Nombre,Descripcion")] BodegaDTOCEdit dto) // POST
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var reg = mapper.Map<Bodega>(dto);
        //        db.Entry(reg).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Details", new { id = dto.BodegaId, Toast = "success.edit" });
        //    }
        //    return View(dto);
        //}


        //[Can(.".editar")]
        //public ActionResult Create()  //GET
        //{            
        //    return View();
        //}
        //[Can(.".editar")]
        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(BodegaDTOCreate dto) //POST
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Bodega reg = new Bodega();

        //        mapper.Map(dto, reg);

        //        db.Bodegas.Add(reg);
        //        await db.SaveChangesAsync();

        //        return RedirectToAction("Index");

        //    }
        //    return View(dto);
        //}
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> JsDelete(int? id)
        //{
        //    string[] permisosRequeridos = { .".eliminar" };
        //    bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
        //    if (!hasPermit)
        //    {
        //        return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
        //    }
        //    try
        //    {
        //        var reg = await db.Bodegas.FindAsync(id);
        //        db.Bodegas.Remove(reg);
        //        await db.SaveChangesAsync();
        //        return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
        //    }
        //}



        //public ActionResult DetailsDBF(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var dto = this.dbfContext.GetBodega(id).FirstOrDefault();
        //    if (dto == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dto);
        //}

        //[ValidateAntiForgeryToken]
        //public JsonResult JsSaveDBF(BodegaDTOCEditDBF dto)
        //{
        //    dbfContext.SetYear(dto.Anio);
        //    //var No = dbfContext.ExecNoQuery(dto.CmdUpdate());
        //    return Json(new { No },JsonRequestBehavior.DenyGet);
        //}

    }
}