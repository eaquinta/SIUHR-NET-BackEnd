using Apphr.Application.Ortopedia.IngresosInventario.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.WebUI.Models.Entities.Ortopedia;
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
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Ortopedia.Controllers
{
    [Authorize]
    [LogAction]
    public class IngresoInventarioController : DbController
    {
        private OrtopediaRepository OrtopediaRep;
        public IngresoInventarioController()
        {
            OrtopediaRep = new OrtopediaRepository(db);
        }

        [Can("ingreso_inventario.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [AppAuthorization(Permit.Edit, Permit.Create)]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")              // GET 
        {
            var reg = await db.ORTIngresosInventario
                .Where(x => x.IngresoInventarioId == id)
                .Include(i => i.OrdenCompra)
                .Include("OrdenCompra.SolicitudPedido")
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                return HttpNotFound();
            }
            var dto = new IngresoInventarioDTOCEdit()
            {
                IngresoInventarioId = reg.IngresoInventarioId,                
                Fecha = reg.Fecha,
                OrdenCompraId = reg.OrdenCompraId,
                NumeroOC = reg.OrdenCompra.Numero,
                FechaOC = reg.OrdenCompra.Fecha,
                AnioOC = reg.OrdenCompra.Anio,
                SolicitudPedidoId = reg.OrdenCompra.SolicitudPedidoId ?? 0,
                NumeroSP = reg.OrdenCompra.SolicitudPedido.Numero,
                FechaSP = reg.OrdenCompra.SolicitudPedido.Fecha,
                AnioSP = reg.OrdenCompra.SolicitudPedido.Anio,
                Observacion = reg.Observacion,
                ProveedorId = reg.OrdenCompra.ProveedorId
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
			IQueryable<ORTIngresoInventario> regs;
			int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

			regs = (from p in db.ORTIngresosInventario.Include(i => i.OrdenCompra) select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs
                        .Where(x => x.IngresoInventarioId.ToString()
                        .Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
			}

			var rows = regs		
				.OrderBy(x => x.IngresoInventarioId);

            List<IngresoInventarioDTOIxGrid> res = new List<IngresoInventarioDTOIxGrid>();
            foreach (var s in rows)
            {
                res.Add(new IngresoInventarioDTOIxGrid()
                {
                    IngresoInventarioId = s.IngresoInventarioId,
                    Fecha = s.Fecha,
                    OC = s.OrdenCompra?.Numero.ToString(),
                    FechaOC = s.OrdenCompra?.Fecha,
                    Observacion = s.Observacion
                });
            }
           
			var dto = (PagedList<IngresoInventarioDTOIxGrid>)res.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

        public async Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "ingreso_inventario.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos,userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new IngresoInventarioDTOCreate();
            
            dto.IngresoInventarioId = 0;            
            dto.Fecha = DateTime.Now;

            return PartialView("_CreateMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(IngresoInventarioDTOCreate dto)      // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.IngresoInventarioId == 0)
                ListPermit.Add("ingreso_inventario.crear");
            else
                ListPermit.Add("ingreso_inventario.editar");

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
                if (dto.IngresoInventarioId == 0)
                {
                    // INSERT
                    // Validación Adicional if exist
                    if (db.ORTSolicitudesPedido.Any(x => x.SolicitudPedidoId == dto.IngresoInventarioId))
                    {
                        return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    }

                    var reg = new ORTIngresoInventario()
                    {                        
                        Fecha = dto.Fecha,
                         OrdenCompraId = dto.OrdenCompraId,
                        Observacion = dto.Observacion                        
                    };

                    db.ORTIngresosInventario.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTIngresosInventario
                        .Where(x => x.IngresoInventarioId == dto.IngresoInventarioId)
                        .FirstOrDefaultAsync();

                    reg.Fecha = dto.Fecha;
                    reg.OrdenCompraId = dto.OrdenCompraId;
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
        public async Task<JsonResult> JsSaveChild(IngresoInventarioDTOCEditChild dto)   // POST
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("ingreso_inventario.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("ingreso_inventario.editar");

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
                        Tipo = "ING",                        
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        OrdenCompraId = dto.OrdenCompraId,
                        ProveedorId = dto.ProveedorId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                        BodegaId = dto.BodegaId,
                        MaterialId = dto.MaterialId,
                        Documento = dto.IngresoInventarioId
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var IngInv = await db.ORTIngresosInventario
                                .Where(x => x.IngresoInventarioId == dto.IngresoInventarioId)
                                .FirstOrDefaultAsync();
                            IngInv.Lineas += 1;

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

                            reg.Cantidad = dto.Cantidad;
                            reg.Precio = dto.Precio;
                            reg.Valor = dto.Valor;

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

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                            // POST
        {
            string[] permisosRequeridos = { "ingreso_inventario.eliminar" };
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
                        var reg = await db.ORTIngresosInventario
                            .Where(x => x.IngresoInventarioId == id)
                            .FirstOrDefaultAsync();
                        var det = await db.ORTMovimientos
                            .Where(x => x.Documento == id && x.Tipo == "ING")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTIngresosInventario.Remove(reg);

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

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)               // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.Documento == id && x.Tipo == "ING")
                .Include(i => i.Material)
                .Select(s => new IngresoInventarioDTOCEditGrid()
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
            var dto = new IngresoInventarioDTOCEditChild();
            if (id == null)
            {
                dto.SolicitudPedidoId = 0;
                dto.Mode = "INS";
                ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.Material)
                    .Include(i => i.Bodega)
                    .Include(i => i.OrdenCompra)
                    .FirstOrDefaultAsync();

                dto.MovimientoId = reg.MovimientoId;
                dto.SolicitudPedidoId = reg.SolicitudPedidoId;                
                dto.Fecha = reg.Fecha;
                dto.BodegaNombre = reg.Bodega.Nombre;
                dto.BodegaDescripcion = reg.Bodega.Descripcion;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                //dto.MaterialPrecio = reg.Material.Precio;
                dto.UnidadMedida = reg.Material.UnidadMedida;
                dto.MaterialId = reg.MaterialId;
                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;

                var reg2 = OrtopediaRep.GetOrdenCompraMaterialFull(reg.OrdenCompraId ?? 0, reg.MaterialId).Where(x => x.OrdenCompraId == reg.OrdenCompraId && x.MaterialId == reg.MaterialId).FirstOrDefault();
                dto.Solicitado = reg2.Solicitado;
                dto.Ordenado = reg2.Ordenado;
                dto.Pendiente = reg2.Pendiente + dto.Cantidad;
                dto.Mode = "UPD";
                ViewBag.MaterialId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.MaterialCodigo, Value = dto.MaterialId.ToString() }
                    }, "Value", "Text");
            }

            return PartialView("_CEditChild", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteChild(Int64 id)                           // POST 
        {
            string[] permisosRequeridos = { "ingreso_inventario.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId );
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

        public async Task<ActionResult> JsView(int? id)                                 // GET 
        {
            var reg = await db.ORTIngresosInventario
                .Include(i => i.OrdenCompra)
                .Include("OrdenCompra.SolicitudPedido")
                .Where(x => x.IngresoInventarioId == id)
                .FirstOrDefaultAsync();

            var dto = new IngresoInventarioDTOViewMaster()
            {
                Fecha = reg.Fecha,
                IngresoInventarioId = reg.IngresoInventarioId,
                NumeroOC = reg.OrdenCompra.Numero,
                FechaOC = reg.OrdenCompra.Fecha,
                AnioOC = reg.OrdenCompra.Anio,
                NumeroSP = reg.OrdenCompra.SolicitudPedido?.Numero ?? 0 ,
                FechaSP = reg.OrdenCompra.SolicitudPedido?.Fecha,
                AnioSP = reg.OrdenCompra.SolicitudPedido?.Anio ?? 0
                //Numero = reg.Numero,
                //TipoPrioridad = reg.TipoPrioridad,
                //TipoPrioridadText = TipoPrioridad.GetText(reg.TipoPrioridad),
                //Observacion = reg.Observacion,
                //TipoEvento = reg.TipoEvento,
                //TipoEventoText = TipoEvento.GetText(reg.TipoEvento),
                //Elaboro = reg.Elaboro,
                //Solicito = reg.Solicito,
                //JefeDepartamento = reg.JefeDepartamento,
                //Director = reg.Director,
                //Gerente = reg.Gerente
            };

            //dto.Detalle = new List<MovimientoDTOBase>();

            var regs = await db.ORTMovimientos.Where(x => x.Documento == id && x.Tipo == "ING")
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
            return PartialView("_View", dto);
        }

        public async Task<ActionResult> JsViewChild(Int64? id)                          // GET 
        {
            //Permit[] permisosRequeridos = { Permit.View };
            //bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            //if (!hasPermit)
            //{
            //    return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            //}
            IngresoInventarioDTOViewChild dto;
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
                dto = new IngresoInventarioDTOViewChild()
                {
                    BodegaNombre = reg.Bodega.Nombre,
                    BodegaDescripcion = reg.Bodega.Descripcion,

                    MaterialCodigo = reg.Material.Codigo,
                    MaterialNombre = reg.Material.Descripcion,

                    Cantidad = reg.Cantidad,
                    Precio = reg.Precio,
                    Valor = reg.Valor,
                    Fecha = reg.Fecha,

                    //PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString(),
                    //PacienteNombreCompleto = reg.Paciente.Nombre,
                    //NumeroOC = reg.OrdenCompra.Numero,
                    //AnioOC = reg.OrdenCompra.Anio,
                    //FechaOC = reg.OrdenCompra.Fecha,

                    //CirujanoId = reg.CirujanoId,
                    //CirujanoNombre = reg.Cirujano.Nombre,

                    //ProveedorNit = reg.Proveedor.Nit,
                    //ProveedorNombre = reg.Proveedor.Descripcion,

                    //MaterialPrecio = reg.Material.Precio,
                    //UnidadMedida = reg.Material.UnidadMedida,
                };
            }


            return PartialView("_ViewChild", dto);
        }
    }
}