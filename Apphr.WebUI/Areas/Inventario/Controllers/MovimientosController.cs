using Apphr.Application.Common.DTOs;
using Apphr.Application.Movimientos.DTOs;
using Apphr.Application.Reports.Inventario;
using Apphr.Domain.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class MovimientosController : DbController
    {
        private ControlAbastecimientoRepository ControlAbastecimientoRep;
        public MovimientosController()
        {
            ControlAbastecimientoRep = new ControlAbastecimientoRepository(db);
        }

        [Can("movimiento.ver")]
        public ActionResult IndexDBF() // GET
        {
            var dto = new MovimientoDTOIxFilter
            {
                DFec = DateTime.Now.Date,
                AFec = DateTime.Now.Date
            };
            ViewBag.CodigoBodega = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CodigoMaterial = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.yearList = ControlAbastecimientoRep.AnioList();
            return View(dto);
        }

        [HttpPost]
        public JsonResult JsGetDataTableDBF(DTOJqueryDatatableParam param, int anio, string CodigoMaterial, string CodigoBodega, DateTime? DFec, DateTime? AFec)                     // GET 
        {
            dbfContext.SetYear(anio);

            var draw = param.draw;
            var start = param.start;
            var length = param.length;
            var sortColumnName = param.columns[param.order[0].column].name;
            var sortColumnDir = param.order[0].dir;
            var searchValue = param.search.value;
            int pageSize = param.length;
            int skip = param.start;
            int recordsTotal = 0;
            List<DTOMovimientosInventarioDBF> Data = new List<DTOMovimientosInventarioDBF>();

            try
            {                
                CodigoMaterial = (string.IsNullOrEmpty(CodigoMaterial)) ? null : CodigoMaterial;
                CodigoBodega = (string.IsNullOrEmpty(CodigoBodega)) ? null : CodigoBodega;                
                var regs = dbfContext.GetMovimientos(CodigoMaterial, CodigoBodega, DFec, AFec, searchValue, sortColumnName, sortColumnDir);
                recordsTotal = regs.Count();
                if (pageSize != -1)
                    Data = regs.Skip(skip).Take(pageSize).ToList();
                else
                    Data = regs.ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { draw = draw, data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }


        //[ValidateAntiForgeryToken]
        //public ActionResult JsFilterIndexDBF(string Buscar, int Year, string CodigoMaterial, string CodigoBodega, DateTime? DFec, DateTime? AFec, int? page) // GET
        //{
        //    int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    dbfContext.SetYear(Year);
        //    var regs = dbfContext.GetMovimientos(CodigoMaterial, CodigoBodega, DFec, AFec);
        //    if (regs == null)
        //        return PartialView("_ErrorSiahr");

        //    if (!String.IsNullOrEmpty(Buscar))
        //    {
        //        regs = regs.Where(s =>
        //        s.CODIGOMATERIAL.Contains(Buscar) ||
        //        //s.NIT.ToUpper().Contains(Buscar.ToUpper()) ||
        //        (s.Fecha.HasValue ? s.Fecha.Value.ToString("d") : "").Contains(Buscar)
        //        );
        //    }

        //    regs = regs.ToList();
        //    var dto = regs.ToPagedList(pageIndex, pageSize);
        //    ViewBag.PLROpions = PagedListOptions;
        //    ViewBag.Year = Year;
        //    return PartialView("_IndexDBFGrid", dto);
        //}

        [Can("reporte_existencias.ver")]
        public ActionResult ExistenciasDBF() //int Year)
        {            
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterExistenciasDBF(string Bodega, DateTime AFec, string Material, bool isDetail, int? page) // GET
        {
            //int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IEnumerable<ReporteExistenciasDBF> regs;
            dbfContext.SetYear(AFec.Year);
            if (isDetail)
            {
                if (string.IsNullOrEmpty(Material))
                {
                    regs = dbfContext.GetExistenciasDetalle(Bodega, AFec);
                }
                else
                {
                    regs = dbfContext.GetExistenciasDetalle(Bodega, Material, AFec);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Material))
                {
                    regs = dbfContext.GetExistencias(Bodega, AFec);
                }
                else
                {
                    regs = dbfContext.GetExistencias(Bodega, Material, AFec);
                }
                    
            }

            if (regs == null)
            {
                return PartialView("_ErrorSiahr");
            }

            var dto = regs.ToList();
            //var dto = regs.ToPagedList(pageIndex, pageSize);
            //ViewBag.PLROpions = PagedListOptions;
            //ViewBag.Year = Year;
            if (isDetail)
                return PartialView("_ExistenciasDBFGridDetalle", dto);
            else
                return PartialView("_ExistenciasDBFGrid", dto);
        }

        public ActionResult PrintExistenciasDBF(string bodega, DateTime afec) // GET
        {
            dbfContext.SetYear(afec.Year);
            var regs = dbfContext.GetExistencias(bodega, afec);
            var dto = regs.ToList().Select(x => new ReporteExistencias() {
                Existencia = x.CANMOV,
                CodigoBodega = x.CODBODE,
                UnidadMedida = x.UNIMED,
                CODIGI = x.CODIGI,
                Codigo = x.CODIGO,
                Descripcion = x.DESCRI,
                Total = x.VALMOV,
                CostoUnitario = x.CostoUnitario??0
            }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptReporteExistencias.rpt")));
            rd.SetDataSource(dto);
            // Assign Paramters 
            rd.SetParameterValue("AFec", afec);

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

    }
}