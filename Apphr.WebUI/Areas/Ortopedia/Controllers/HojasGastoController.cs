using Apphr.Application.Common.DTOs;
using Apphr.Application.Ortopedia.HojaGasto.DTOs;
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
	public class HojasGastoController : DbController
    {
        private OrtopediaRepository OrtopediaRep;
        public HojasGastoController()
        {
            OrtopediaRep = new OrtopediaRepository(db);
        }
		[Can("hoja_gasto.ver")]
		public ActionResult Index()																	// GET
        {
			ViewBag.Permissions = Utilidades.GetCans(userId);
			return View();
        }

		[ValidateAntiForgeryToken]
		public async Task<ActionResult> JsIndexGrid(string Buscar, int? Page)                       // GET
		{
			IQueryable<ORTHojaGasto> regs;
			int pageIndex = Page.HasValue ? Convert.ToInt32(Page) : 1;

			regs = (from p in db.ORTHojasGasto select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs.Where(x => x.Fecha.ToString("d").Contains(Buscar)); // || x.Descripcion.Contains(Buscar));
			}
			var res = await regs
				.OrderBy(x => x.Fecha)
				.Select(s => new HojaGastoDTOIxGrid
				{
					HojaGastoId = s.HojaGastoId,
					Fecha = s.Fecha,
                    HojaControlSala = s.HojaControlSala,
				})
				.ToListAsync();

			var dto = (PagedList<HojaGastoDTOIxGrid>)res.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

		public async Task<ActionResult> JsGetCreateForm()                                                       // GET 
		{
			string[] permisosRequeridos = { "hoja_gasto.crear" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos,userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
			}
			var dto = new HojaGastoDTOCreate();

			dto.HojaGastoId = 0;
			dto.Fecha = DateTime.Now.Date;
			dto.FechaOperacion = DateTime.Now.Date;
            ViewBag.ServicioId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CirujanoId = new SelectList(Enumerable.Empty<SelectListItem>());

            return PartialView("_CreateMaster", dto);
		}

        public ActionResult JsSelectHojaGasto()
        {
            ViewBag.HojaGastoId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.BodegaId = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView("_SelectHojaGasto");
        }

        public ActionResult JsSelectHojaGastoSingle()
        {
            ViewBag.HojaGastoId = new SelectList(Enumerable.Empty<SelectListItem>());
           // ViewBag.BodegaId = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView("_SelectHojaGastoSingle");
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(HojaGastoDTOCreate dto)                          // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.HojaGastoId == 0)
                ListPermit.Add("hoja_gasto.crear");
            else
                ListPermit.Add("hoja_gasto.editar");

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
                if (dto.HojaGastoId == 0)
                {
                    // INSERT
                    // Validación Adicional if exist
                    //if (db.ORTHojasGasto.Any(x => x.HojaGastoId == dto.HojaGastoId))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}

                    var reg = new ORTHojaGasto()
                    {
                        Fecha = dto.Fecha,
                        Observacion = dto.Observacion,
                        FechaOperacion = dto.FechaOperacion,
                        AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante,
                        AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista,
                        Cama = dto.Cama,
                        CirujanoId = dto.CirujanoId,
                        ServicioId = dto.ServicioId,
                        HojaControlSala = dto.HojaControlSala,
                        PacienteId = dto.PacienteId,
                    };

                    db.ORTHojasGasto.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTHojasGasto
                        .Where(x => x.HojaGastoId == dto.HojaGastoId)
                        .FirstOrDefaultAsync();

                    reg.Fecha = dto.Fecha;
                    reg.Observacion = dto.Observacion;
                    reg.FechaOperacion = dto.FechaOperacion;
                    reg.AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante;
                    reg.AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista;
                    reg.Cama = dto.Cama;
                    reg.CirujanoId = dto.CirujanoId;
                    reg.ServicioId = dto.ServicioId;
                    reg.HojaControlSala = dto.HojaControlSala;
                    reg.PacienteId = dto.PacienteId;

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [AppAuthorization(Permit.Edit, Permit.Create)]
        public async Task<ActionResult> CEdit(int id, string Mode = "INS")                          // GET 
        {
            var reg = await db.ORTHojasGasto
                .Include(i => i.Servicio)
                .Include(i => i.Cirujano)
                .Include(i => i.Paciente)
                .Where(x => x.HojaGastoId == id)
                .FirstOrDefaultAsync();

            if (reg == null)
            {
                return View("RegisterNotFound");                
            }
            var dto = new HojaGastoDTOCEditMaster()
            {
                HojaGastoId = reg.HojaGastoId,
                Fecha = reg.Fecha,
                Observacion = reg.Observacion,
                FechaOperacion = reg.FechaOperacion,
                AuxiliarEnfermeriaCirculante = reg.AuxiliarEnfermeriaCirculante,
                AuxiliarEnfermeriaInstrumentista = reg.AuxiliarEnfermeriaInstrumentista,
                Cama = reg.Cama,
                CirujanoId = reg.CirujanoId,
                ServicioId = reg.ServicioId,
                HojaControlSala = reg.HojaControlSala,
                Servicio = reg.Servicio,
                Cirujano = reg.Cirujano,
                PacienteId = reg.PacienteId,
                PacienteNombreCompleto = reg.Paciente.Nombre,
                PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString()
            };
            ViewBag.Mode = Mode;
            //if (Mode == "INS")
            //{
            //    ViewBag.CirujanoId = new SelectList(Enumerable.Empty<SelectListItem>());
            //    ViewBag.ServicioId = new SelectList(Enumerable.Empty<SelectListItem>());
            //}
            ViewBag.ServicioId = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = dto.Servicio.Nombre, Value = dto.ServicioId.ToString() }
                }, "Value", "Text");
            ViewBag.CirujanoId = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = dto.Cirujano.Nombre, Value = dto.CirujanoId.ToString() }
                }, "Value", "Text");

            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View(dto);
        }

        public async Task<ActionResult> JsViewMaster(int? id)                                       // GET 
        {
            HojaGastoDTOViewMaster dto;
            var reg = await db.ORTHojasGasto              
                .Where(x => x.HojaGastoId == id)
                .FirstOrDefaultAsync();
            if (reg == null)
            {
                return PartialView("_RegisterNotFound");
            }

            dto = new HojaGastoDTOViewMaster()
            {
                Fecha = reg.Fecha,
                HojaGastoId = reg.HojaGastoId,
                Observacion = reg.Observacion,
                FechaOperacion = reg.FechaOperacion,
                AuxiliarEnfermeriaCirculante = reg.AuxiliarEnfermeriaCirculante,
                AuxiliarEnfermeriaInstrumentista = reg.AuxiliarEnfermeriaInstrumentista,
                Cama = reg.Cama,
                CirujanoId = reg.CirujanoId,
                ServicioId = reg.ServicioId,
                HojaControlSala = reg.HojaControlSala,
            };

            dto.Detalle = await db.ORTMovimientos
                .Where(x => x.HojaGastoId == id && x.Tipo == "DES")
                .Include(i => i.Material)
                .Select(s => new MovimientoDTOBase()
                {
                    Material = s.Material,
                    Cantidad = s.Cantidad,
                    Precio = s.Precio,
                    Valor = s.Valor
                })
                .ToListAsync();
            

            return PartialView("_ViewMaster", dto);
        }

        public async Task<ActionResult> JsViewChild(Int64? id)                            // GET 
        {
            //Permit[] permisosRequeridos = { Permit.View };
            //bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            //if (!hasPermit)
            //{
            //    return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            //}
            HojaGastoDTOViewChild dto;
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
                dto = new HojaGastoDTOViewChild()
                {
                    ProveedorNit = reg.Proveedor.Nit,
                    ProveedorNombre = reg.Proveedor.Descripcion,
                    MaterialCodigo = reg.Material.Codigo,
                    MaterialNombre = reg.Material.Descripcion,
                    Cantidad = reg.Cantidad,
                    Precio = reg.Precio,
                    Valor = reg.Valor,
                    Fecha = reg.Fecha,
                    //PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString(),
                    //PacienteNombreCompleto = reg.Paciente.Nombre,
                    //BodegaNombre = reg.Bodega.Nombre,
                    //BodegaDescripcion = reg.Bodega.Descripcion,
                    //NumeroOC = reg.OrdenCompra.Numero,
                    //AnioOC = reg.OrdenCompra.Anio,
                    //FechaOC = reg.OrdenCompra.Fecha,
                    //CirujanoId = reg.CirujanoId,
                    //CirujanoNombre = reg.Cirujano.Nombre,
                    //MaterialPrecio = reg.Material.Precio,
                    //UnidadMedida = reg.Material.UnidadMedida,
                };
            }


            return PartialView("_ViewChild", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                                        // POST
        {
            string[] permisosRequeridos = { "hoja_gasto.eliminar" };
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
                        var reg = await db.ORTHojasGasto
                            .Where(x => x.HojaGastoId == id)
                            .FirstOrDefaultAsync();
                        var det = await db.ORTMovimientos
                            .Where(x => x.Documento == id && x.Tipo == "GAS")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTHojasGasto.Remove(reg);

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
        public async Task<JsonResult> JsDeleteChild(Int64 id)                                       // POST 
        {
            string[] permisosRequeridos = { "hoja_gasto.eliminar" };
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

                        var master = await db.ORTHojasGasto.FindAsync(reg.Documento);
                        master.Lineas -= 1;

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
        public async Task<JsonResult> JsSaveChild(HojaGastoDTOCEditChild dto)   // POST
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("hoja_gasto.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("hoja_gasto.editar");

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
                        Tipo = "GAS",
                        //Documento = dto.HojaGastoId,
                        HojaGastoId = dto.HojaGastoId,
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        OrdenCompraId = dto.OrdenCompraId,
                        CirujanoId = dto.CirujanoId,
                        PacienteId = dto.PacienteId,
                        ProveedorId = dto.ProveedorId,
                        //BodegaId = dto.BodegaId,
                        MaterialId = dto.MaterialId,
                        //Documento = dto.DespachoInventarioId,
                        Cantidad = dto.Cantidad,
                        Devolver = dto.Devolver,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Fecha = dto.Fecha,
                        Linea = 0,
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var master = await db.ORTHojasGasto
                                .Where(x => x.HojaGastoId == dto.HojaGastoId)
                                .FirstOrDefaultAsync();
                            master.Lineas += 1;

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
                            reg.CirujanoId = dto.CirujanoId;
                            reg.PacienteId = dto.PacienteId;
                            reg.Cantidad = dto.Cantidad;
                            reg.Precio = dto.Precio;
                            reg.Valor = dto.Valor;
                            reg.Devolver = dto.Devolver;

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

        public async Task<ActionResult> JsCEditGrid(int? id, string mode)                           // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.Tipo == "GAS" && x.HojaGastoId == id)
                .Include(i => i.Material)
                .Select(s => new HojaGastoDTOCEditGrid()
                {
                    MovimientoId = s.MovimientoId,
                    SolicitudPedidoId = s.SolicitudPedidoId,
                    Devolver = s.Devolver,
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
            var dto = new HojaGastoDTOCEditChild();
            if (id == null)
            {
                //dto.SolicitudPedidoId = 0;

                ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
                //ViewBag.OrdenCompraId = new SelectList(Enumerable.Empty<SelectListItem>());
                //ViewBag.CirujanoId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id && x.Tipo == "GAS")
                    .Include(i => i.Paciente)
                    .Include(i => i.Material)
                    //.Include(i => i.Bodega)
                    .Include(i => i.Proveedor)
                    //.Include(i => i.OrdenCompra)
                    .Include(i => i.Cirujano)
                    .FirstOrDefaultAsync();

                dto.MaterialId = reg.MaterialId;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;

                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;

                //dto.PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString();
                //dto.PacienteNombreCompleto = reg.Paciente.Nombre;
                dto.PacienteId = reg.PacienteId;

                //dto.BodegaId = reg.BodegaId ?? 0;
                //dto.BodegaNombre = reg.Bodega.Nombre;
                //dto.BodegaDescripcion = reg.Bodega.Descripcion;

                dto.ProveedorId = reg.Proveedor.ProveedorId;
                dto.ProveedorNit = reg.Proveedor.Nit;
                dto.ProveedorNombre = reg.Proveedor.Descripcion;

                //dto.NumeroOC = reg.OrdenCompra.Numero;
                //dto.AnioOC = reg.OrdenCompra.Anio;
                //dto.FechaOC = reg.OrdenCompra.Fecha;

                dto.CirujanoId = reg.CirujanoId;
                //dto.CirujanoNombre = reg.Cirujano.Nombre;

                dto.MovimientoId = reg.MovimientoId;
                //dto.SolicitudPedidoId = reg.SolicitudPedidoId;
                //dto.OrdenCompraId = reg.OrdenCompraId;

                dto.Fecha = reg.Fecha;
                //dto.MaterialPrecio = reg.Material.Precio;
                //dto.UnidadMedida = reg.Material.UnidadMedida;

                ViewBag.ProveedorId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true,Text = dto.ProveedorNit,Value = dto.ProveedorId.ToString() }
                    }, "Value", "Text");
                //ViewBag.OrdenCompraId = new SelectList(
                //    new List<SelectListItem>
                //    {
                //         new SelectListItem { Selected = true, Text = dto.NumeroOC.ToString(), Value = dto.OrdenCompraId.ToString() }
                //    }, "Value", "Text");
                //ViewBag.CirujanoId = new SelectList(
                //    new List<SelectListItem>
                //    {
                //         new SelectListItem { Selected = true, Text = dto.CirujanoNombre, Value = dto.CirujanoId.ToString() }
                //    }, "Value", "Text");
            }
            //ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());

            return PartialView("_CEditChild", dto);
        }

        public async Task<JsonResult> JsHojasGastoByFilter(string f)
        {
            var regs = await db.ORTHojasGasto
                //.Include(i => i.Proveedor)
                .Where(x => x.HojaControlSala.Contains(f))
                .Select(s => new DTOSelect2
                {
                    id = s.HojaGastoId.ToString(),
                    text = s.HojaControlSala,
                    html = "<div style=\"font-weight: bold;\">" + s.HojaGastoId.ToString() + "</div><div style=\"font-size: 0.75em;\">" + s.HojaControlSala +"</div>"
                })
                .ToListAsync();


            return Json(new { success = true, data = regs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsGetMaterialesByFilterInHojaGasto(Int64 HojaGastoId, int ProveedorId, string f, string tipo = "AC")
        {
            Object res = null;
            //var result = from r in db.Materiales select r;
            var result = OrtopediaRep.GetMaterialesByFilterInHojaGasto(HojaGastoId, ProveedorId);

            //var result = new List<DTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(f))
                result = result.Where(x => x.Codigo.Contains(f) || x.Descripcion.Contains(f));

            result = result.Take(autoCompleteSize);

            if (tipo == "AC")
            {
                res = result.Select(p => new DTOAutocompleteItem { id = p.Codigo, text = p.Descripcion })
                    .ToList();
            }
            if (tipo == "S")
            {
                res = result.Select(s => new DTOSelect2
                {
                    id = s.MaterialId.ToString(),
                    text = s.Descripcion,
                    html = "<div style=\"font-weight: bold;\">" + s.Codigo + "</div><div style=\"font-size: 0.75em;\">" + s.Descripcion + "</div>"
                })
                .ToList();
            }

            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsGetMaterialByIdInHojaGasto(Int64 HojaGastoId, int MaterialId)
        {
            var reg = OrtopediaRep.GetMaterialByIdInHojaGasto(HojaGastoId, MaterialId);
            return Json(new { success = true, data = reg }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsHojaGastoById(Int64 id)
        {
            var reg = await db.ORTHojasGasto
                .Include(i => i.Paciente)
                .Where(x => x.HojaGastoId == id)
                .FirstOrDefaultAsync();
                
            return Json(new { success = true, data = reg }, JsonRequestBehavior.AllowGet);
        }
    }
}