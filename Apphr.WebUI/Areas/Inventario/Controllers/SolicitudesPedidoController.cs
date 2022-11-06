using Apphr.Application.Common;
using Apphr.Application.Common.Models;
using Apphr.Application.SolicitudesPedido.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models;
using CrystalDecisions.CrystalReports.Engine;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class SolicitudesPedidoController : DbController
    {
        [AppAuthorization(Permit.View)]
        public ActionResult Index(SolicitudPedidoDTOIndex dto, int? page) //GET
        {
            IQueryable<SolicitudPedido> regs;

            int pageIndex = 1;
            if (dto?.F == null) dto.F = new IxFilter();
            if (dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //            BodegaDTOIndex dto = new BodegaDTOIndex();


            regs = (from p in db.SolicitudesPedido.Include("Departamento") select p);
            if (dto.F != null)
            {
                if (!string.IsNullOrEmpty(dto.F.Buscar))
                    regs = regs.Where(x => x.SolicitudPedidoId.Contains(dto.F.Buscar) ||
                    (DbFunctions.Right("0"+SqlFunctions.DatePart("d",x.Fecha),2)+"/"+DbFunctions.Right("0"+SqlFunctions.DatePart("m",x.Fecha),2)+"/"+SqlFunctions.DatePart("yyyy", x.Fecha)).Contains(dto.F.Buscar) ||
                    x.Departamento.Descripcion.Contains(dto.F.Buscar));
                    //(x.Fecha.HasValue ? x.Fecha.Value.ToString("d") : "").Contains(dto.F.Buscar));
            }

            regs = regs.OrderBy(x => x.SolicitudPedidoId);

            var rows = mapper.Map<List<SolicitudPedidoDTOBase>>(regs.ToList());
            dto.Result = (PagedList<SolicitudPedidoDTOBase>)rows.ToPagedList(pageIndex, pageSize);
            return View(dto);
        }

        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await this.DetailsData(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            var dto = mapper.Map<SolicitudPedidoDTODetails>(reg);

            return View(dto);
        }

        [AppAuthorization(Permit.Edit)]
        public async Task<ActionResult> CEdit(string id)  //GET
        {
            var dto = new SolicitudPedidoDTOCEdit();
            if (!string.IsNullOrEmpty(id))
            { 
                // Update
                var reg = await db.SolicitudesPedido
                     .Include("Departamento")
                     .Include("Seccion")
                     .Where(x => x.SolicitudPedidoId == id).FirstOrDefaultAsync();
                if (reg == null)
                {
                    return HttpNotFound();
                }

                dto = mapper.Map<SolicitudPedidoDTOCEdit>(reg);
                // Mapeo especial
                dto.DepartamentoCodigo = reg.Departamento.Codigo;
                dto.DepartamentoNombre = reg.Departamento.Descripcion;
                dto.SeccionCodigo = reg.Seccion.Codigo;
                dto.SeccionNombre = reg.Seccion.Descripcion;
                dto.serie = dto.SolicitudPedidoId.Substring(5, 6);
                dto.year = dto.SolicitudPedidoId.Substring(0, 4);
                
                dto.Child.SolicitudId = dto.SolicitudPedidoId;
                ViewBag.mode = "UPD";
            }
            else
            { 
                // Insert
                dto.Correlativo = Correlativo.Get();
                dto.Fecha = DateTime.Now;
                dto.year = DateTime.Now.Year.ToString();
                dto.Director = AppDefaults.GetValue("Director");
                dto.Gerente = AppDefaults.GetValue("Gerente");
                ViewBag.mode = "INS";
            }

            ViewBag.ListTipo = PrioridadTipo.GetSelectList();


            return View(dto);
        }

        [AppAuthorization(Permit.View)]
        public ActionResult IndexDBF(SolicitudPedidoDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
        {            
            int pageIndex = 1;
            if (dto?.F == null) dto.F = new IxFilter();
            if ( dto.F.Buscar != dto.F._Buscar)
            {
                page = 1;
                dto.F._Buscar = dto.F.Buscar;
            }           
           
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var regs = this.dbfContext.GetSolicitudesPedido();
           
            if (!String.IsNullOrEmpty(dto?.F?.Buscar))
            {
                regs = regs.Where(s => 
                s.NUMSOL.Contains(dto?.F?.Buscar) || 
                s.DEPSOL.ToUpper().Contains(dto?.F?.Buscar.ToUpper()) ||                
                (s.Fecha.HasValue ? s.Fecha.Value.ToString("d") :"").Contains(dto?.F?.Buscar)
                );
            }
            
            regs = regs.ToList();
            dto.Result = regs.ToPagedList(pageIndex, pageSize);
            return View(dto);
        }

        [AppAuthorization(Permit.View)]
        public ActionResult DetailsDBF(string id) // GET
        {
            id = FixFormatId(id);            

            SolicitudPedidoDBF dte = this.dbfContext.GetSolicitudPedido(id);
            List<SolicitudPedidoDetalleDBF> dtd = this.dbfContext.GetSolicitudPedidoDetalle(id);
            List<MaterialDBF> mat = this.dbfContext.GetMateriales();


            SolicitudPedidoDTODetailsDBF reg = new SolicitudPedidoDTODetailsDBF()
            {
                Solicitud = dte.NUMSOL,
                Correlativo = dte.CORSOL,
                DIASOL = dte.DIASOL,
                MESSOL = dte.MESSOL,
                ANOSOL = dte.ANOSOL,
                Departamento = dte.DEPSOL,
                Observaciones = dte.OBSSOL,
                Solicito = dte.SOLSOL,
                Elaboro = dte.OTRSOL,
                Jefe = dte.JEFSOL,
                Gerente = dte.GERSOL,
                Director = dte.DIRSOL,
                Tipo = dte.TIPSOL,            
            };

            var joined = from d in dtd
                         join m in mat on d.MATSOL equals m.CODIGO
                         select new { d, m };

            List<SolicitudPedidoDTODetailsL2DBF> regs = new List<SolicitudPedidoDTODetailsL2DBF>();

            foreach (var j in joined)
            {
                regs.Add(new SolicitudPedidoDTODetailsL2DBF()
                {
                    Material = j.d.MATSOL,
                    Descripcion = j.m.DESCRI,
                    Cantidad = j.d.CANSOL,
                    Valor = j.d.VALSOL
                });
            }

            reg.Detalle = regs;
            return View(reg);
        }
        public ActionResult Print(string id) // GET
        {
            id = FixFormatId(id);

            List<SolicitudPedidoDTORpt> regsReport = new List<SolicitudPedidoDTORpt>();
            List<SolicitudPedidoDBF> def = new List<SolicitudPedidoDBF>();
            def.Add(this.dbfContext.GetSolicitudPedido(id));
            List<DestinoDBF> des = this.dbfContext.GetDestinos();
            List<MaterialDBF> mat = this.dbfContext.GetMateriales();
            List<SolicitudPedidoDetalleDBF> det = this.dbfContext.GetSolicitudPedidoDetalle(id);

            var joined = from df in def
                         join dt in det on df.NUMSOL equals dt.NUMSOL 
                         join ds in des on df.CPROVE equals ds.CODIGO
                         join dm in mat on new { A = dt.MATSOL, B=dt.CODIGI } equals new { A = dm.CODIGO, B = dm.CODIGI }
                         select new { df, dt, ds, dm};

            foreach (var j in joined)
            {
                regsReport.Add(new SolicitudPedidoDTORpt()
                {
                    SolicitudNumero = j.df.NUMSOL,
                    Elaboro = j.df.OTRSOL,
                    Jefe = j.df.JEFSOL,
                    Gerente = j.df.GERSOL,
                    Director = j.df.DIRSOL,
                    Cantidad = Convert.ToDecimal(j.dt.CANSOL),
                    UnidadMedida = j.dt.UMESOL,
                    Valor = Convert.ToDecimal(j.dt.VALSOL),
                    Fecha = j.df.Fecha ?? DateTime.MinValue,
                    Descripcion = j.dm.DESCRI,
                    MaterialCodigo = j.dt.MATSOL,
                    SigesCodigo = "" ?? "",                   
                    //        RenglonCodigo = dme.dm.m.RenglonCodigo,
                    Observaciones = j.df.OBSSOL,
                    CPROVE = j.df.CPROVE,
                    DestinoDescripcion = j.ds.DESCRI,

                });
            }

            
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptSolicitudPedido.rpt")));
            rd.SetDataSource(regsReport);
                
            
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

        [AppAuthorization(Permit.Delete)]
        public async Task<ActionResult> Delete(string id) //GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await this.DetailsData(id);
            if (reg == null)
            {
                return HttpNotFound();
            }

            var dto = mapper.Map<SolicitudPedidoDTODetails>(reg);
            return View(dto);
        }
        [AppAuthorization(Permit.Delete)]
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id) // POST
        {
            var reg = await db.SolicitudesPedido.FindAsync(id);
            db.SolicitudesPedido.Remove(reg);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { Toast = "success.delete" });
        }

        #region Js

        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult JsSaveMaster(SolicitudPedidoDTOCEdit dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
                }
                if (string.IsNullOrEmpty(dto.SolicitudPedidoId))
                {
                    dto.SolicitudPedidoId = dto.year + "-" + dto.serie.PadLeft(6, '0');
                    var reg = new SolicitudPedido()
                    {
                        SolicitudPedidoId = dto.SolicitudPedidoId,
                        Correlativo = dto.Correlativo,
                        Fecha = dto.Fecha,
                        Tipo = dto.Tipo,
                        DepartamentoId = dto.DepartamentoId,
                        SeccionId = dto.SeccionId,
                        Elaboro = dto.Elaboro,
                        Solicito = dto.Solicito,
                        JefeDepartamento = dto.JefeDepartamento,
                        Gerente = dto.Gerente,
                        Director = dto.Director,
                        Observaciones = dto.Observaciones
                    };
                    db.SolicitudesPedido.Add(reg);
                    db.SaveChanges();
                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg });
                }
                else
                {
                    var reg = db.SolicitudesPedido.Find(dto.SolicitudPedidoId);
                    reg.Correlativo = dto.Correlativo;
                    reg.Fecha = dto.Fecha;
                    reg.Tipo = dto.Tipo;
                    reg.DepartamentoId = dto.DepartamentoId;
                    reg.SeccionId = dto.SeccionId;
                    reg.Elaboro = dto.Elaboro;
                    reg.Solicito = dto.Solicito;
                    reg.JefeDepartamento = dto.JefeDepartamento;
                    reg.Gerente = dto.Gerente;
                    reg.Director = dto.Director;
                    reg.Observaciones = dto.Observaciones;
                    db.SaveChanges();
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException });
            }
        }



        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult JsSaveChild(SolicitudPedidoDTOBaseDT dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
                }
                if (dto.SolicitudPedidoDTId == 0)
                {
                    //var reg = mapper.Map<SolicitudPedidoDT>(dto);
                    var reg = new SolicitudPedidoDetalle()
                    {
                        SolicitudId = dto.SolicitudId,
                        MaterialId = dto.MaterialId,
                        Cantidad = dto.Cantidad,
                        Precio = dto.Precio,
                        Valor = dto.Valor ?? 0
                    };
                    reg.Material = null;
                    db.SolicitudesPedidoDT.Add(reg);
                    db.SaveChanges();
                    return Json(new { success = true, message = Resources.Msg.success_create });
                }
                else
                {
                    var reg = db.SolicitudesPedidoDT.Find(dto.SolicitudPedidoDTId);
                    reg.SolicitudId = dto.SolicitudId;
                    reg.MaterialId = dto.MaterialId;
                    reg.Cantidad = dto.Cantidad;
                    reg.Precio = dto.Precio;
                    reg.Valor = dto.Valor ?? 0;
                    db.SaveChanges();
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
                var reg = await db.SolicitudesPedido
                .Include("Detalle")
                .Where(x => x.SolicitudPedidoId == id).FirstOrDefaultAsync();               
                db.SolicitudesPedido.Remove(reg);
                await db.SaveChangesAsync();
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure,  exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }


        [ValidateAntiForgeryToken]
        public ActionResult JsDeleteChild(int? id)
        {
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var reg = db.SolicitudesPedidoDT.Find(id);
                db.SolicitudesPedidoDT.Remove(reg);
                db.SaveChanges();
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }


        public ActionResult JsGrid(string id)
        {
            if (string.IsNullOrEmpty(id))
            { return null; }
            var regs = db.SolicitudesPedidoDT.Include("Material").Where(x => x.SolicitudId == id).ToList();
            var dto = mapper.Map<IEnumerable<SolicitudPedidoDTOBaseDT>>(regs);
            return PartialView("_Grid", dto);
        }




        [HttpPost]
        public JsonResult IsSolicitudPedidoIdAvailable(string serie, string year, string SolicitudPedidoId)
        {
            if (string.IsNullOrEmpty(SolicitudPedidoId))
            {
                serie = year + "-" + serie.PadLeft(6, '0');
                return Json(!db.SolicitudesPedido.Any(u => u.SolicitudPedidoId == serie));
            }
            else
            {
                return Json(true);
            }
        }
        

        public JsonResult JsGetChild(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message ="Solicitud mal formada!" }, JsonRequestBehavior.AllowGet);
            }
            var reg = db.SolicitudesPedidoDT
                .Where(x => x.SolicitudPedidoDTId == id)
                .Include("Material").FirstOrDefault();
            if (reg == null)
            {
                return Json(new { success = false, message = "Registro no localizado!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = reg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private
        private async Task<SolicitudPedido> DetailsData(string id)
        {
            return await db.SolicitudesPedido
                .Where(x => x.SolicitudPedidoId == id)
                .Include("Detalle")
                .Include("Detalle.Material")
                .Include("Seccion")
                .Include("Departamento")
                .FirstOrDefaultAsync();
        }
        private string GetCorrelativo()
        {
            string Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            return Usuario.ToUpper().Substring(0, 3) + " " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        }
        private string FixFormatId(string id)
        {
            return id.PadLeft(4);
        }        
        #endregion
    }
}