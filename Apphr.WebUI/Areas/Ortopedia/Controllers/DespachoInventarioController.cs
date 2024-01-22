using Apphr.Application.Ortopedia.DespachosInventario.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Apphr.WebUI.Models.Repository;
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
    public class DespachoInventarioController : DbController
    {
        private OrtopediaRepository OrtopediaRep;
        public DespachoInventarioController()
        {
            OrtopediaRep = new OrtopediaRepository(db);
        }

        [Can("despacho_inventario.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [AppAuthorization(Permit.Edit, Permit.Create)]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")              // GET 
        {
            var reg = await db.ORTDespachosInventario.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }
            var dto = new DespachoInventarioDTOCEdit()
            {
                DespachoInventarioId = reg.DespachoInventarioId,
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
			IQueryable<ORTDespachoInventario> regs;
			int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

			regs = (from p in db.ORTDespachosInventario select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs.Where(x => x.DespachoInventarioId.ToString().Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
			}

			var rows = regs
				.Select(s => new DespachoInventarioDTOIxGrid()
				{
					DespachoInventarioId = s.DespachoInventarioId,
                    Fecha = s.Fecha,
                    Observacion = s.Observacion
				})
				.OrderBy(x => x.DespachoInventarioId);

			var dto = (PagedList<DespachoInventarioDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

        public async Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "despacho_inventario.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new DespachoInventarioDTOCreate();

            dto.DespachoInventarioId = 0;
            dto.Fecha = DateTime.Now;

            return PartialView("_CreateMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(DespachoInventarioDTOCreate dto)     // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.DespachoInventarioId == 0)
                ListPermit.Add("despacho_inventario.crear");
            else
                ListPermit.Add("despacho_inventario.editar");

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
                if (dto.DespachoInventarioId == 0)
                {
                    // INSERT
                    // Validación Adicional if exist
                    if (db.ORTDespachosInventario.Any(x => x.DespachoInventarioId == dto.DespachoInventarioId))
                    {
                        return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    }

                    var reg = new ORTDespachoInventario()
                    {
                        Fecha = dto.Fecha,
                        Observacion = dto.Observacion
                    };

                    db.ORTDespachosInventario.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTDespachosInventario
                        .Where(x => x.DespachoInventarioId == dto.DespachoInventarioId)
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
        public async Task<JsonResult> JsSaveChild(DespachoInventarioDTOCEditChild dto)  // POST
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("despacho_inventario.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("despacho_inventario.editar");

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
                        Tipo = "DES",
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        OrdenCompraId = dto.OrdenCompraId,
                        CirujanoId = dto.CirujanoId,
                        ServicioId = dto.ServicioId,
                        PacienteId = dto.PacienteId,
                        ProveedorId = dto.ProveedorId,
                        BodegaId = dto.BodegaId,
                        MaterialId = dto.MaterialId,
                        Documento = dto.DespachoInventarioId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var DesInv = await db.ORTDespachosInventario
                                .Where(x => x.DespachoInventarioId == dto.DespachoInventarioId)
                                .FirstOrDefaultAsync();
                            DesInv.Lineas += 1;

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
                    using (DbContextTransaction t = db.Database.BeginTransaction())
                    {
                        try
                        {
                            reg = await db.ORTMovimientos
                               .Where(x => x.MovimientoId == dto.MovimientoId)
                               .FirstOrDefaultAsync();

                            reg.PacienteId = dto.PacienteId;
                            reg.BodegaId = dto.BodegaId;
                            reg.MaterialId = dto.MaterialId;
                            reg.ProveedorId = dto.ProveedorId;
                            reg.SolicitudPedidoId = dto.SolicitudPedidoId;
                            reg.OrdenCompraId = dto.OrdenCompraId;
                            reg.CirujanoId = dto.CirujanoId;
                            reg.ServicioId = dto.ServicioId;
                            reg.Cantidad = dto.Cantidad;
                            reg.Precio = dto.Precio;
                            reg.Valor = dto.Valor;

                            await db.SaveChangesAsync();
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.Rollback();
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

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)               // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.Tipo=="DES" && x.Documento == id)
                .Include(i => i.Material)
                .Select(s => new DespachoInventarioDTOCEditGrid()
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

        public async Task<ActionResult> JsGetFormChild(int? id)                         // GET 
        {
            var dto = new DespachoInventarioDTOCEditChild();
            if (id == null)
            {
                dto.SolicitudPedidoId = 0;

                ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.OrdenCompraId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.CirujanoId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ServicioId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.Paciente)
                    .Include(i => i.Material)
                    .Include(i => i.Bodega)
                    .Include(i => i.Proveedor)
                    .Include(i => i.OrdenCompra)
                    .Include(i => i.Cirujano)
                    .Include(i => i.Servicio)
                    .FirstOrDefaultAsync();

                dto.PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString();
                dto.PacienteNombreCompleto = reg.Paciente.Nombre;
                dto.PacienteId = reg.PacienteId;

                dto.BodegaId = reg.BodegaId??0;
                dto.BodegaNombre = reg.Bodega.Nombre;
                dto.BodegaDescripcion = reg.Bodega.Descripcion;

                dto.ProveedorId = reg.Proveedor.ProveedorId;
                dto.ProveedorNit = reg.Proveedor.Nit;
                dto.ProveedorNombre = reg.Proveedor.Descripcion;

                if (reg.OrdenCompraId != null)
                {
                    dto.OrdenCompraId = reg.OrdenCompraId ?? 0;
                    dto.NumeroOC = reg.OrdenCompra.Numero;
                    dto.AnioOC = reg.OrdenCompra.Anio;
                    dto.FechaOC = reg.OrdenCompra.Fecha;
                }
                
                

                dto.CirujanoId = reg.CirujanoId;
                dto.CirujanoNombre = reg.Cirujano.Nombre;

                dto.MovimientoId = reg.MovimientoId;
                dto.SolicitudPedidoId = reg.SolicitudPedidoId;
                dto.MaterialId = reg.MaterialId;
                
                
                dto.Fecha = reg.Fecha;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                dto.MaterialPrecio = reg.Material.Precio;
                dto.UnidadMedida = reg.Material.UnidadMedida;
                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;

                dto.ServicioId = reg.ServicioId??0;
                dto.ServicioNombre = reg.Servicio.Nombre;

                ViewBag.ProveedorId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true,Text = dto.ProveedorNit,Value = dto.ProveedorId.ToString() }
                    }, "Value", "Text");
                ViewBag.OrdenCompraId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.NumeroOC.ToString(), Value = dto.OrdenCompraId.ToString() }
                    }, "Value", "Text"); 
                ViewBag.CirujanoId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.CirujanoNombre, Value = dto.CirujanoId.ToString() }
                    }, "Value", "Text");
                ViewBag.ServicioId = new SelectList(new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.ServicioNombre, Value = dto.ServicioId.ToString() }
                    }, "Value", "Text");

            }            
            return PartialView("_CEditChild", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteChild(Int64 id)                           // POST 
        {
            string[] permisosRequeridos = { "despacho_inventario.eliminar" };
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

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST
        {
            string[] permisosRequeridos = { "despacho_inventario.eliminar" };
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
                        var reg = await db.ORTDespachosInventario
                            .Where(x => x.DespachoInventarioId == id)
                            .FirstOrDefaultAsync();
                        var det = await db.ORTMovimientos
                            .Where(x => x.Documento == id && x.Tipo == "DES")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTDespachosInventario.Remove(reg);

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

        public async Task<ActionResult> JsViewChild(Int64? id)                          // GET 
        {
            //Permit[] permisosRequeridos = { Permit.View };
            //bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            //if (!hasPermit)
            //{
            //    return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            //}
            DespachoInventarioDTOViewChild dto;
            var reg = await db.ORTMovimientos
                .Include(i => i.Paciente)
                .Include(i => i.Material)
                .Include(i => i.Bodega)
                .Include(i => i.Proveedor)
                .Include(i => i.OrdenCompra)
                .Include(i => i.Cirujano)
                .Where(x => x.MovimientoId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                dto = null;
            }
            else 
            {
                dto = new DespachoInventarioDTOViewChild()
                {
                    PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString(),
                    PacienteNombreCompleto = reg.Paciente.Nombre,
                    BodegaNombre = reg.Bodega.Nombre,
                    BodegaDescripcion = reg.Bodega.Descripcion,
                    NumeroOC = reg.OrdenCompra.Numero,
                    AnioOC = reg.OrdenCompra.Anio,
                    FechaOC = reg.OrdenCompra.Fecha,

                    CirujanoId = reg.CirujanoId,
                    CirujanoNombre = reg.Cirujano.Nombre,

                    ProveedorNit = reg.Proveedor.Nit,
                    ProveedorNombre = reg.Proveedor.Descripcion,

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
        public async Task<ActionResult> JsViewMaster(int? id)                           // GET 
        {
            DespachoInventarioDTOViewMaster dto;
            var reg = await db.ORTDespachosInventario
                //.Include(i => i.OrdenCompra)
                //.Include("OrdenCompra.SolicitudPedido")
                .Where(x => x.DespachoInventarioId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                dto = null;
            }
            else 
            {
                dto = new DespachoInventarioDTOViewMaster()
                {
                    Fecha = reg.Fecha,
                    DespachoInventarioId = reg.DespachoInventarioId,
                    Observacion = reg.Observacion,
                };

                //dto.Detalle = new List<MovimientoDTOBase>();

                var regs = await db.ORTMovimientos.Where(x => x.Documento == id && x.Tipo == "DES")
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
            }

            return PartialView("_ViewMaster", dto);
        }

        public JsonResult JsAddFromHojaGasto(Int64 HojaGastoId, int BodegaId, Int64 DespachoInventarioId)
        {
            var res = OrtopediaRep.AddHojaGastoToDespacho(HojaGastoId, BodegaId, DespachoInventarioId); //AddHojaGastoToDevolucion(HojaGastoId, DevolucionId, ProveedorId);
            if (res)
            {
                return Json(new { success = true, message = "Se Agregaron correctamente los materiales." }, JsonRequestBehavior.DenyGet);
            } 
            else
            {
                return Json(new { success = false, message = "No se lograron agregar los materiales." }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}