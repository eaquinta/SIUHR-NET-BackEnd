using Apphr.Application.Kardex.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Services;
using CrystalDecisions.CrystalReports.Engine;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class KardexController : DbController
    {
        [Can("kardex.ver")]
        public ActionResult Index() // GET
        {            
            return View();
        }

        public ActionResult Cierre() // GET
        {
            return View();
        }

        public ActionResult Print(int? BodegaId, int? MaterialId) // GET
        {
            if (BodegaId == null)
            {
                return null;
            }
            // Data Division
            List<KardexDTOBase> rows = new List<KardexDTOBase>();
            var dfec = DateTime.Now;
            dfec = dfec.Date.AddDays(1 - dfec.Day);
            var afec = DateTime.Now;


            //decimal CantidadAcumulado = 0;
            //decimal ValorAcumulado = 0;
            var regs = db.MovimientosInvnentario
                .Include("Bodega")
                .Include("Paciente")
                .Include("Paciente.Persona")
                .Include("Material")
                .Include("Proveedor")
                .Include(x => x.TipoMovimientoInventario)
                .Include(x => x.Destino)
                .Where(x => x.BodegaId == BodegaId && DbFunctions.TruncateTime(x.Fecha) >= dfec.Date && DbFunctions.TruncateTime(x.Fecha) <= afec.Date  && (x.MaterialId == MaterialId || MaterialId == null)) 
                //.OrderBy(x => x.Material.Codigo)
                .ToList();

            var KdxRepository = new KardexRepository();
            var rowsDetalle = KdxRepository.GetCRptKardex(KdxRepository.GetKardexReportModel(BodegaId.Value, regs));
            var rowsResumen = GetResumenMovimientosInventario(regs);
            var Bod = db.Bodegas.Find(BodegaId);


            //Report Division
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptKardex.rpt")));
            //rd.SetDataSource(rows);
            rd.Database.Tables[0].SetDataSource(rowsDetalle);
            rd.Subreports[0].Database.Tables[0].SetDataSource(rowsResumen);
            rd.SetParameterValue("BodegaNombre",Bod.Nombre);
            rd.SetParameterValue("BodegaDescripcion",Bod.Descripcion);

            //rd.Subreports[1].Database.Tables[0].SetDataSource(rowsSaldoAnterior);
            //rd.Subreports[2].Database.Tables[0].SetDataSource(rowsSaldoActual);

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




        #region Js
        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(int? BodegaId, int? MaterialId, int? page)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<KardexDTOBase> rows = new List<KardexDTOBase>();
            var regs = db.MovimientosInvnentario
                .Include("Bodega")
                .Include("Material")
                .Include("Proveedor")
                .Include(x => x.TipoMovimientoInventario)
                .Where(x => x.MaterialId == MaterialId && x.BodegaId == BodegaId)
                .OrderBy(x => x.Fecha)
                .ThenBy(x => x.Documento)
                .ThenBy(x => x.Line)
                .ToList();

            decimal CantidadAcumulado = 0;
            decimal ValorAcumulado = 0;
            // recuperar saldo anterior
            DateTime now = DateTime.Now;
            now = now.AddDays(-now.Day);
            KardexDTOKardexMaterial km = new KardexDTOKardexMaterial();
            km.SaldoAnterior = new KardexDTOSaldo()
            {
                Cantidad = 0,
                Valor = 0
            };

            var existencias = db.CierresInventario.Where(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId && DbFunctions.TruncateTime(x.Fecha) == now.Date).FirstOrDefault();
            if (existencias != null)
            {
                CantidadAcumulado = existencias.Cantidad;
                ValorAcumulado = existencias.Valor;
            }

            km.SaldoAnterior = new KardexDTOSaldo()
            {
                Cantidad = CantidadAcumulado,
                Valor = ValorAcumulado
            };

            if (!regs.Any())
            {

            }


            foreach (var item in regs)
            {
                CantidadAcumulado += (item.Cantidad * item.Efecto);
                ValorAcumulado += (item.Valor * item.Efecto);

                rows.Add(new KardexDTOBase()
                {
                    Fecha = item.Fecha,
                    Cantidad = item.Cantidad,
                    Valor = item.Valor,
                    Documento = item.Documento,
                    DocumentoReferencia = item.DocumentoReferencia,
                    Line = item.Line,
                    TipoDocumento = item.TipoMovimientoInventario.NombreCorto,
                    CantidadAcumulado = CantidadAcumulado,
                    ValorAcumulado = ValorAcumulado,
                    CostoUnitario = item.Precio
                });
            }
            km.Detalle = rows;
            km.SaldoActual = new KardexDTOSaldo()
            {
                Cantidad = CantidadAcumulado,
                Valor = ValorAcumulado
            };
            km.Resumen = GetResumenMovimientosInventario(regs);


            var dto = (PagedList<KardexDTOBase>)rows.ToPagedList(pageIndex, pageSize);
            return PartialView("_IndexGrid", km);
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsCierre(int? BodegaId)
        {
            var Fecha = DateTime.Now;
            Fecha = Fecha.Date.AddDays(-Fecha.Day);
            try
            {
                if (BodegaId == null)
                {
                    throw new ArgumentException("No se ha proporcionado un codigo de Bodega Válido");
                }
                if (db.CierresInventario.Any(x => x.BodegaId == BodegaId && DbFunctions.TruncateTime(x.Fecha) == Fecha))
                {
                    throw new ArgumentException("Este cierre ya se ha realizado");
                }
                
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@BodegaId", BodegaId));
                parameters.Add(new SqlParameter("@Fecha", Fecha.Date));

                string query = @"
                    INSERT INTO CierresInventario (
                        [BodegaId], 
                        [MaterialId], 
                        [Fecha],
                        [Cantidad],
                        [Valor])
                    (SELECT
	                    MovimientosInvnetario.BodegaId, 
	                    MovimientosInvnetario.MaterialId, 
	                    @Fecha AS Fecha,
	                    SUM(Cantidad*Efecto) AS Cantidad,
	                    SUM(Valor*Efecto) AS Valor
                    FROM
	                    dbo.MovimientosInvnetario
                    WHERE
	                    CAST(MovimientosInvnetario.Fecha AS DATE) <= @Fecha AND
	                    BodegaId = 12
                    GROUP BY
	                    MovimientosInvnetario.BodegaId, 
	                    MovimientosInvnetario.MaterialId)";
                db.Database.ExecuteSqlCommand(query,parameters.ToArray());
                return Json(new { success = true, message = "Esta correcto" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Ha ocurrido un prblema con el cierre" }, JsonRequestBehavior.DenyGet);
            }
            
            
           
        }
        #endregion
        #region Private
        private KardexDTOSaldo GetSaldoAnterior(int BodegaId, int MaterialId)
        {
            DateTime now = DateTime.Now;
            now = now.AddDays(-now.Day);
            var existencias = db.CierresInventario.Where(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId && DbFunctions.TruncateTime(x.Fecha) == now.Date).FirstOrDefault();
            if (existencias != null)
            {
                return new KardexDTOSaldo() {
                    MaterialId = MaterialId,
                    Cantidad = existencias.Cantidad,
                    Valor = existencias.Valor
                };                
            }
            else
            {
                return new KardexDTOSaldo()
                {
                    MaterialId = MaterialId,
                    Cantidad = 0,
                    Valor = 0
                };
            }
        }
        private List<KardexDTOResumenMovimiento> GetResumenMovimientosInventario(List<MovimientoInventario> regs)
        {
            var Resumen  = regs.GroupBy(x => new { x.Efecto, x.MaterialId })
               .Select(x => new KardexDTOResumenMovimiento()
               {
                   TipoMovimiento = (x.Key.Efecto == 1) ? "Ingresos Totales" : "Egresos Totales",
                   MaterialId = x.Key.MaterialId,
                   CantidadDocumentos = x.Count(),
                   Cantidad = x.Sum(y => y.Cantidad),
                   Valor = x.Sum(y => y.Valor)
               })
               .ToList();
            Resumen.AddRange(regs.GroupBy(x => new { x.TipoMovimientoInventario, x.MaterialId })
               .Select(x => new KardexDTOResumenMovimiento()
               {
                   TipoMovimiento = x.Key.TipoMovimientoInventario.NombreCorto,
                   MaterialId = x.Key.MaterialId,
                   CantidadDocumentos = x.Count(),
                   Cantidad = x.Sum(y => y.Cantidad),
                   Valor = x.Sum(y => y.Valor)
               })
               .ToList());
            return Resumen;
        }

        #endregion
    }
}