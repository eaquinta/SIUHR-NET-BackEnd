using Apphr.Application.OrdenesCompra.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class OrdenesCompraController : DbController
    {
        private ProveedorRepository ProveedorRep;
        private MaterialRepository MaterialRep;
        private OrdenCompraRepository OrdenCompraRep;
        private ControlAbastecimientoRepository ControlAbastecimientoRep;

        public OrdenesCompraController()
        {
            ProveedorRep = new ProveedorRepository(db);
            MaterialRep = new MaterialRepository(db);
            OrdenCompraRep = new OrdenCompraRepository(db);
            ControlAbastecimientoRep = new ControlAbastecimientoRepository(db);
        }
        [Can("orden_compra.ver")]
        public ActionResult Index() // GET
        {            
            return View();
        }

        [Can("orden_compra.ver")]
        public ActionResult IndexDBF() // GET
        {
            ViewBag.yearList = ControlAbastecimientoRep.AnioList();
            var dto = new OrdenCompraDTOIxFilter() { Year = defaultSiahrYear };
            return View(dto);
        }

        [Can("orden_compra.ver")]
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = db.OrdenesCompra
                .Where(x => x.OrdenCompraId == id)
                .Include(x => x.Proveedor)
                .FirstOrDefault();
            var dto = mapper.Map<OrdenCompraDTODetails>(reg);
            return View(dto);
        }

        //[Can(.".ver")]
        //public ActionResult DetailsDBF(string id, int year)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    id = FixFormatId(id);

        //    this.dbfContext.SetYear(year);

        //    var dte = this.dbfContext.GetOrdenCompra(id);
        //    List<OrdenCompraDetalleDBF> dtd = this.dbfContext.GetOrdenCompraDetalle(id);
        //    List<MaterialDBF> mat = this.dbfContext.GetMateriales();
        //    ProveedorDBF prv = this.dbfContext.GetProveedor(dte.NIT).FirstOrDefault();


        //    var reg = new OrdenCompraDTODetailsDBF()
        //    {
        //        Orden = dte.ORDEN,
        //        Nit = dte.NIT,
        //        ProveedorNombre = prv.DESCRI,
        //        Fecha = dte.Fecha                
        //    };

        //    var joined = from d in dtd
        //                 join m in mat on d.CODMAT equals m.CODIGO
        //                 select new { d, m };

        //    var regs = new List<OrdenCompraDetalleDTODetailsDBF>();

        //    foreach (var j in joined)
        //    {
        //        regs.Add(new OrdenCompraDetalleDTODetailsDBF()
        //        {
        //            Nit = j.d.NIT,
        //            Orden = j.d.ORDEN,
        //            MaterialCodigo = j.d.CODMAT,
        //            Renglon = j.d.RENGL,
        //            MaterialNombre = j.m.DESCRI,
        //            UnidadMedida = j.d.UNIDET,
        //            Cantidad = (decimal)j.d.CANDET,
        //            Precio = (decimal)j.d.PREDET,
        //            Valor = (decimal)j.d.VALDET
        //            // Descipcion = j.d.
        //            //Material = j.d.MATSOL,
        //            //Descripcion = j.m.DESCRI,
        //            //Cantidad = j.d.CANDET,
        //            //Valor = j.d.VALSOL
        //        });
        //    }

        //    reg.Detalle = regs;
        //    return View(reg);
        //}

        public ActionResult JsDetailsDBF(string id, int year) // GET
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = FixFormatId(id);

            this.dbfContext.SetYear(year);

            var dte = this.dbfContext.GetOrdenCompra(id);
            List<OrdenCompraDetalleDBF> dtd = this.dbfContext.GetOrdenCompraDetalle(id);
            List<MaterialDBF> mat = this.dbfContext.GetMateriales();
            ProveedorDBF prv = this.dbfContext.GetProveedor(dte.NIT).FirstOrDefault();


            var reg = new OrdenCompraDTODetailsDBF()
            {
                Orden = dte.ORDEN,
                Nit = dte.NIT,
                ProveedorNombre = prv.DESCRI,
                Fecha = dte.Fecha
            };

            var joined = from d in dtd
                         join m in mat on d.CODMAT equals m.CODIGO
                         select new { d, m };

            var regs = new List<OrdenCompraDetalleDTODetailsDBF>();

            foreach (var j in joined)
            {
                regs.Add(new OrdenCompraDetalleDTODetailsDBF()
                {
                    Nit = j.d.NIT,
                    Orden = j.d.ORDEN,
                    MaterialCodigo = j.d.CODMAT,
                    Renglon = j.d.RENGL,
                    MaterialNombre = j.m.DESCRI,
                    UnidadMedida = j.d.UNIDET,
                    Cantidad = (decimal)j.d.CANDET,
                    Precio = (decimal)j.d.PREDET,
                    Valor = (decimal)j.d.VALDET,
                    CantidadRecibido = (decimal)j.d.CANREC
                    // Descipcion = j.d.
                    //Material = j.d.MATSOL,
                    //Descripcion = j.m.DESCRI,
                    //Cantidad = j.d.CANDET,
                    //Valor = j.d.VALSOL
                });
            }

            reg.Detalle = regs;
            return PartialView("_DetailsDBF", reg);
        }


        #region Js

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndexDBF(string Buscar, int Year ,int? page)
        {            
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            dbfContext.SetYear(Year);

            var regs = this.dbfContext.GetOrdenesCompra();
            if (regs == null)
                return PartialView("_ErrorSiahr");
            if (!String.IsNullOrEmpty(Buscar))
            {
                regs = regs.Where(s =>
                s.ORDEN.Contains(Buscar) ||
                s.NIT.ToUpper().Contains(Buscar.ToUpper()) ||
                (s.Fecha.HasValue ? s.Fecha.Value.ToString("d") : "").Contains(Buscar)
                );
            }

            regs = regs.ToList();
            var dto = regs.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            ViewBag.Year = Year;

            return PartialView("_IndexDBFGrid", dto);
        }


        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndex(string Buscar, int? page)
        {
            IQueryable<OrdenCompra> regs;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;


            regs = (from p in db.OrdenesCompra.Include("Proveedor") select p);


            if (!String.IsNullOrEmpty(Buscar))
            {
                regs = regs.Where(x =>
                x.OrdenCompraId.Contains(Buscar) ||
                x.Proveedor.Nit.ToUpper().Contains(Buscar.ToUpper()) ||
                (DbFunctions.Right("0" + SqlFunctions.DatePart("d", x.Fecha), 2) + "/" + DbFunctions.Right("0" + SqlFunctions.DatePart("m", x.Fecha), 2) + "/" + SqlFunctions.DatePart("yyyy", x.Fecha)).Contains(Buscar) 
                );
            }
            regs = regs.OrderBy(x => x.SolicitudPedidoId);

           // regs = regs.ToList();
            var dto = regs.ToPagedList(pageIndex, pageSize);
            ViewBag.PLROpions = PagedListOptions;
            return PartialView("_IndexGrid", dto);
        }



        public async Task<JsonResult> JsImportOrdenCompra(string CODIGO, int year)
        {
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }

                dbfContext.SetYear(year);

                // Datos a Importar
                var OrdenCompraDBF = dbfContext.GetOrdenCompra(CODIGO);
                var OrdenCompraDetalleDBF = dbfContext.GetOrdenCompraDetalle(CODIGO);
                // Importar Datos Relacionados
                
                // Importar Proveedor
                ProveedorRep.ImportIfNotExist(OrdenCompraDBF.NIT);
                int ProveedorId = ProveedorRep.GetIdFromNit(OrdenCompraDBF.NIT) ?? 0;
                int NumeroSP = 0;
                Int64? SolicitudPedidoId = null;

                // Importar materiales si no existen                
                foreach (var item in OrdenCompraDetalleDBF)
                {
                    MaterialRep.ImportIfNotExist(item.CODMAT);
                    NumeroSP = Convert.ToInt32(item.SOLREL);
                }

                var NumeroOC = Convert.ToInt32(OrdenCompraDBF.ORDEN); //OrdenCompraRep.OrdenCompraIdFromDBF(year, OrdenCompraDBF.ORDEN);

                using (DbContextTransaction t = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Buscar SP
                        var sp = await db.ORTSolicitudesPedido.Where(x => x.Numero == NumeroSP && x.Anio == year && x.OrdenCompraId == null).FirstOrDefaultAsync();
                        if (sp != null)
                        {
                            SolicitudPedidoId = sp.SolicitudPedidoId;                           
                        }   
                        
                        if (!db.ORTOrdenesCompra.Any(x => x.Anio == year && x.Numero == NumeroOC))
                        { // INSERT
                            var oc = new ORTOrdenCompra()
                            {
                                Numero = NumeroOC,
                                Anio = year,
                                ProveedorId = ProveedorId,
                                SolicitudPedidoId = SolicitudPedidoId,
                                Fecha = OrdenCompraDBF.Fecha ?? DateTime.MinValue,
                                Observacion = OrdenCompraDBF.OBSORD,
                                Lineas = 0,                               
                            };
                            db.ORTOrdenesCompra.Add(oc);
                            await db.SaveChangesAsync();

                            // Actualizar Referencia de Numero de Orden en Solicitud de Pedido Detalle
                            if (sp != null)
                            {
                                sp.OrdenCompraId = oc.OrdenCompraId;
                                await db.ORTMovimientos
                                    .Where(x => x.SolicitudPedidoId == SolicitudPedidoId && x.Tipo == "SOL")
                                    .ForEachAsync(x => x.OrdenCompraId = oc.OrdenCompraId);
                            }

                            // Agrega el detalle de la orden de compra
                            foreach (var item in OrdenCompraDetalleDBF)
                            {
                                int MaterialId = MaterialRep.GetIdFromCodigo(item.CODMAT) ?? 0;
                                db.ORTMovimientos.Add(new ORTMovimiento()
                                {
                                    Tipo = "ORD",
                                    SolicitudPedidoId = SolicitudPedidoId,
                                    OrdenCompraId = oc.OrdenCompraId,
                                    ProveedorId = ProveedorId,
                                    Cantidad = item.CANDET ?? 0,
                                    Precio = item.PREDET ?? 0,
                                    Valor = item.VALDET ?? 0,
                                    MaterialId = MaterialId,
                                    Fecha = oc.Fecha,
                                });
                            }
                        }
                        else
                        { // UPDATE
                            var oc = await db.ORTOrdenesCompra.Where(x => x.Anio == year && x.Numero == NumeroOC).FirstOrDefaultAsync();
                            if (oc != null) // La orden de compra exite y se va a actualizar
                            {
                                oc.SolicitudPedidoId = SolicitudPedidoId;
                                oc.Fecha = OrdenCompraDBF.Fecha ?? DateTime.MinValue;

                                // Actualizar Referencia de Numero de Orden --> Solicitud de Pedido Detalle
                                if (sp != null)
                                {
                                    sp.OrdenCompraId = oc.OrdenCompraId;    // asigna numero oc en sp encabezado
                                    await db.ORTMovimientos                 // asinga numero oc en sp detalle
                                        .Where(x => x.SolicitudPedidoId == SolicitudPedidoId && x.Tipo == "SOL")
                                        .ForEachAsync(x => x.OrdenCompraId = oc.OrdenCompraId);
                                }

                                await db.ORTMovimientos
                                    .Where(x => x.OrdenCompraId == oc.OrdenCompraId && x.Tipo == "ORD")
                                    .ForEachAsync(x => x.ProveedorId = ProveedorId);
                            }
                        }
                        await db.SaveChangesAsync();
                        t.Commit();
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                }                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private
        private string FixFormatId(string id)
        {
            return id.PadRight(6);
        }
        #endregion
    }
}