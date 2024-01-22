using Apphr.Application.Common.DTOs;
using Apphr.Application.Common.Models;
using Apphr.Application.Ortopedia.OrdenesCompra.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.Application.Ortopedia.SolicitudesPedido.DTOs;
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
    public class OrdenCompraController : DbController
    {
        private OrtopediaRepository OrtopediaRepo;

        public OrdenCompraController()
        {
            OrtopediaRepo = new OrtopediaRepository(db);
        }

        [Can("orden_compra.ver")]
        public ActionResult Index()                                                     // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [AppAuthorization(Permit.Edit, Permit.Create)]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")              // GET 
        {
            var reg = await db.ORTOrdenesCompra
                .Where(x => x.OrdenCompraId == id)
                .Include(i => i.SolicitudPedido)
                .Include(i => i.Proveedor)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                return HttpNotFound();
            }
            var dto = new OrdenCompraDTOCEdit()
            {
                Anio = reg.Anio,
                OrdenCompraId = reg.OrdenCompraId,
                Fecha = reg.Fecha,
                Numero = reg.Numero,
                Observacion = reg.Observacion,
                SolicitudPedidoId = reg.SolicitudPedidoId ?? 0,
                NumeroSP = reg.SolicitudPedido?.Numero ?? 0,
                FechaSP = reg.SolicitudPedido?.Fecha,
                AnioSP = reg.SolicitudPedido?.Anio ?? 0,
                ProveedorId = reg.ProveedorId,
                ProveedorNit = reg.Proveedor.Nit,
                ProveedorNombre = reg.Proveedor.Descripcion
            };
            ViewBag.Mode = Mode;
            ViewBag.ProveedorId = new SelectList(
                   new List<SelectListItem>
                   {
                         new SelectListItem { Selected = true,Text = dto.ProveedorNit,Value = dto.ProveedorId.ToString() }
                   }, "Value", "Text");
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View(dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsFilterIndex(string Buscar, int? Page)                     // GET 
        {
            IQueryable<ORTOrdenCompra> regs;
            int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

            regs = (from p in db.ORTOrdenesCompra.Include(i => i.SolicitudPedido)  select p);

            if (Buscar != null)
            {
                if (!string.IsNullOrEmpty(Buscar))
                    regs = regs.Where(x => x.Numero.ToString().Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
            }

            var rows = await regs.OrderBy(x => x.OrdenCompraId).ToListAsync();
            List<OrdenCompraDTOIxGrid> res = new List<OrdenCompraDTOIxGrid>();
            foreach (var s in rows)
            {
                res.Add(new OrdenCompraDTOIxGrid()
                {
                    Numero = s.Numero,
                    Anio = s.Anio,
                    OrdenCompraId = s.OrdenCompraId,
                    NumeroSP = s.SolicitudPedido?.Numero ?? 0,
                    FechaSP = s.SolicitudPedido?.Fecha,
                    Fecha = s.Fecha,
                    Observacion = s.Observacion
                });
            }
            
            var dto = (PagedList<OrdenCompraDTOIxGrid>)res.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }

        public async  Task<ActionResult> JsGetCreateForm()                                           // GET 
        {
            string[] permisosRequeridos = { "orden_compra.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new OrdenCompraDTOCreate();
            ViewBag.ListTipoPrioridad = TipoPrioridad.GetSelectList();
            ViewBag.ListTipoEvento = TipoEvento.GetSelectList();
            dto.OrdenCompraId = 0;
            dto.Anio = DateTime.Now.Year;
            dto.Fecha = DateTime.Now;

            return PartialView("_CreateMaster", dto);
        }

        public async Task<ActionResult> JsGetFormChild(int? id)                         // GET 
        {
            var dto = new OrdenCompraDTOCEditChild();
            if (id == null)
            {
                dto.OrdenCompraId = 0;

                ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.Material)
                    .Include(i => i.Proveedor)
                    .FirstOrDefaultAsync();

                dto.MovimientoId = reg.MovimientoId;
                dto.SolicitudPedidoId = reg.SolicitudPedidoId;
                dto.Fecha = reg.Fecha;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                dto.MaterialPrecio = reg.Material.Precio;
                dto.UnidadMedida = reg.Material.UnidadMedida;
                dto.MaterialId = reg.MaterialId;

                dto.ProveedorId = reg.Proveedor.ProveedorId;
                dto.ProveedorNit = reg.Proveedor.Nit;
                dto.ProveedorNombre = reg.Proveedor.Descripcion;

                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;

                
            }
            return PartialView("_CEditChild", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(OrdenCompraDTOCreate dto)            // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.OrdenCompraId == 0)
                ListPermit.Add("orden_compra.crear");
            else
                ListPermit.Add("orden_compra.editar");
                        
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
                if (dto.OrdenCompraId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    if (db.ORTOrdenesCompra.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    {
                        return Json(new { success = false, message = "Esta Orden de Compra ya esta registrada." });
                    }

                    var reg = new ORTOrdenCompra()
                    {
                        Anio = dto.Fecha.Year,
                        Fecha = dto.Fecha,
                        Numero = dto.Numero,
                        Observacion = dto.Observacion,
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        ProveedorId = dto.ProveedorId,
                        Lineas = 0
                    };

                    db.ORTOrdenesCompra.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTOrdenesCompra
                        .Where(x => x.OrdenCompraId == dto.OrdenCompraId)
                        .FirstOrDefaultAsync();

                    reg.Fecha = dto.Fecha;
                    reg.ProveedorId = dto.ProveedorId;
                    reg.SolicitudPedidoId = dto.SolicitudPedidoId;
                    //reg.Gerente = dto.Gerente;
                    //reg.Director = dto.Director;
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
            string[] permisosRequeridos = { "orden_compra.eliminar" };
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
                        var reg = await db.ORTOrdenesCompra
                            .Where(x => x.OrdenCompraId == id)
                            .FirstOrDefaultAsync();

                        if (reg.SolicitudPedidoId != null)
                        {
                            var sol = await db.ORTSolicitudesPedido.FindAsync(reg.SolicitudPedidoId);
                            sol.OrdenCompraId = null;

                            await db.ORTMovimientos
                                .Where(x => x.SolicitudPedidoId == reg.SolicitudPedidoId && x.Tipo == "SOL")
                                .ForEachAsync(x => x.OrdenCompraId = null);
                        }

                        await db.SaveChangesAsync();

                        var det = await db.ORTMovimientos
                            .Where(x => x.OrdenCompraId == id && x.Tipo == "ORD")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTOrdenesCompra.Remove(reg);

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
        public async Task<JsonResult> JsSaveChild(SolicitudPedidoDTOCEditChild dto)     // POST 
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("orden_compra.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("orden_compra.editar");

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
                        Tipo = "ORD",
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        OrdenCompraId = dto.OrdenCompraId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                        ProveedorId = dto.ProveedorId,
                        MaterialId = dto.MaterialId
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var OrdCom = await db.ORTOrdenesCompra
                                .Where(x => x.OrdenCompraId == dto.OrdenCompraId)
                                .FirstOrDefaultAsync();
                            OrdCom.Lineas += 1;

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
                            reg.ProveedorId = dto.ProveedorId;

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
        public async Task<JsonResult> JsDeleteChild(Int64 id)                             // POST 
        {
            string[] permisosRequeridos = { "orden_compra.eliminar" };
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

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)                    // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.OrdenCompraId == id && x.Tipo == "ORD")
                .Include(i => i.Material)
                .Select(s => new MovimientoDTOJsGrid()
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

        public async Task<ActionResult> JsViewMaster(int? id)                                 // GET 
        {
            var reg = await db.ORTOrdenesCompra
                .Include(i => i.SolicitudPedido)
                .Include(i => i.Proveedor)
                .Where(x => x.OrdenCompraId == id)
                .FirstOrDefaultAsync();
            var dto = new OrdenCompraDTOView()
            {
                Anio = reg.Anio,
                Fecha = reg.Fecha,
                Numero = reg.Numero,
                NumeroSP = reg.SolicitudPedido?.Numero ?? 0,
                FechaSP = reg.SolicitudPedido?.Fecha,
                AnioSP = reg.SolicitudPedido?.Anio ?? 0,              
                Observacion = reg.Observacion,
                ProveedorNit = reg.Proveedor.Nit,
                ProveedorNombre = reg.Proveedor.Descripcion
            };

            var regs = await db.ORTMovimientos.Where(x => x.OrdenCompraId == id && x.Tipo == "ORD")
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
            OrdenCompraDTOViewChild dto;
            var reg = await db.ORTMovimientos
                //.Include(i => i.Paciente)
                .Include(i => i.Material)
                //.Include(i => i.Bodega)
                .Include(i => i.Proveedor)
                //.Include(i => i.OrdenCompra)
                //.Include(i => i.Cirujano)
                .Where(x => x.MovimientoId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                dto = null;
            }
            else
            {
                dto = new OrdenCompraDTOViewChild()
                {
                    //PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString(),
                    //PacienteNombreCompleto = reg.Paciente.Nombre,
                    //BodegaNombre = reg.Bodega.Nombre,
                    //BodegaDescripcion = reg.Bodega.Descripcion,
                    //NumeroOC = reg.OrdenCompra.Numero,
                    //AnioOC = reg.OrdenCompra.Anio,
                    //FechaOC = reg.OrdenCompra.Fecha,

                    //CirujanoId = reg.CirujanoId,
                    //CirujanoNombre = reg.Cirujano.Nombre,

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

        public async Task<JsonResult> JsFindOrdenCompra(string id)                      // GET  
        {
            var result = new List<SolicitudPedidoDTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(id))
            {
                result = await db.ORTOrdenesCompra
                    .Where(x => x.Numero.ToString().Contains(id))
                    .Take(autoCompleteSize)
                    .Select(p => new SolicitudPedidoDTOAutocompleteItem
                    {
                        id = p.OrdenCompraId.ToString(),
                        text = p.Numero.ToString(),
                        desc = "(" + p.Anio + ") " + p.Numero,
                        anio = p.Anio
                    })
                   .ToListAsync();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetOrdenCompraByCodigo(string id)
        {

            try
            {
                int OrdenCompraId = Convert.ToInt32(id);
                var reg = await db.ORTOrdenesCompra
                    .Where(x => x.OrdenCompraId == OrdenCompraId)
                    .Include(i => i.SolicitudPedido)
                    .FirstOrDefaultAsync();   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }
                var dto = new
                {
                    SolicitudPedidoId = reg.SolicitudPedidoId,
                    OrdenCompraId = reg.OrdenCompraId,
                    NumeroOC = reg.Numero,
                    FechaOC = reg.Fecha,
                    AnioOC = reg.Anio,
                    NumeroSP = reg.SolicitudPedido.Numero,
                    FechaSP = reg.SolicitudPedido.Fecha,
                    AnioSP = reg.SolicitudPedido.Anio,
                    ProveedorId = reg.ProveedorId,
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message  }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsGetMaterialOCById(Int64 OrdenCompraId, Int64 MaterialId)
        {
            var dto = OrtopediaRepo.GetOrdenCompraMaterialFull(OrdenCompraId, MaterialId).FirstOrDefault();
            return Json(new { success = (dto != null), data = dto }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> JsGetOrdenCompraById(string id)   // 
        {
            try
            {                
                int OrdenCompraId = Convert.ToInt32(id);
                //var OrdenFull = OrtopediaRepo.GetOrdenCompraMaterialFull(OrdenCompraId, 169963);
                var reg = await db.ORTOrdenesCompra
                    .Where(x => x.OrdenCompraId == OrdenCompraId)
                    .Include(i => i.SolicitudPedido)
                    .FirstOrDefaultAsync();   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }
                var dto = new
                {
                    SolicitudPedidoId = reg.SolicitudPedidoId,
                    OrdenCompraId = reg.OrdenCompraId,
                    NumeroOC = reg.Numero,
                    FechaOC = reg.Fecha,
                    AnioOC = reg.Anio,
                    NumeroSP = reg.SolicitudPedido.Numero,
                    FechaSP = reg.SolicitudPedido.Fecha,
                    AnioSP = reg.SolicitudPedido.Anio,
                    ProveedorId = reg.ProveedorId,
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> JsGetByFilter(Int64 OrdenCompraId, string f, string tipo = "AC")
        {
            Object res = null;
            var result = from ocx in db.ORTMovimientos.Where(x => x.OrdenCompraId == OrdenCompraId && x.Tipo =="ORD")
                         join mt in db.Materiales on ocx.MaterialId equals mt.MaterialId select new { ocx, mt };

            if (!string.IsNullOrEmpty(f))
                result = result.Where(x => x.mt.Codigo.Contains(f) || x.mt.Descripcion.Contains(f));

            result = result.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = await result.Select(p => new DTOAutocompleteItem { id = p.mt.Codigo, text = p.mt.Descripcion })
                    .ToListAsync();
            }
            if (tipo == "S")
            {
                res = result.Select(s => new DTOSelect2
                {
                    id = s.mt.MaterialId.ToString(),
                    text = s.mt.Codigo,
                    html = "<div style=\"font-weight: bold;\">" + s.mt.Codigo + "</div><div style=\"font-size: 0.75em;\">" + s.mt.Descripcion + "</div>"
                })
                .ToList();
            }

            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetByCodigo(Int64 OrdenCompraId, string Codigo)
        {
            Int64? SolicitudPedidoId = null;
            //int? MaterialId = null;
            var OC = await db.ORTOrdenesCompra.Include(i => i.SolicitudPedido).Where(x => x.OrdenCompraId == OrdenCompraId).FirstOrDefaultAsync();
            if (OC != null)
            {
                SolicitudPedidoId = OC.SolicitudPedido.SolicitudPedidoId;
            }

            var mat = await db.Materiales.Where(x => x.Codigo == Codigo).FirstOrDefaultAsync();
            if (mat == null)
            {
                return null;
            }

            //var regs = await db.ORTMovimientos
            //   .Where(x => x.SolicitudPedidoId == SolicitudPedidoId && x.MaterialId == mat.MaterialId)
            //   .ToListAsync();

            var regs = OrtopediaRepo.GetOrdenCompraMaterialFull(OrdenCompraId, mat.MaterialId).FirstOrDefault();

            //var Solicitado = regs.Where(x => x.Tipo == "SOL").Select(x => x.Cantidad).Sum();
            //var Ordenado = regs.Where(x => x.Tipo == "ORD").Select(x => x.Cantidad).Sum();
            //var Ingresado = regs.Where(x => x.Tipo == "ING").Select(x => x.Cantidad).Sum();
            //var Pendiente = Ordenado - Ingresado;
            //var PrecioOC = regs.Where(x => x.Tipo == "ORD").Select(x => x.Precio).FirstOrDefault();
            var res = new { 
                mat.MaterialId, 
                mat.Codigo,
                mat.Codigi,
                mat.Descripcion,
                mat.Producto,
                Precio = regs.PrecioOC,
                mat.UnidadMedida,
                Solicitado = regs.Solicitado,
                Ordenado = regs.Ordenado,
                Pendiente = regs.Pendiente,
            };
            
            return Json(new { success = true, data =  res}, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetById(Int64 OrdenCompraId, int MaterialId)
        {
            Int64? SolicitudPedidoId = null;
            //int? MaterialId = null;
            var OC = await db.ORTOrdenesCompra.Include(i => i.SolicitudPedido).Where(x => x.OrdenCompraId == OrdenCompraId).FirstOrDefaultAsync();
            if (OC != null)
            {
                SolicitudPedidoId = OC.SolicitudPedido.SolicitudPedidoId;
            }

            var mat = await db.Materiales.Where(x => x.MaterialId == MaterialId).FirstOrDefaultAsync();
            if (mat == null)
            {
                return null;
            }

            
            var regs = OrtopediaRepo.GetOrdenCompraMaterialFull(OrdenCompraId, MaterialId).FirstOrDefault();

            
            var res = new
            {
                mat.MaterialId,
                mat.Codigo,
                mat.Codigi,
                mat.Descripcion,
                mat.Producto,
                Precio = regs.PrecioOC,
                mat.UnidadMedida,
                Solicitado = regs.Solicitado,
                Ordenado = regs.Ordenado,
                Pendiente = regs.Pendiente,
            };

            return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetMaterialProveedoresByFilter(string f, int MaterialId)
        {            
            var regs = await db.ORTMovimientos.Include(i => i.Proveedor)
                .Where(x => x.MaterialId == MaterialId && x.Tipo == "ORD")
                .Select(s => new DTOSelect2
                {
                    id = s.Proveedor.ProveedorId.ToString(),
                    text = s.Proveedor.Nit,
                    html = "<div style=\"font-weight: bold;\">" + s.Proveedor.Nit + "</div><div style=\"font-size: 0.75em;\">" + s.Proveedor.Descripcion + "</div>"
                })
                .ToListAsync();
            

            return Json(new { success = true, data = regs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsFindOCByMatProvId(int? MaterialId, int? ProveedorId)
        {
            var regs = (from row in db.ORTMovimientos //.Where(x => x.Tipo == "ORD" && x.MaterialId == MaterialId && x.ProveedorId == ProveedorId)
                        group row by row.SolicitudPedidoId into grp
                        select new
                        {
                            SolicitudPedidoId = grp.Key,
                            Solicitado = grp.Sum(r => (r.Tipo == "SOL" ? r.Cantidad : 0)),
                            Ordenado = grp.Sum(r => (r.Tipo == "ORD" ? r.Cantidad : 0)),
                            Ingresado = grp.Sum(r => ((r.Tipo == "ING" || r.Tipo == "AJU") ? r.Cantidad : 0)),
                            Despachado = grp.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0))
                        }); //.ToList();
            var regs2 = (from x in regs
                         join oc in db.ORTOrdenesCompra on x.SolicitudPedidoId equals oc.SolicitudPedidoId
                         where oc.ProveedorId == ProveedorId
                         select new DTOSelect2
                         {
                             //SolicitudPedidoId = x.SolicitudPedidoId,
                             id = oc.OrdenCompraId.ToString(),
                             text = oc.Numero.ToString(),
                             html = "<div style=\"font-weight: bold;\">" + oc.Numero.ToString() + "</div><div style=\"font-size: 0.75em;\">Disponible " + (x.Ingresado - x.Despachado).ToString() + "</div>"
                         }).ToList();
            //var regs = await db.ORTMovimientos
            //    .Where(x => x.Tipo == "ORD" && x.MaterialId == MaterialId && x.ProveedorId == ProveedorId)
            //    .Select(s => new DTOSelect2
            //    {
            //        id = s.OrdenCompraId.ToString(),
            //        text = s.OrdenCompra.Numero.ToString(),
            //        html = "<div style=\"font-weight: bold;\">" + s.OrdenCompra.Numero + "</div><div style=\"font-size: 0.75em;\">Disponible XXX" + "</div>"
            //    })
            //    .ToListAsync();
            //var regs = new {};
            return Json(new { success = true, data = regs2 }, JsonRequestBehavior.AllowGet);
        }
    }
}
