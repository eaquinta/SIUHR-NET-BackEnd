using Apphr.Application.Common;
using Apphr.Application.Common.DTOs;
using Apphr.Application.SiahadDespacho.DTOs;
using Apphr.Application.SiahadIngreso.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.Resources;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class SiahadController : DbController
    {
        private SHDContadorRepository SHDContadorRep;
        private SHRMaterialRepository SHRMaterialRep;
        private SHDMovimientoInventarioRepository SHDMovimientoInventarioRep;
        public SiahadController()
        {
            SHDContadorRep = new SHDContadorRepository(db);
            SHRMaterialRep = new SHRMaterialRepository(db);
            SHDMovimientoInventarioRep = new SHDMovimientoInventarioRepository(db);
        }

        #region Ingesos
        public ActionResult Ingresos() // GET
        {
            var UserRoles = GetUserRoleList();
            ViewBag.ListAdminitraciones = db.SHDAdministracion.Where(x => UserRoles.Contains(x.AccessRole)).Select(x => new SelectListItem {
                Text = x.Nombre,
                Value = x.AdministracionId.ToString()
            }).ToList();
            var dto = new SiahadIngresoDTOCreate() { FechaMovimiento = DateTime.Now.Date, FechaEmision = DateTime.Now.Date };
            return View(dto);
        }
        public ActionResult JsDoIngreso(SiahadIngresoDTOCreate dto)
        {
            if (!ModelState.IsValid)
            {

            }
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (dto.Documento == null)
                    {
                        // Numeracion de Ingresos
                        dto.Documento = SHDContadorRep.GetNextNum(dto.AdministracionId, SHDTipoMovimiento.Ingreso);
                        dto.Correlativo = 1;
                    }
                    else { dto.Correlativo += 1; }
                                     
                    SHDMovimientoInventarioRep.AddIngreso(dto);
                    transaction.Commit();                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            return Json(new { success = true, data = dto, message = Msg.success_create }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        #endregion

        public ActionResult Despachos() // GET
        {
            return View();
        }

        public ActionResult JsDoDespacho(SiahadDespachoDTOCreate dto)
        {
            if (!ModelState.IsValid)
            {

            }
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (dto.Documento == null)
                    {
                        // Numeracion de Depachos 
                        dto.Documento = SHDContadorRep.GetNextNum(dto.AdministracionId, SHDTipoMovimiento.Despacho);
                        dto.Correlativo = 1;
                    }
                    else { dto.Correlativo += 1; }
                   
                    SHDMovimientoInventarioRep.AddDespacho(dto);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            return Json(new { success = true, data = dto, message = Msg.success_create }, JsonRequestBehavior.AllowGet);
            //return View();
        }

        public ActionResult Traslados() // GET
        {
            return View();
        }
        public ActionResult OperarDevolucionesAlmacenes() // GET
        {
            return View();
        }
        public ActionResult OperarDevolucionesServicios() // GET
        {
            return View();
        }
        public ActionResult OperarRevisionesAjustes() // GET
        {
            return View();
        }

        public ActionResult ImportMateriales()
        {
            dbfContext.SetYear(2023);
            var dbf = dbfContext.GetMateriales().ToList();
            //db.Database.ExecuteSqlCommand($"DELETE FROM SHRMateriales");
            //db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('[dbo].[SHRMateriales]', RESEED, 0)");
            foreach (var item in dbf)
            {
                var reg = new SHRMaterial()
                {
                    Codigo = item.CODIGO,
                    Descripcion = item.DESCRI,
                    UnidadMedida = item.UNIMED,
                    Producto = item.PRODUC,
                    Precio = item.PRECIO ?? 0,
                    Minimo = item.MINIMO,
                    Maximo = item.MAXIMO,
                    Ojo = item.OJO,
                    AlternoDe = item.ALTERNODE,
                    Bres = item.BRES,
                    Codigi = item.CODIGI,
                    //FechaCreacion = item.FECHA ?? DateTime.MinValue,
                    FechaCreacion = item.FechaCreacion ?? DateTime.MinValue,
                    Usuario = item.USUA,
                    Estatus = item.STATUS,                    
                    UsoBodega = item.USOBOD,
                    VigenciaDe = item.VigenciaDe,
                    VigenciaA = item.VigenciaA,
                    UsoServicio = item.USOSER,
                };
                try
                {
                    if (!db.SHRMaterial.Any(x => x.Codigo == reg.Codigo && x.Codigi == reg.Codigi))
                    {
                        db.SHRMaterial.Add(reg);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Console.Write(reg.Codigo);
                    //throw;
                }
                
            }
            ViewBag.Registros = db.SHRMaterial.Count();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> JsMaterialCodigoExist(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                codigo = Request.Params[0];
            }
            var res = await db.SHRMaterial.Where(x => x.Codigo == codigo).AnyAsync();
            return Json(res);
        }

        public async Task<JsonResult> JsHRGetByFilter(string f, string tipo = "AC")
        {
            var result = new List<MaterialDTOAutocompleteItem>();
            if (!string.IsNullOrEmpty(f))
            {
                result = await db.SHRMaterial.Where(x => x.Codigo.Contains(f) || x.Descripcion.Contains(f))
                   .Take(autoCompleteSize)
                   .Select(p => new MaterialDTOAutocompleteItem { MaterialId = p.MaterialId.ToString(), id = p.Codigo, text ="("+p.Codigi+") "+  p.Descripcion })
                   .ToListAsync();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> JsGetByFilter(string val)
        {
            var result = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(val))
            {
                result = await db.SHDBodega.Where(x => x.Codigo.Contains(val) || x.Descripcion.Contains(val))
                   .Take(autoCompleteSize)
                   .Select(p => new SelectListItem { Value = p.Codigo, Text = p.Descripcion })
                   .ToListAsync();
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }






        public async Task<JsonResult> JsGetMaterialCantidadByCodigoOrId(string MaterialCodigo, int? MaterialId, int? AdministracionId)
        {
            try
            {
                var reg = new SHRMaterial();
                if (MaterialId == null)
                {
                    reg = await db.SHRMaterial.FindAsync(MaterialId);
                }
                else
                {
                    reg = await SHRMaterialRep.GetMaterialByCodigoAsync(MaterialCodigo);
                }                
                var exi = await db.SHDExistenciaTotal.Where(x => x.AdministracionId == AdministracionId && x.IdMaterial == reg.MaterialId).FirstOrDefaultAsync();
                if (reg == null)
                {
                    throw new ArgumentException("");
                }

                var dto = new
                {
                    MaterialId      = reg.MaterialId,
                    Codigo          = reg.Codigo,
                    Descripcion     = reg.Descripcion,
                    UnidadMedida    = reg.UnidadMedida,
                    Producto        = reg.Producto,
                    Codigi          = reg.Codigi,
                    Precio          = reg.Precio,
                    Minimo          = reg.Minimo ?? 0,
                    Cantidad        = exi?.Cantidad ?? 0, //BodegaRep.GetExistencia(BodegaId ?? 0, reg.MaterialId)
                    Valor           = exi?.Valor ?? 0
                };

                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> JsBodegaCodigoExist(string Codigo, int? AdministracionId)
        {
            if (string.IsNullOrEmpty(Codigo))
            {
                Codigo = Request.Params[0];
            }
            if (AdministracionId == null)
            {
                AdministracionId = Convert.ToInt32(Request.Params[1]);
            }
            var res = await db.SHDBodega.Where(x => x.Codigo == Codigo).AnyAsync();
            return Json(res);
        }
        

        public async Task<JsonResult> JsGetBodegaByNombre(string nombre)
        {
            var res = await db.SHDBodega.Where(x => x.Codigo == nombre).FirstOrDefaultAsync();
            if (res == null)
                return Json(new { success = false, data = res }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = true, data = res }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<JsonResult> JsDestinoCodigoExist(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                codigo = Request.Params[0];
            }
            var res = await db.SHRDestino.Where(x => x.Codigo == codigo).AnyAsync();
            return Json(res);
        }
    }
}