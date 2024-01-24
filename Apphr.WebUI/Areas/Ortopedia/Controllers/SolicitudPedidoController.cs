using Apphr.Application.Common;
using Apphr.Application.Common.Models;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.Application.Ortopedia.SolicitudesPedido.DTOs;
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
	public class SolicitudPedidoController : DbController
    {
        //[Can(.".ver")]
        [Can("solicitud_pedido.ver")]
        public ActionResult Index()                                                             // GET 
        {
            ViewBag.Permissions = Utilidades.GetCans(userId);            
            //ViewBag.Permissions = Utilidades.GetCans(userId);
            return View();
        }

        [Can("solicitud_pedido.editar", "solicitud_pedido.crear")]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")                      // GET 
        {
            var reg = await db.ORTSolicitudesPedido.FindAsync(id);
            if (reg == null)
            {
                return HttpNotFound();
            }
            var dto = new SolicitudPedidoDTOEdit()
            {
                Anio = reg.Anio,
                SolicitudPedidoId = reg.SolicitudPedidoId,
                Fecha = reg.Fecha,
                Numero = reg.Numero,
                TipoPrioridad = reg.TipoPrioridad,
                TipoEvento = reg.TipoEvento,
                Elaboro = reg.Elaboro,
                Solicito = reg.Solicito,
                JefeDepartamento = reg.JefeDepartamento,
                Gerente = reg.Gerente,
                Director = reg.Director,
                Observacion = reg.Observacion
            };
            ViewBag.Mode = Mode;
            ViewBag.ListTipo = TipoPrioridad.GetSelectList();
            ViewBag.ListTipoEvento = TipoEvento.GetSelectList();
            ViewBag.Permissions = Utilidades.GetCans(userId);            
            return View(dto);
        }

        [ValidateAntiForgeryToken]
		public ActionResult JsIndexGrid(string Buscar, int? Page)				                // GET 
		{
			IQueryable<ORTSolicitudPedido> regs;
			int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

			regs = (from p in db.ORTSolicitudesPedido select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs.Where(x => x.Numero.ToString().Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
			}            

            var rows = (from p in regs
                        join o in db.ORTOrdenesCompra on p.OrdenCompraId equals o.OrdenCompraId into ordenesCompraGroup
                        from o in ordenesCompraGroup.DefaultIfEmpty()
                        orderby p.SolicitudPedidoId
                        select new SolicitudPedidoDTOIxGrid
                        {
                            SolicitudPedidoId = p.SolicitudPedidoId,
                            Anio = p.Anio,
                            Numero = p.Numero,
                            Fecha = p.Fecha,
                            TipoEvento = p.TipoEvento,
                            TipoPrioridad = p.TipoPrioridad,
                            Observacion = p.Observacion,
                            NumeroOC = o.Numero,
                            FechaOC = o.Fecha
                        }).ToList();

            var dto = (PagedList<SolicitudPedidoDTOIxGrid>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

		public async Task<ActionResult> JsGetCreateForm()                                                   // GET 
        {
            string[] permisosRequeridos = { "solicitud_pedido.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new SolicitudPedidoDTOCreate();
            ViewBag.ListTipoPrioridad = TipoPrioridad.GetSelectList();
            ViewBag.ListTipoEvento = TipoEvento.GetSelectList();
            dto.SolicitudPedidoId = 0;
            dto.Anio = DateTime.Now.Year;
            dto.Fecha = DateTime.Now;

			return PartialView("_CreateMaster", dto);
        }

        public async Task<ActionResult> JsGetFormChild(int? id)                                 // GET 
        {
            var dto = new SolicitudPedidoDTOCEditChild();
            if (id == null)
            {
                dto.SolicitudPedidoId = 0;
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.Material)
                    .FirstOrDefaultAsync();
            
                dto.MovimientoId = reg.MovimientoId;
                dto.SolicitudPedidoId = reg.SolicitudPedidoId;
                dto.Fecha = reg.Fecha;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                dto.MaterialPrecio = reg.Material.Precio;
                dto.UnidadMedida = reg.Material.UnidadMedida;
                dto.MaterialId = reg.MaterialId;                               
                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;
            }
            
            return PartialView("_CEditChild", dto);
        }


        public async Task<ActionResult> JsView(int? id)                                         // GET 
        {
            var reg = await db.ORTSolicitudesPedido.FindAsync(id);
            var dto = new SolicitudPedidoDTOView()
            {
                Anio = reg.Anio,
                Fecha = reg.Fecha,
                Numero = reg.Numero,
                TipoPrioridad = reg.TipoPrioridad,
                TipoPrioridadText = TipoPrioridad.GetText(reg.TipoPrioridad),
                Observacion = reg.Observacion,
                TipoEvento = reg.TipoEvento,
                TipoEventoText = TipoEvento.GetText(reg.TipoEvento),
                Elaboro = reg.Elaboro,
                Solicito = reg.Solicito,
                JefeDepartamento = reg.JefeDepartamento,
                Director = reg.Director,
                Gerente = reg.Gerente
            };

            //dto.Detalle = new List<MovimientoDTOBase>();

            var regs = await db.ORTMovimientos.Where(x => x.SolicitudPedidoId == id && x.Tipo == "SOL")
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

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)                       // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.SolicitudPedidoId == id)
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

        public async Task<JsonResult> JsFindSolicitudPedido(string id)                          // GET 
        {
            var result = new List<SolicitudPedidoDTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(id))
            {
                result = await db.ORTSolicitudesPedido.Where(x => x.Numero.ToString().Contains(id)) //|| x.Descripcion.Contains(id)
                   .Take(autoCompleteSize)
                   .Select(p => new SolicitudPedidoDTOAutocompleteItem
                   {
                       id = p.SolicitudPedidoId.ToString(),
                       text = p.Numero.ToString(),
                       desc = "(" + p.Anio + ") " + p.Numero,
                       anio = p.Anio
                   })
                   .ToListAsync();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetSolicitudPedidoByNoAnio(int no, int anio)            // GET 
        {
            try
            {
                var reg = await db.ORTSolicitudesPedido
                    .Where(x => x.Numero == no && x.Anio == anio)
                    .FirstOrDefaultAsync();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }

                var dto = new
                {
                    SolicitudPedidoId = reg.SolicitudPedidoId,
                    Numero = reg.Numero,
                    Fecha = reg.Fecha
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> JsGetSolicituPedidoByCodigo(string id)                    // GET 
        {

            try
            {
                int SolictiduPedidoId = Convert.ToInt32(id);
                var reg = await db.ORTSolicitudesPedido.FindAsync(SolictiduPedidoId);   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }
                var dto = new
                {
                    SolicitudPedidoId = reg.SolicitudPedidoId,
                    Numero = reg.Numero,
                    NumeroOC = reg.NumeroOC
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> JsGetByFilter(Int64 SolicitudPedidoId, string f)// GET 
        {
            var result = new List<MaterialDTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(f))
            {
                result = await (from sp in db.ORTSolicitudesPedido
                                join mv in db.ORTMovimientos on sp.SolicitudPedidoId equals mv.SolicitudPedidoId
                                join mt in db.Materiales on mv.MaterialId equals mt.MaterialId
                                where (sp.SolicitudPedidoId == SolicitudPedidoId && (mt.Codigo.Contains(f) || mt.Descripcion.Contains(f)))
                                select new MaterialDTOAutocompleteItem
                                {
                                    MaterialId = mt.MaterialId.ToString(),
                                    id = mt.Codigo,
                                    text = "(" + mt.Codigi + ") " + mt.Descripcion
                                })
                                .Take(autoCompleteSize)
                                .ToListAsync();


                //result = await db.ORTSolicitudesPedido.Where(x => x.SolicitudPedidoId.ToString().Contains(id) || x.Descripcion.Contains(id))
                //   .Take(autoCompleteSize)
                //   .Select(p => new MaterialDTOAutocompleteItem { 
                //       MaterialId = p.MaterialId.ToString(), 
                //       id = p.Codigo, text = "(" + p.Codigi + ") " + p.Descripcion
                //   })
                //   .ToListAsync();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(SolicitudPedidoDTOCreate dto)                // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.SolicitudPedidoId == 0)
                ListPermit.Add("solicitud_pedido.crear");
            else
                ListPermit.Add("solicitud_pedido.editar");
            
            //Permit[] permisosRequeridos = ListPermit.ToArray();
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
                if (dto.SolicitudPedidoId == 0)
                {
                    // INSERT
                    // Validación Adicional
                    if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
                    {
                        return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    }

                    var reg = new ORTSolicitudPedido()
                    {
                        Anio = dto.Fecha.Year,
                        Fecha = dto.Fecha,
                        Numero = dto.Numero,
                        TipoPrioridad = dto.TipoPrioridad,
                        TipoEvento = dto.TipoEvento,
                        Elaboro = dto.Elaboro,
                        Solicito = dto.Solicito,
                        JefeDepartamento = dto.JefeDepartamento,
                        Gerente = dto.Gerente,
                        Director = dto.Director,
                        Observacion = dto.Observacion,
                        Lineas = 0
                    };

                    db.ORTSolicitudesPedido.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTSolicitudesPedido
                        .Where(x => x.SolicitudPedidoId == dto.SolicitudPedidoId)
                        .FirstOrDefaultAsync();
                    reg.TipoPrioridad = dto.TipoPrioridad;
                    reg.TipoEvento = dto.TipoEvento;
                    reg.Elaboro = dto.Elaboro;
                    reg.Solicito = dto.Solicito;
                    reg.JefeDepartamento = dto.JefeDepartamento;
                    reg.Gerente = dto.Gerente;
                    reg.Director = dto.Director;
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
        public async Task<JsonResult> JsDeleteMaster(int id)                                    // POST 
        {
            string[] permisosRequeridos = { "solicitud_pedido.eliminar" };
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
                        var reg = await db.ORTSolicitudesPedido
                            .Where(x => x.SolicitudPedidoId == id)
                            .FirstOrDefaultAsync();

                        if (reg != null && reg.OrdenCompraId != null)
                        {
                            return Json(new { success = false, message = "Esta solicitud ya tiene una OC asociada, de eliminar antes la solicitud !" }, JsonRequestBehavior.DenyGet);
                        }

                        var det = await db.ORTMovimientos
                            .Where(x => x.SolicitudPedidoId == id && x.Tipo == "SOL")
                            .ToListAsync();
                        
                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTSolicitudesPedido.Remove(reg);

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
        public async Task<JsonResult> JsSaveChild(SolicitudPedidoDTOCEditChild dto)             // POST 
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
               Permisos.Add("solicitud_pedido.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("solicitud_pedido.editar");

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
                        Tipo = "SOL",
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                        MaterialId = dto.MaterialId
                    };
                    using (DbContextTransaction t = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var SolPed = await db.ORTSolicitudesPedido.Where(x => x.SolicitudPedidoId == dto.SolicitudPedidoId).FirstOrDefaultAsync();
                            SolPed.Lineas += 1;

                            db.ORTMovimientos.Add(reg);
                            await db.SaveChangesAsync();
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.Rollback();
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
        public async Task<JsonResult> JsDeleteChild(Int64 id)                                   // POST 
        {
            string[] permisosRequeridos = { "solicitud_pedido.eliminar" };
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


        [HttpPost]
        public async Task<JsonResult> JsSolicitudExist(string id)                               // POST 
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Request.Params[0];
            }
            var res = await db.ORTSolicitudesPedido.Where(x => x.Numero.ToString() == id).AnyAsync();
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> JsOrdenCompraExist(string id)                             // POST 
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Request.Params[0];
            }
            var res = await db.ORTSolicitudesPedido.Where(x => x.NumeroOC.ToString() == id).AnyAsync();
            return Json(res);
        }        
    }
}
