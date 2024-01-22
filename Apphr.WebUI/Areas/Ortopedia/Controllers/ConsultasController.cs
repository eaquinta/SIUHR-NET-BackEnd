using Apphr.Application.Ortopedia.Consultas.DTOs;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Excel.Ortopedia;
using Apphr.WebUI.Models.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Ortopedia.Controllers
{
    [Authorize]
    [LogAction]
    public class ConsultasController : DbController
    {
        private OrtopediaRepository OrtopediaRepo;

        public ConsultasController()
        {
            OrtopediaRepo = new OrtopediaRepository(db);
        }

        [Can("consulta.ver")]
        public ActionResult Ingresos()
        {
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        //public ActionResult IngresosXLS()
        //{
        //    return View();
        //}
        public void IngresosExcel()
        {
            ExcelDTOConsultaIngresos excel = new ExcelDTOConsultaIngresos();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetMovimientos()));
            Response.AddHeader("content-disposition", "attachment;filename=Consulta_Ingresos.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }

        public List<ORTMovimiento> GetMovimientos()
        {
            return db.ORTMovimientos.Take(10).ToList();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIngresos(int? BodegaId, int? MaterialId, int? ProveedorId, int? TipoExistencia, int? page)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
           
            var rows = (from m in OrtopediaRepo.GetORTMovimientosWithIncludes().Where(x => x.Tipo == "ING")
                        join ocs in OrtopediaRepo.GetOrdenesCompraStatus() on m.OrdenCompraId equals ocs.OrdenCompraId
                        where (m.MaterialId == MaterialId || MaterialId == null) && (m.ProveedorId == ProveedorId || ProveedorId == null)
                        select new ConsultasDTOIngresoGrid
                        {
                            MaterialCodigo = m.Material.Codigo,
                            MaterialNombre = m.Material.Descripcion,
                            MaterialSiges = m.Material.SigesCodigo,
                            NumeroSP = m.SolicitudPedido.Numero,
                            AnioSP = m.SolicitudPedido.Anio,
                            FechaSP = m.SolicitudPedido.Fecha,
                            ProveedorNombre = m.Proveedor.Descripcion,
                            NumeroOC = m.OrdenCompra.Numero,
                            AnioOC = m.OrdenCompra.Anio,
                            FechaOC = m.OrdenCompra.Fecha,
                            TipoEvento = m.SolicitudPedido.TipoEvento,
                            Fecha = m.Fecha,
                            Cantidad = m.Cantidad,
                            Valor = m.Valor,
                            CantidadSolicitada = ocs.Solicitado,
                        }).ToList();
        
            var dto = (PagedList<ConsultasDTOIngresoGrid>)rows.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IngresosGrid", dto);
        }

        [Can("consulta.ver")]
        public ActionResult Egresos()
        {
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterEgresos(int? BodegaId, int? MaterialId, int? ProveedorId, int? TipoExistencia, int? page)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var rows = OrtopediaRepo.GetORTMovimientosWithIncludes()
                .Where(x => x.Tipo == "DES")
                .Where(x => x.ProveedorId == ProveedorId || ProveedorId == null)
                .Where(x => x.ProveedorId == MaterialId || MaterialId == null)
                .Select(s => new ConsultasDTOEgresoGrid
                {
                    MaterialCodigo = s.Material.Codigo,
                    MaterialNombre = s.Material.Descripcion,
                    Fecha = s.Fecha,
                    Cantidad = s.Cantidad,
                    PacienteRM = s.Paciente.RefPac_NumHCAntiguo.ToString(),
                    PacienteNombreCompleto = s.Paciente.Nombre,
                    Valor = s.Valor,
                    CirujanoNombre = s.Cirujano.Nombre,
                    ProveedorNombre = s.Proveedor.Descripcion,
                })
                .ToList();
            var dto = (PagedList<ConsultasDTOEgresoGrid>)rows.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_EgresosGrid", dto);
        }

        [Can("consulta.ver")]
        public ActionResult Saldos()
        {
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.TipoExistencia = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Por Insumo", Value = "1" },
                new SelectListItem(){ Text = "Por Proveedor", Value = "2" }
            };
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JsFilterSaldos(string Buscar, int? BodegaId, int? MaterialId, int? TipoExistencia, int? page)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;            
                        
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Buscar", Buscar));
            foreach (var p in parameters)
            {
                if (p.Value == null)
                    p.Value = DBNull.Value;
            }
            ViewBag.PLROpions = PagedListOptions;
            switch (TipoExistencia)
            {
                case 1:            
                    var rows1 = await db.Database.SqlQuery<ConsultasDTOSaldoGridMaterial>("GetSaldosMaterial @Buscar", parameters.ToArray()).ToListAsync();
                    var dto1 = (PagedList<ConsultasDTOSaldoGridMaterial>)rows1.ToPagedList(pageIndex, pageSize);
                    return PartialView("_SaldosGridMaterial", dto1);
                case 2:                
                    var rows2 = await db.Database.SqlQuery<ConsultasDTOSaldoGridProveedor>("GetSaldosProveedor @Buscar", parameters.ToArray()).ToListAsync();
                    var dto2 = (PagedList<ConsultasDTOSaldoGridProveedor>)rows2.ToPagedList(pageIndex, pageSize);
                    return PartialView("_SaldosGridProveedor", dto2);
                //    case 3: return PartialView("_IndexGridLotes", dto);
                default: return null;

            }            
        }

        [Can("consulta.ver")]
        public ActionResult Devoluciones()
        {
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterDevoluciones(int? BodegaId, int? MaterialId, int? TipoExistencia, int? page)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var rows = db.ORTMovimientos                
                .Include(i => i.Material)
                .Where(x => x.Tipo == "DEV")
                
                .Select(s => new ConsultasDTODevolucionesGrid
                {
                    MaterialNombre =s.Material.Descripcion,
                    Cantidad = s.Cantidad,
                    Valor = s.Valor,

                })
                .ToList();
            var dto = (PagedList<ConsultasDTODevolucionesGrid>)rows.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_DevolucionesGrid", dto);
        }

        [Can("consulta.ver")]
        public ActionResult Movimientos()
        {
            ViewBag.ProveedorId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MaterialId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PacienteId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CirujanoId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ServicioId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterMovimientos(int? BodegaId, int? MaterialId, int? ProveedorId, int? page, int? PacienteId, int? CirujanoId, int? ServicioId)
        {
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var searchTipo = new List<string> { "ING", "AJU", "DES"};
            var rows = db.ORTMovimientos
                .Include(i => i.Material)
                .Include(i => i.Proveedor)
                .Include(i => i.Servicio)
                .Where(x => searchTipo.Contains(x.Tipo))
                .Where(x => x.ProveedorId == ProveedorId || ProveedorId == null)
                .Where(x => x.MaterialId == MaterialId || MaterialId == null)
                .Where(x => x.PacienteId == PacienteId || PacienteId == null)
                .Where(x => x.ServicioId == ServicioId || ServicioId == null)
                .Where(x => x.CirujanoId == CirujanoId || CirujanoId == null)

                .Select(s => new ConsultasDTOMovimientosGrid
                {
                    Tipo =s.Tipo,
                    MaterialCodigo = s.Material.Codigo,
                    MaterialNombre = s.Material.Descripcion,
                    Cantidad = s.Cantidad,
                    Valor = s.Valor,
                    ProveedorNombre = s.Proveedor.Descripcion,
                    PacienteRM = s.Paciente.RefPac_NumHCAntiguo.ToString(),
                    PacienteNombreCompleto = s.Paciente.Nombre,
                    CirujanoNombre = s.Cirujano.Nombre,
                    ServicioNombre = s.Servicio.Nombre
                })
                .ToList();
            var dto = (PagedList<ConsultasDTOMovimientosGrid>)rows.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_MovimientosGrid", dto);
        }
    }
}
