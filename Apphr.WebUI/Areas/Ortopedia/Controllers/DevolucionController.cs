using Apphr.Application.Common.DTOs;
using Apphr.Application.Ortopedia.Devolucion.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Apphr.WebUI.Models.Repository;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Ortopedia.Controllers
{
    [Authorize]
    [LogAction]
    public class DevolucionController : DbController
    {
        private OrtopediaRepository OrtopediaRep;
        public DevolucionController()
        {
            OrtopediaRep = new OrtopediaRepository(db);
        }

        [Can("devolucion.ver")]
        public ActionResult Index()
        {
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View();
        }

        [HttpPost]
        public JsonResult JsGetDataTable(DTOJqueryDatatableParam param)                     // GET 
        {
            List<ORTDevolucionInventario> Data;

            var draw = param.draw;
            var start = param.start;
            var length = param.length;
            var sortColumnName = param.columns[param.order[0].column].name;
            var sortColumnDir = param.order[0].dir;
            var searchValue = param.search.value;
            int pageSize = param.length;
            int skip = param.start;
            int recordsTotal = 0;


            try
            {
                var regs = from p in db.ORTDevoluciones.Include(i => i.Proveedor) select p;

                if (!(string.IsNullOrEmpty(sortColumnName) && string.IsNullOrEmpty(sortColumnDir)))
                    regs = regs.OrderBy(sortColumnName + " " + sortColumnDir);

                if (!string.IsNullOrEmpty(searchValue))

                    regs = regs.Where(m => m.DevolucionId.ToString().Contains(searchValue) || m.Observacion.Contains(searchValue) || m.Proveedor.Descripcion.Contains(searchValue));

                recordsTotal = regs.Count();

                Data = regs.Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { draw = draw, data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }

        public async  Task<ActionResult> JsCreateMaster()                                                // GET 
        {
            string[] permisosRequeridos = { "devolucion.crear" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }
            var dto = new DevolucionDTOCreate();

            dto.DevolucionId = 0;
            dto.Fecha = DateTime.Now;
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView("_CreateMaster", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(DevolucionDTOCreate dto)                 // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.DevolucionId == 0)
                ListPermit.Add("devolucion.crear");
            else
                ListPermit.Add("devolucion.editar");

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
                if (dto.DevolucionId == 0)
                {
                    // INSERT
                    // Validación Adicional if exist
                    //if (db.ORTDespachosInventario.Any(x => x.DespachoInventarioId == dto.DespachoInventarioId))
                    //{
                    //    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
                    //}

                    var reg = new ORTDevolucionInventario()
                    {
                        Fecha = dto.Fecha,
                        Observacion = dto.Observacion,
                        ProveedorId = dto.ProveedorId
                    };

                    db.ORTDevoluciones.Add(reg);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    // UPDATE
                    var reg = await db.ORTDevoluciones
                        .Where(x => x.DevolucionId == dto.DevolucionId)
                        .FirstOrDefaultAsync();

                    reg.Fecha = dto.Fecha;
                    reg.Observacion = dto.Observacion;
                    reg.ProveedorId = dto.ProveedorId;

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
        public async Task<JsonResult> JsSaveChild(DevolucionDTOCEditChild dto)              // POST
        {
            List<string> Permisos = new List<string>();
            if (dto.Mode == "INS")
                Permisos.Add("devolucion.crear");
            if (dto.Mode == "UPD")
                Permisos.Add("devolucion.editar");

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
                        Tipo = "DEV",
                        DevolucionId = dto.DevolucionId,
                        ProveedorId = dto.ProveedorId, //*
                        PacienteId = dto.PacienteId, //*
                        Fecha = dto.Fecha,                       
                        HojaGastoId = dto.HojaGastoId,                        
                        MaterialId = dto.MaterialId,                        
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor,
                        Linea = 0,
                    };
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var DesInv = await db.ORTDevoluciones
                                .Where(x => x.DevolucionId == dto.DevolucionId)
                                .FirstOrDefaultAsync();
                            //DesInv.Lineas += 1;

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

                            reg.ProveedorId = dto.ProveedorId; //*
                            reg.PacienteId = dto.PacienteId; //*
                            reg.HojaGastoId = dto.HojaGastoId;
                            reg.MaterialId = dto.MaterialId;
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

        public async Task<ActionResult> JsViewMaster(int? id)                               // GET 
        {
            DevolucionDTOViewMaster dto;
            var reg = await db.ORTDevoluciones
                .Include(i => i.Proveedor)
                //.Include(i => i.OrdenCompra)
                //.Include("OrdenCompra.SolicitudPedido")
                .Where(x => x.DevolucionId == id)
                .FirstOrDefaultAsync();

            if (reg == null)            
                return PartialView("_RegisterNotFound");
            
            else
            {
                dto = new DevolucionDTOViewMaster()
                {
                    DevolucionId = reg.DevolucionId,
                    Fecha = reg.Fecha,
                    ProveedorId = reg.ProveedorId,
                    Proveedor = reg.Proveedor,
                    Observacion = reg.Observacion,
                };

                dto.Detalle = new List<MovimientoDTOBase>();

                var regs = await db.ORTMovimientos.Where(x => x.DevolucionId == id && x.Tipo == "DEV")
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

        public async Task<ActionResult> CEditMaster(int id, string Mode = "INS")            // GET 
        {
            var reg = await db.ORTDevoluciones
                .Include(i => i.Proveedor)
                .Where(x => x.DevolucionId == id )
                .FirstOrDefaultAsync();

            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new DevolucionDTOCEdit()
            {
                DevolucionId = reg.DevolucionId,
                Fecha = reg.Fecha,
                Observacion = reg.Observacion,
                ProveedorId = reg.ProveedorId,
                ProveedorNit = reg.Proveedor.Nit,
                ProveedorNombre = reg.Proveedor.Descripcion
            };
            ViewBag.Mode = Mode;
            ViewBag.ProveedorId = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = dto.ProveedorNit, Value = dto.ProveedorId.ToString() }
                }, "Value", "Text");

            ViewBag.Permissions = Utilidades.GetCans(userId);
            return View(dto);
        }
        public async Task<ActionResult> JsCEditGrid(int? id, string mode)                   // GET 
        {
            if (id == null)
            { return null; }
            var dto = await db.ORTMovimientos
                .Where(x => x.Tipo == "DEV" && x.DevolucionId == id)
                .Include(i => i.Material)
                .Select(s => new DevolucionDTOCEditGrid()
                {
                    MovimientoId = s.MovimientoId,
                    SolicitudPedidoId = s.SolicitudPedidoId,
                    Material = s.Material,
                    Cantidad = s.Cantidad,
                    Valor = s.Valor,
                    Precio = s.Precio,
                    HojaGastoId = s.HojaGastoId ?? 0,
                    HGFormulario = s.HojaGasto.HojaControlSala
                })
                .ToListAsync();
            ViewBag.Mode = mode;
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Permissions = Utilidades.GetCans(userId);
            return PartialView("_CEditGrid", dto);
        }

        public async Task<ActionResult> JsCEditChild(int? id)                               // GET 
        {
            var dto = new DevolucionDTOCEditChild();
            if (id == null)
            {
                dto.DevolucionId = 0;
                ViewBag.HojaGastoId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                var reg = await db.ORTMovimientos
                    .Where(x => x.MovimientoId == id)
                    .Include(i => i.HojaGasto)
                    .Include(i => i.Material)
                    .Include("HojaGasto.Paciente")
                    //.Include(i => i.Proveedor)
                    //.Include(i => i.OrdenCompra)
                    //.Include(i => i.Cirujano)
                    .FirstOrDefaultAsync();

                //dto.PacienteRM = reg.Paciente.RefPac_NumHCAntiguo.ToString();
                //dto.PacienteNombreCompleto = reg.Paciente.Nombre;
                //dto.PacienteId = reg.PacienteId;

                //dto.BodegaId = reg.BodegaId ?? 0;
                //dto.BodegaNombre = reg.Bodega.Nombre;
                //dto.BodegaDescripcion = reg.Bodega.Descripcion;

                //dto.ProveedorId = reg.Proveedor.ProveedorId;
                //dto.ProveedorNit = reg.Proveedor.Nit;
                //dto.ProveedorNombre = reg.Proveedor.Descripcion;

                //dto.NumeroOC = reg.OrdenCompra.Numero;
                //dto.AnioOC = reg.OrdenCompra.Anio;
                //dto.FechaOC = reg.OrdenCompra.Fecha;

                //dto.CirujanoId = reg.CirujanoId;
                //dto.CirujanoNombre = reg.Cirujano.Nombre;

                dto.HGFormulario = reg.HojaGasto.HojaControlSala;
                dto.HGFechaFormulario = reg.HojaGasto.Fecha;
                dto.HGRegistroMedico = reg.HojaGasto.Paciente.RefPac_NumHCAntiguo.ToString();
                dto.HGPaciente = reg.HojaGasto.Paciente.Nombre;

                //dto.Fecha = reg.Fecha;
                dto.MaterialCodigo = reg.Material.Codigo;
                dto.MaterialNombre = reg.Material.Descripcion;
                //dto.MaterialPrecio = reg.Material.Precio;
                //dto.UnidadMedida = reg.Material.UnidadMedida;
                dto.Cantidad = reg.Cantidad;
                dto.Precio = reg.Precio;
                dto.Valor = reg.Valor;

                ViewBag.MaterialId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.MaterialCodigo, Value = dto.MaterialId.ToString() }
                    }, "Value", "Text");
                //ViewBag.OrdenCompraId = new SelectList(
                //    new List<SelectListItem>
                //    {
                //         new SelectListItem { Selected = true, Text = dto.NumeroOC.ToString(), Value = dto.OrdenCompraId.ToString() }
                //    }, "Value", "Text");
                ViewBag.HojaGastoId = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Selected = true, Text = dto.HGFormulario, Value = dto.HojaGastoId.ToString() }
                    }, "Value", "Text");
            }            
            return PartialView("_CEditChild", dto);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteChild(Int64 id)                               // POST 
        {
            string[] permisosRequeridos = { "devolucion.eliminar" };
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
            string[] permisosRequeridos = { "devolucion.eliminar" };
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
                        var reg = await db.ORTDevoluciones
                            .Where(x => x.DevolucionId == id)
                            .FirstOrDefaultAsync();
                        var det = await db.ORTMovimientos
                            .Where(x => x.DevolucionId == id && x.Tipo == "DEV")
                            .ToListAsync();

                        foreach (var item in det)
                        {
                            db.ORTMovimientos.Remove(item);
                        }

                        db.ORTDevoluciones.Remove(reg);

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

        public async Task<ActionResult> Print(Int64 id)                                     // 
        {
            var dto = await OrtopediaRep.GetImpresionDevolucion(id); 

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Ortopedia/rptDevolucionProveedor.rpt")));
            rd.SetDataSource(dto);
            // Assign Paramters 
            //rd.SetParameterValue("AFec", afec);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);

            rd.Dispose();
            rd.Close();
            // opcion 1
            return new FileStreamResult(str, "application/pdf");
            // opcion 2
            //string saveFileName = string.Format("Solicitud_Pedido_{0}_{1}", id, DateTime.Now.ToString("yyyyMMddHHmmss"));
            //return File(str, "application/pdf", saveFileName); 
        }

        public JsonResult JsAddFromHojaGasto(Int64 HojaGastoId, Int64 DevolucionId, int ProveedorId, DateTime Fecha)
        {
            var res = OrtopediaRep.AddHojaGastoToDevolucion(HojaGastoId, DevolucionId, ProveedorId, Fecha);
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