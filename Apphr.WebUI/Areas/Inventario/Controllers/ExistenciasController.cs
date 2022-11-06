using Apphr.Application.ExistenciasBodega.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class ExistenciasController : DbController
    {
        [AppAuthorization(Permit.View)]
        public ActionResult Index() // GET
        {
            ViewBag.TipoExistencia = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Por Bodega", Value = "1" },
                new SelectListItem(){ Text = "Por Proveedor", Value = "2" },
                new SelectListItem(){ Text = "Por Lote/Fecha Vencimiento", Value = "3" },
            };
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(int? BodegaId, int? MaterialId, int? TipoExistencia, int? page)
        {
            string query = "";
            //List<ExistenciaBodegaDTOIndex> rows = new List<ExistenciaBodegaDTOIndex>();
            switch (TipoExistencia)
            {
                case 1:
                    //Ref. Q001
                    query = @"
                        SELECT
	                        eb.BodegaId, 
	                        ( SELECT Nombre FROM Bodegas WHERE BodegaId = eb.BodegaId ) AS BodegaNombre, 
	                        eb.MaterialId, 
	                        ( SELECT Descripcion FROM Materiales WHERE MaterialId = eb.MaterialId ) AS MaterialNombre, 
	                        SUM(eb.Valor) AS Valor, 
	                        SUM(eb.Cantidad) AS Cantidad
                        FROM
	                        dbo.ExistenciasBodega AS eb
                        WHERE
	                        eb.BodegaId = @BodegaId AND
                            (eb.MaterialId = @MaterialId OR @MaterialId IS NULL)
                        GROUP BY
	                        eb.MaterialId, 
	                        eb.BodegaId";                    
                    break;
                case 2:
                    //Ref. Q002
                    query = @"
                        SELECT
	                        eb.BodegaId,
	                        ( SELECT Nombre FROM Bodegas WHERE BodegaId = eb.BodegaId ) AS BodegaNombre,
	                        eb.MaterialId,
	                        ( SELECT Descripcion FROM Materiales WHERE MaterialId = eb.MaterialId ) AS MaterialNombre,
	                        eb.ProveedorId,
	                        ( SELECT Nit FROM Proveedores WHERE ProveedorId = eb.ProveedorId) AS ProveedorNit,
	                        ( SELECT Descripcion FROM Proveedores WHERE ProveedorId = eb.ProveedorId) AS ProveedorDescipcion,
	                        SUM ( eb.Valor ) AS Valor,
	                        SUM ( eb.Cantidad ) AS Cantidad 
                        FROM
	                        dbo.ExistenciasBodega AS eb 
                        WHERE
	                        eb.BodegaId = @BodegaId 
	                        AND ( eb.MaterialId = @MaterialId OR @MaterialId IS NULL ) 
                        GROUP BY
	                        eb.MaterialId,
	                        eb.BodegaId,
	                        eb.ProveedorId;";
                    break;
                case 3:
                    //Ref. Q002
                    query = @"
                        SELECT
	                        eb.BodegaId,
	                        ( SELECT Nombre FROM Bodegas WHERE BodegaId = eb.BodegaId ) AS BodegaNombre,
	                        eb.MaterialId,
	                        ( SELECT Descripcion FROM Materiales WHERE MaterialId = eb.MaterialId ) AS MaterialNombre,
	                        eb.Lote, eb.FechaVencimiento,
	                        SUM ( eb.Valor ) AS Valor,
	                        SUM ( eb.Cantidad ) AS Cantidad 
                        FROM
	                        dbo.ExistenciasBodega AS eb 
                        WHERE
	                        eb.BodegaId = @BodegaId 
	                        AND ( eb.MaterialId = @MaterialId OR @MaterialId IS NULL ) 
                        GROUP BY
	                        eb.MaterialId,
	                        eb.BodegaId,
	                        eb.Lote,
	                        eb.FechaVencimiento;";
                    break;
            }
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@BodegaId", (object)BodegaId ?? DBNull.Value));
            p.Add(new SqlParameter("@MaterialId", (object)MaterialId ?? DBNull.Value));

            var rows = db.Database
                .SqlQuery<ExistenciaBodegaDTOIndex>(query, p.ToArray())
                .ToList();

            var dto = (PagedList<ExistenciaBodegaDTOIndex>)rows.ToPagedList(pageIndex, pageSize);

            ViewBag.PLROpions = PagedListOptions;

            switch (TipoExistencia)
            {
                case 1: return PartialView("_IndexGridGeneral", dto);                   
                case 2: return PartialView("_IndexGridProveedor", dto);
                case 3: return PartialView("_IndexGridLotes", dto);
                default: return null;

            }            
        }
    }
}