using Apphr.Application.OrdenesCompra.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    [LogAction]
    public class OrdenesCompraController : DbController
    {
        [AppAuthorization(Permit.View)]
        public ActionResult Index() // GET
        {
            return View();
        }

        [AppAuthorization(Permit.View)]
        public ActionResult IndexDBF() // GET
        {
            return View();
        }

        [AppAuthorization(Permit.View)]
        public ActionResult DetailsDBF(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = FixFormatId(id);
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
                //DIASOL = dte.DIASOL,
                //MESSOL = dte.MESSOL,
                //ANOSOL = dte.ANOSOL,
                //Departamento = dte.DEPSOL,
                //Observaciones = dte.OBSSOL,
                //Solicito = dte.SOLSOL,
                //Elaboro = dte.OTRSOL,
                //Jefe = dte.JEFSOL,
                //Gerente = dte.GERSOL,
                //Director = dte.DIRSOL,
                //Tipo = dte.TIPSOL,
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
                    Valor = (decimal)j.d.VALDET
                    // Descipcion = j.d.
                    //Material = j.d.MATSOL,
                    //Descripcion = j.m.DESCRI,
                    //Cantidad = j.d.CANDET,
                    //Valor = j.d.VALSOL
                });
            }

            reg.Detalle = regs;
            return View(reg);
        }


        #region Js

        [ValidateAntiForgeryToken]
        public ActionResult JsFilterIndexDBF(string Buscar, int? page)
        {            
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var regs = this.dbfContext.GetOrdenesCompra();
            
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
            return PartialView("_IndexDBFGrid", dto);
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