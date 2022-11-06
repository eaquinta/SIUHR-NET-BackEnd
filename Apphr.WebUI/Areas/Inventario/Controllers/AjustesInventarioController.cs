using Apphr.Application.AjustesInventario.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class AjustesInventarioController : DbController
    {
        private AjusteInventarioDetalleRepository AjusteInventarioDetalleRep;
        public AjustesInventarioController()
        {
            AjusteInventarioDetalleRep = new AjusteInventarioDetalleRepository(db);
        }
        [AppAuthorization(Permit.View)]
        public ActionResult Index() // GET
        {
            return View();
        }

        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(string id) // GET
        {

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await db.AjustesInventario
                .Include(x => x.Detalle)
                .Include("Detalle.Material")
                .Include("Detalle.Bodega")
                .Where(x => x.AjusteInventarioId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                return HttpNotFound();
            }

            var dto = mapper.Map<AjusteInventarioDTOBase>(reg);

            return View(dto);
        }

        public async Task<ActionResult> CEdit(string id)  //GET
        {
            var dto = new AjusteInventarioDTOCEdit();
            if (!string.IsNullOrEmpty(id))
            {
                // Update
                var reg = await db.AjustesInventario
                     //.Include("Bodega")
                     //.Include("Seccion")
                     .Where(x => x.AjusteInventarioId == id).FirstOrDefaultAsync();
                if (reg == null)
                {
                    return HttpNotFound();
                }

                dto = mapper.Map<AjusteInventarioDTOCEdit>(reg);
                // Mapeo especial                
                dto.Child.AjusteInventarioId = dto.AjusteInventarioId;
                ViewBag.mode = "UPD";
            }
            else
            {
                // Insert
                dto.FechaDocumentoReferencia = DateTime.Now;
                dto.Fecha = DateTime.Now;
                ViewBag.mode = "INS";
            }
            ViewBag.TipoMovimientoId = new List<SelectListItem>() {
                new SelectListItem { Text = "Ingreso (+)", Value = "3" },
                new SelectListItem { Text = "Egreso (-)", Value = "4" },
                //new SelectListItem { Text = "Urgente", Value = "5" }
            };
            return View(dto);
        }


        #region Js

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? page)
        {
            IQueryable<AjusteInventario> regs;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            regs = (from p in db.AjustesInventario select p);
            if (Buscar != null)
            {
                if (!string.IsNullOrEmpty(Buscar))
                    regs = regs.Where(x => x.AjusteInventarioId.Contains(Buscar) ||
                    (DbFunctions.Right("0" + SqlFunctions.DatePart("d", x.Fecha), 2) + "/" + DbFunctions.Right("0" + SqlFunctions.DatePart("m", x.Fecha), 2) + "/" + SqlFunctions.DatePart("yyyy", x.Fecha)).Contains(Buscar) //||
//                    x.Bodega.Descripcion.Contains(Buscar)
                    );
            }
            regs = regs.OrderBy(x => x.AjusteInventarioId);
            var rows = mapper.Map<List<AjusteInventarioDTOBase>>(regs.ToList());
            var dto = (PagedList<AjusteInventarioDTOBase>)rows.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(AjusteInventarioDTOCEdit dto)
        {
            Permit[] permisosRequeridos = { Permit.Edit };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
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
                if (string.IsNullOrEmpty(dto.AjusteInventarioId))
                {
                    // INSERT
                    dto.AjusteInventarioId = db.GetYearlyAutonumeric("AjusteInventario", DateTime.Now.Year);
                    var reg = new AjusteInventario()
                    {
                        AjusteInventarioId = dto.AjusteInventarioId,
                        //BodegaId = dto.BodegaId,
                        Fecha = dto.Fecha,
                        FechaDocumentoReferencia = dto.FechaDocumentoReferencia,
                        DocumentoReferencia = dto.DocumentoReferencia,
                        Lineas = 0
                    };


                    db.AjustesInventario.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.AjustesInventario
                        .Where(x => x.AjusteInventarioId == dto.AjusteInventarioId)
                        .Include("Detalle")
                        .Include("Detalle.MovimientoInventario")
                        .FirstOrDefaultAsync();
                    //reg.BodegaId = dto.BodegaId;
                    reg.Fecha = dto.Fecha;
                    reg.FechaDocumentoReferencia = dto.FechaDocumentoReferencia;
                    reg.DocumentoReferencia = dto.DocumentoReferencia;
                    // L2
                    foreach (var item in reg.Detalle)
                    {
                        //item.MovimientoInventario.BodegaId = dto.BodegaId;
                        item.MovimientoInventario.Fecha = dto.Fecha;
                    }
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
        public async Task<JsonResult> JsSaveChild(AjusteInventarioDetalleDTOCEdit dto)
        {
            Permit[] permisosRequeridos = { Permit.Edit };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
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
                if (dto.AjusteInventarioDetalleId == 0)
                {
                    // INSERT                    
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Actualizacion Master
                            var Efecto = await db.TiposMovimientoInventario.FindAsync(dto.TipoMovimientoId);
                            var masterReg = await db.AjustesInventario.FindAsync(dto.AjusteInventarioId);
                            masterReg.Lineas += 1;
                            // Crear Movimiento Inventario
                            var movInv = new MovimientoInventario()
                            {
                                Documento = dto.AjusteInventarioId,
                                DocumentoReferencia = dto.DocumentoReferencia,
                                MaterialId = dto.MaterialId,
                                BodegaId = dto.BodegaId,
                                Lote = dto.Lote,
                                FechaVencimiento = dto.FechaVencimiento,
                                Fecha = dto.Fecha,
                                ProveedorId = dto.ProveedorId,
                                Observacion = dto.Observacion,
                                TipoMovimientoInventarioId = dto.TipoMovimientoId,
                                Line = masterReg.Lineas,
                                Efecto = Efecto.Efecto,
                                Cantidad = dto.Cantidad,
                                Precio = dto.MaterialPrecio,
                                Valor = dto.MaterialPrecio * dto.Cantidad,
                            };

                            // Crear Child 
                            var childReg = new AjusteInventarioDetalle()
                            {
                                AjusteInventarioId = dto.AjusteInventarioId,
                                Linea = masterReg.Lineas,
                                BodegaId = dto.BodegaId,
                                MaterialId = dto.MaterialId,
                                Cantidad = dto.Cantidad,
                                Precio = dto.MaterialPrecio,
                                Valor = dto.Cantidad * dto.MaterialPrecio,
                                ProveedorId = dto.ProveedorId,
                                FechaVencimiento = dto.FechaVencimiento,
                                MovimientoInventario = movInv,
                                TipoMovimientoId = dto.TipoMovimientoId
                            };

                            db.AjustesInventarioDetalle.Add(childReg);
                            await db.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    return Json(new { success = true, message = Resources.Msg.success_create });
                }
                else
                {
                    // UPDATE
                    // Ajuste Inventario Detalle
                    var Efecto = await db.TiposMovimientoInventario.FindAsync(dto.TipoMovimientoId);
                    var reg = await db.AjustesInventarioDetalle
                        .Where(x => x.AjusteInventarioDetalleId == dto.AjusteInventarioDetalleId)
                        .Include("MovimientoInventario")
                        .FirstOrDefaultAsync();
                    reg.BodegaId = dto.BodegaId;
                    reg.MaterialId = dto.MaterialId;
                    reg.Cantidad = dto.Cantidad;
                    reg.Precio = dto.MaterialPrecio;
                    reg.Valor = dto.MaterialPrecio * dto.Cantidad;
                    reg.ProveedorId = dto.ProveedorId;
                    reg.FechaVencimiento = dto.FechaVencimiento;
                    reg.Lote = dto.Lote;
                    reg.Observacion = dto.Observacion;
                    reg.TipoMovimientoId = dto.TipoMovimientoId;
                    // Movmiento Inventario
                    reg.MovimientoInventario.MaterialId = dto.MaterialId;
                    reg.MovimientoInventario.BodegaId = dto.BodegaId;
                    reg.MovimientoInventario.FechaVencimiento = dto.FechaVencimiento;
                    reg.MovimientoInventario.Lote = dto.Lote;
                    reg.MovimientoInventario.Fecha = dto.Fecha;
                    reg.MovimientoInventario.ProveedorId = dto.ProveedorId;
                    reg.MovimientoInventario.Observacion = dto.Observacion;
                    reg.MovimientoInventario.Cantidad = dto.Cantidad;
                    reg.MovimientoInventario.Precio = dto.MaterialPrecio;
                    reg.MovimientoInventario.Valor = dto.MaterialPrecio * dto.Cantidad;
                    reg.MovimientoInventario.Efecto = Efecto.Efecto;
                    reg.MovimientoInventario.TipoMovimientoInventarioId = dto.TipoMovimientoId;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException });
            }
        }



        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsDeleteMaster(string id) // POST
        {
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var reg = await db.AjustesInventario.Where(x => x.AjusteInventarioId == id).FirstOrDefaultAsync();
                        var det = await db.AjustesInventarioDetalle.Where(x => x.AjusteInventarioId == reg.AjusteInventarioId).ToListAsync();
                        foreach (var item in det)
                        {
                            var mov = await db.MovimientosInvnentario.FindAsync(item.MovimientoInventarioId);
                            db.MovimientosInvnentario.Remove(mov);
                            db.AjustesInventarioDetalle.Remove(item);
                        }
                        db.AjustesInventario.Remove(reg);
                        await db.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
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
        public async Task<ActionResult> JsDeleteChild(int? id)
        {
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var reg = await db.AjustesInventarioDetalle
                            .Where(x => x.AjusteInventarioDetalleId == id)
                            .FirstOrDefaultAsync();
                        var mov = await db.MovimientosInvnentario.FindAsync(reg.MovimientoInventarioId);
                        db.MovimientosInvnentario.Remove(mov);
                        db.AjustesInventarioDetalle.Remove(reg);
                        await db.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
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


        public async Task<ActionResult> JsGetChild(int? id)
        {
            try
            {
                var reg = await AjusteInventarioDetalleRep.GetChildAsync(id);
                //db.AjustesInventarioDetalle
                //    .Include("Material")
                //    .Include("Proveedor")
                //    .Include("Bodega")
                //    .Where(x => x.AjusteInventarioDetalleId == id)
                //    .FirstOrDefault();
                var dto = mapper.Map<AjusteInventarioDetalleDTOCEdit>(reg);
                // Special Map
                dto.BodegaNombre = reg.Bodega.Nombre;
                dto.BodegaDescripcion = reg.Bodega.Descripcion;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                dto.MaterialPrecio = reg.Material.Precio ?? 0;
                dto.ProveedorNit = reg.Proveedor?.Nit ?? "";
                dto.ProveedorNombre = reg.Proveedor?.Descripcion ?? "";

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Apphr.Resources.Msg.failure, exmessage = ex.Message, exinner = ex.InnerException }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult JsGrid(string id)
        {
            if (string.IsNullOrEmpty(id))
            { return null; }

            var regs = db.AjustesInventarioDetalle
                .Include("Bodega")
                .Include("Material")
                .Where(x => x.AjusteInventarioId == id)
                .ToList();

            var dto = mapper.Map<IEnumerable<AjusteInventarioDetalleDTOBase>>(regs);

            return PartialView("_CEditGrid", dto);
        }
        #endregion


    }
}