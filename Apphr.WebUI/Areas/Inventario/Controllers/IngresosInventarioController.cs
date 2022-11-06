using Apphr.Application.Common;
using Apphr.Application.IngresosInventario.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class IngresosInventarioController : DbController
    {
        [AppAuthorization(Permit.View)]
        public ActionResult Index(IngresoInventarioDTOIndex dto, int? page) //GET
		{
			IQueryable<IngresoInventario> regs;

			int pageIndex = 1;
			if (dto?.F == null) dto.F = new IxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			

			regs = (from p in db.IngresosInventario select p);

			if (dto.F != null)
			{
				if (!string.IsNullOrEmpty(dto.F.Buscar))
					regs = regs.Where(x => x.IngresoInventarioId.Contains(dto.F.Buscar) ||
					(DbFunctions.Right("0" + SqlFunctions.DatePart("d", x.Fecha), 2) + "/" + DbFunctions.Right("0" + SqlFunctions.DatePart("m", x.Fecha), 2) + "/" + SqlFunctions.DatePart("yyyy", x.Fecha)).Contains(dto.F.Buscar) // ||
					//x.Bodega.Descripcion.Contains(dto.F.Buscar)
					);
			}

			regs = regs.OrderBy(x => x.IngresoInventarioId);

			var rows = mapper.Map<List<IngresoInventarioDTOIxRow>>(regs.ToList());
			dto.Result = (PagedList<IngresoInventarioDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;

			return View(dto);
		}

        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var reg = await db.IngresosInventario
				.Where(x => x.IngresoInventarioId == id)				
				.Include(x => x.Detalle)
				.Include("Detalle.Material")
				.Include("Detalle.Bodega")
				.FirstOrDefaultAsync();
			if (reg == null)
			{
				return HttpNotFound();
			}

			var dto = mapper.Map<IngresoInventarioDTODetails>(reg);
			// Mapeo especial
			//dto.BodegaNombre = reg.Bodega.Nombre;
			//dto.BodegaDescripcion = reg.Bodega.Descripcion;

			return View(dto);
		}

        [AppAuthorization(Permit.Edit)]
        public async Task<ActionResult> CEdit(string id)  //GET
		{
			var dto = new IngresoInventarioDTOCEdit();
			if (!string.IsNullOrEmpty(id))
			{
				// Update
				var reg = await db.IngresosInventario
					 //.Include("Bodega")					
					 .Where(x => x.IngresoInventarioId == id)
					 .FirstOrDefaultAsync();

				if (reg == null)
				{
					return HttpNotFound();
				}

				dto = mapper.Map<IngresoInventarioDTOCEdit>(reg);
				// Mapeo especial
				

				dto.Child.IngresoInventarioId = dto.IngresoInventarioId;
				//ViewBag.mode = "UPD";
			}
			else
			{
				// Insert
				dto.FechaDocumentoReferencia = DateTime.Now;
				dto.Fecha = DateTime.Now;
				//dto.Correlativo = Correlativo.Get();
				//
				//dto.year = DateTime.Now.Year.ToString();
				//dto.Director = AppDefaults.GetValue("Director");
				//dto.Gerente = AppDefaults.GetValue("Gerente");
				//ViewBag.mode = "INS";
			}

			return View(dto);
		}


		#region Js
		
		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveMaster(IngresoInventarioDTOCEdit dto)
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
				if (string.IsNullOrEmpty(dto.IngresoInventarioId))
				{
					// INSERT
					dto.IngresoInventarioId = db.GetYearlyAutonumeric("IngresoInventario", DateTime.Now.Year);
					var reg = new IngresoInventario()
					{
						IngresoInventarioId = dto.IngresoInventarioId,
						//BodegaId = dto.BodegaId,
						Fecha = dto.Fecha,
						FechaDocumentoReferencia = dto.FechaDocumentoReferencia,
						DocumentoReferencia = dto.DocumentoReferencia,
						Lineas = 0
					};


					db.IngresosInventario.Add(reg);
					await db.SaveChangesAsync();
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
				}
				else
				{
					// UPDATE
					var reg = await db.IngresosInventario
						.Where(x => x.IngresoInventarioId == dto.IngresoInventarioId)
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
		public async Task<JsonResult> JsSaveChild(IngresoInventarioDetalleDTOCEdit dto)
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
				if (dto.IngresoInventarioDetalleId == 0)
				{
					// INSERT DETALLE
					using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
							// Actualizacion Master
							var masterReg = await db.IngresosInventario.FindAsync(dto.IngresoInventarioId);
							masterReg.Lineas += 1;
							// Crear Movimiento Inventario
							var movInv = new MovimientoInventario()
							{
								Documento = dto.IngresoInventarioId,
								DocumentoReferencia = dto.DocumentoReferencia,								
								MaterialId = dto.MaterialId,
								BodegaId = dto.BodegaId,
								Lote = dto.Lote,
								FechaVencimiento = dto.FechaVencimiento,
								Fecha = dto.Fecha,
								ProveedorId = dto.ProveedorId,
								Observacion = dto.Observacion,
								TipoMovimientoInventarioId = 1,								
								Line = masterReg.Lineas,
								Efecto = 1,
								Cantidad = dto.Cantidad,
								Precio = dto.MaterialPrecio,
								Valor = dto.MaterialPrecio * dto.Cantidad,
							};

							// Crear Child 
							var childReg = new IngresoInventarioDetalle()
							{
								IngresoInventarioId = dto.IngresoInventarioId,
								Linea = masterReg.Lineas,
								BodegaId = dto.BodegaId,
								MaterialId = dto.MaterialId,
								Cantidad = dto.Cantidad,
								Precio = dto.MaterialPrecio,
								Valor = dto.Cantidad * dto.MaterialPrecio,
								ProveedorId = dto.ProveedorId,
								FechaVencimiento = dto.FechaVencimiento,								
								MovimientoInventario = movInv
							};

							db.IngresosInventarioDetalle.Add(childReg);
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
					// UPDATE DETALLE
					var reg = await db.IngresosInventarioDetalle
						.Where(x => x.IngresoInventarioDetalleId == dto.IngresoInventarioDetalleId)
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
						var reg = await db.IngresosInventario.Where(x => x.IngresoInventarioId == id).FirstOrDefaultAsync();
						var det = await db.IngresosInventarioDetalle.Where(x => x.IngresoInventarioId == reg.IngresoInventarioId).ToListAsync();
						foreach (var item in det)
						{
                            var mov = await db.MovimientosInvnentario.FindAsync(item.MovimientoInventarioId);
                            db.MovimientosInvnentario.Remove(mov);
                            db.IngresosInventarioDetalle.Remove(item);
						}
						db.IngresosInventario.Remove(reg);
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
						var reg = await db.IngresosInventarioDetalle.Where(x => x.IngresoInventarioDetalleId == id).FirstOrDefaultAsync();
                        var mov = await db.MovimientosInvnentario.FindAsync(reg.MovimientoInventarioId);
                        db.MovimientosInvnentario.Remove(mov);
                        db.IngresosInventarioDetalle.Remove(reg);
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

		public ActionResult JsGetChild(int? id)
		{
			try
			{
				var reg = db.IngresosInventarioDetalle
					.Include("Material")
					.Include("Proveedor")
					.Where(x => x.IngresoInventarioDetalleId == id)
					.FirstOrDefault();
				var dto = mapper.Map<IngresoInventarioDetalleDTOCEdit>(reg);
				// Special Map
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
			var regs = db.IngresosInventarioDetalle
				.Include("Bodega")
				.Include("Material")
				.Where(x => x.IngresoInventarioId == id)
				.ToList();

			var dto = mapper.Map<IEnumerable<IngresoInventarioDetalleDTOBase>>(regs);
			return PartialView("_CEditGrid", dto);
		}
		#endregion
	}
}