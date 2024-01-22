using Apphr.Application.Ortopedia.AjustesInventario.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
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
    public class AjusteInventarioController : DbController
    {
        [Can("ajuste_inventario.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [AppAuthorization(Permit.Edit, Permit.Create)]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")              // GET
        {
            var reg = await db.ORTAjustesInventario.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }
            var dto = new AjusteInventarioDTOCEdit()
            {
                AjusteInventarioId = reg.AjusteInventarioId,
                Fecha = reg.Fecha,
                Observacion = reg.Observacion
            };
            ViewBag.Mode = Mode;
            //ViewBag.ListTipo = TipoPrioridad.GetSelectList();
            //ViewBag.ListTipoEvento = TipoEvento.GetSelectList();
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View(dto);
        }

        [ValidateAntiForgeryToken]
		public ActionResult JsIndexGrid(string Buscar, int? Page)                       // GET 
		{
			IQueryable<ORTAjusteInventario> regs;
			int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

			regs = (from p in db.ORTAjustesInventario select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs.Where(x => x.AjusteInventarioId.ToString().Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
			}		

			var rows = regs
				.Select(s => new AjusteInventarioDTOIxGrid()
				{
					AjusteInventarioId = s.AjusteInventarioId,
                    Fecha = s.Fecha,
                    Observacion = s.Observacion
				})
				.OrderBy(x => x.AjusteInventarioId);

			var dto = (PagedList<AjusteInventarioDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

        public async  Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "ajuste_inventario.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new AjusteInventarioDTOCreate();

            dto.AjusteInventarioId = 0;
            dto.Fecha = DateTime.Now;

            return PartialView("_CreateMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(AjusteInventarioDTOCreate dto)       // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.AjusteInventarioId == 0)
                ListPermit.Add("ajuste_inventario.crear");
            else
                ListPermit.Add("ajsute_inventario.editar");

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
                if (dto.AjusteInventarioId == 0)
                {
                    // INSERT
                    // Validación Adicional if exist
                    if (db.ORTAjustesInventario.Any(x => x.AjusteInventarioId == dto.AjusteInventarioId))
                    {
                        return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    }

                    var reg = new ORTAjusteInventario()
                    {
                        Fecha = dto.Fecha,
                        Observacion = dto.Observacion
                    };

                    db.ORTAjustesInventario.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTAjustesInventario
                        .Where(x => x.AjusteInventarioId == dto.AjusteInventarioId)
                        .FirstOrDefaultAsync();

                    reg.Fecha = dto.Fecha;
                    reg.Observacion = dto.Observacion;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST
        {
            string[] permisosRequeridos = { "ajuste_inventario.eliminar" };
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
                        var reg = await db.ORTAjustesInventario
                            .Where(x => x.AjusteInventarioId == id)
                            .FirstOrDefaultAsync();
                        var det = await db.ORTMovimientos
                            .Where(x => x.Documento == id && x.Tipo == "AJU")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTAjustesInventario.Remove(reg);

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
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveChild(AjusteInventarioDTOCEditChild dto)    // POST
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("ajuste_inventario.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("ajuste_inventario.editar");

            bool hasPermit = await Utilidades.Can(Permisos.ToArray(), userId);
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
                if (dto.MovimientoId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    //if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}
                    ORTMovimiento reg = new ORTMovimiento()
                    {
                        Tipo = "AJU",
                        //SolicitudPedidoId = dto.SolicitudPedidoId,
                        OrdenCompraId = dto.OrdenCompraId,
                        ProveedorId = dto.ProveedorId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                        BodegaId = dto.BodegaId,
                        MaterialId = dto.MaterialId,
                        Documento = dto.AjusteInventarioId
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var AjuInv = await db.ORTAjustesInventario
                                .Where(x => x.AjusteInventarioId == dto.AjusteInventarioId)
                                .FirstOrDefaultAsync();
                            AjuInv.Lineas += 1;

                            db.ORTMovimientos.Add(reg);
                            await db.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    ORTMovimiento reg = new ORTMovimiento();
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            reg = await db.ORTMovimientos
                               .Where(x => x.MovimientoId == dto.MovimientoId)
                               .FirstOrDefaultAsync();

                            reg.OrdenCompraId = dto.OrdenCompraId;
                            reg.ProveedorId = dto.ProveedorId;
                            reg.Cantidad = dto.Cantidad;
                            reg.Precio = dto.Precio;
                            reg.Valor = dto.Valor;
                            reg.BodegaId = dto.BodegaId;
                            reg.MaterialId = dto.MaterialId;

                            await db.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public async Task<ActionResult> JsGetFormChild(int? id)                         // GET 
        {
            var dto = new AjusteInventarioDTOCEditChild();
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.OrdenCompraId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.BodegaId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());


            if (id == null)
            {
                dto.SolicitudPedidoId = 0;            
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.Material)
                    .Include(i => i.Bodega)
                    .Include(i => i.Proveedor)
                    .FirstOrDefaultAsync();

                dto.MovimientoId = reg.MovimientoId;
                dto.SolicitudPedidoId = reg.SolicitudPedidoId;
                dto.BodegaNombre = reg.Bodega.Nombre;
                dto.BodegaDescripcion = reg.Bodega.Descripcion;
                dto.BodegaId = reg.BodegaId ?? 0;
                dto.Fecha = reg.Fecha;

                dto.MaterialId = reg.MaterialId;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                dto.MaterialPrecio = reg.Material.Precio;
                dto.UnidadMedida = reg.Material.UnidadMedida;

                dto.ProveedorId = reg.ProveedorId;
                dto.ProveedorNit = reg.Proveedor?.Nit;
                dto.ProveedorNombre = reg.Proveedor?.Descripcion;

                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;
                if (reg.Proveedor != null)
                    ViewBag.ProveedorId = new SelectList(
                       new List<SelectListItem>
                       {
                         new SelectListItem { Selected = true,Text = dto.ProveedorNit,Value = dto.ProveedorId.ToString() }
                       }, "Value", "Text");
                if (reg.OrdenCompra != null)
                    ViewBag.OrdenCompraId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.NumeroOC.ToString(), Value = dto.OrdenCompraId.ToString() }
                    }, "Value", "Text");
                if (reg.BodegaId != null)
                    ViewBag.BodegaId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.BodegaNombre.ToString(), Value = dto.BodegaId.ToString() }
                    }, "Value", "Text");
            }

            return PartialView("_CEditChild", dto);
        }

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)               // GET
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.Tipo == "AJU" && x.Documento == id)
                .Include(i => i.Material)
                .Select(s => new AjusteInventarioDTOCEditGrid()
                {
                    MovimientoId = s.MovimientoId,
                    SolicitudPedidoId = s.SolicitudPedidoId,
                    Material = s.Material,
                    Cantidad = s.Cantidad,
                    Valor = s.Valor,
                    Precio = s.Precio
                })
                .ToListAsync();
            ViewBag.Mode = mode;
            return PartialView("_CEditGrid", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteChild(Int64 id)                           // POST
        {
            string[] permisosRequeridos = { "ajuste_inventario.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
                        var reg = await db.ORTMovimientos
                            .Where(x => x.MovimientoId == id)
                            .FirstOrDefaultAsync();

                        db.ORTMovimientos.Remove(reg);
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
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public async Task<ActionResult> JsViewMaster(int? id)                                 // GET 
        {
            var reg = await db.ORTAjustesInventario
                //.Include(i => i.SolicitudPedido)
                .Where(x => x.AjusteInventarioId == id)
                .FirstOrDefaultAsync();
            var dto = new AjusteInventarioDTOViewMaster()
            {
                //Anio = reg.Anio,
                AjusteInventarioId = reg.AjusteInventarioId,
                Fecha = reg.Fecha,
                //Numero = reg.Numero,
                //NumeroSP = reg.SolicitudPedido?.Numero ?? 0,
                //FechaSP = reg.SolicitudPedido?.Fecha,
                //AnioSP = reg.SolicitudPedido?.Anio ?? 0,
                Observacion = reg.Observacion,
            };

            var regs = await db.ORTMovimientos.Where(x => x.Documento == id && x.Tipo == "AJU")
                .Include(i => i.Material)
                .Select(s => new MovimientoDTOBase()
                {
                    Material = s.Material,
                    Cantidad = s.Cantidad,
                    Precio = s.Precio,
                    Valor = s.Valor
                })
                .ToListAsync();
            dto.Detalle = regs;
            return PartialView("_ViewMaster", dto);
        }

        public async Task<ActionResult> JsViewChild(Int64? id)
        {
            AjusteInventarioDTOViewChild dto;
            var reg = await db.ORTMovimientos
                //.Include(i => i.Paciente)
                .Include(i => i.Material)
                .Include(i => i.Bodega)
                .Include(i => i.Proveedor)
                .Include(i => i.OrdenCompra)
                //.Include(i => i.Cirujano)
                .Where(x => x.MovimientoId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                dto = null;
            }
            else
            {
                dto = new AjusteInventarioDTOViewChild()
                {
                    //PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString(),
                    //PacienteNombreCompleto = reg.Paciente.Nombre,
                    BodegaNombre = reg.Bodega.Nombre,
                    BodegaDescripcion = reg.Bodega.Descripcion,

                    NumeroOC = reg.OrdenCompra?.Numero??0,
                    AnioOC = reg.OrdenCompra?.Anio??0,
                    FechaOC = reg.OrdenCompra?.Fecha??DateTime.MinValue,

                    //CirujanoId = reg.CirujanoId,
                    //CirujanoNombre = reg.Cirujano.Nombre,

                    ProveedorNit = reg.Proveedor?.Nit,
                    ProveedorNombre = reg.Proveedor?.Descripcion,

                    MaterialCodigo = reg.Material.Codigo,
                    MaterialNombre = reg.Material.Descripcion,
                    MaterialPrecio = reg.Material.Precio,
                    UnidadMedida = reg.Material.UnidadMedida,
                    Cantidad = reg.Cantidad,
                    Precio = reg.Precio,
                    Valor = reg.Valor,
                    Fecha = reg.Fecha,
                };
            }


            return PartialView("_ViewChild", dto);
        }
    }
}