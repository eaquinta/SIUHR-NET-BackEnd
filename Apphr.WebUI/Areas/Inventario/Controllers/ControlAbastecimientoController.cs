using Apphr.Application.ControlAbastecimiento.DTOs;
using Apphr.Application.IngresosAbastecimiento.DTOs;
using Apphr.Application.InicialAbastecimiento.DTOs;
using Apphr.Application.SolicitudesPedido.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
	[LogAction]
	public class ControlAbastecimientoController : DbController
	{
		private MaterialRepository MaterialRep;
		private SolicitudMaterialSalaRepository SolicitudMaterialSalaRep;
		private ControlAbastecimientoRepository ControlAbastecimientoRep;
		private MovimientosAbastecimientoRepository MovimientosAbastecimientoRep;
		public ControlAbastecimientoController()
		{
			MaterialRep = new MaterialRepository(db);
			SolicitudMaterialSalaRep = new SolicitudMaterialSalaRepository(db);
			ControlAbastecimientoRep = new ControlAbastecimientoRepository(db);
			MovimientosAbastecimientoRep = new MovimientosAbastecimientoRepository(db);
		}

		[Can("control_abastecimiento.ver")]
		public ActionResult Index(string Codigo) // GET
		{
			var dto = new SolicitudPedidoDTOIxDBFFilter();
			return View();
		}


		public ActionResult InicialMateriales()
        {
			return View();
        }

		public ActionResult JsFilterIndex(int? MaterialId)
		{
			//var MaterialId = MaterialRep.GetMaterialByCodigoAsync(Codigo);
			//var regs = db.SolicitudesPedidoDetalle.Include(x => x.Material).ToList();
			var parameters = new List<SqlParameter>();
			
			//Q005
			string query = @"
				SELECT
					ca.DestinoId, 
					ca.MaterialId, 
					m.Codigo AS MaterialCodigo, 
					m.Descripcion AS MaterialDescripcion, 
					spd.SolicitudPedidoId, 
					spd.Cantidad AS CantidadSolicitado, 
					m.SigesCodigo AS MaterialCodigoSiges, 
					sp.Fecha AS FechaSolicitud, 
					spd.Precio AS PrecioUnitario, 
					spd.Valor AS ValorTotal, 
					ocd.OrdenCompraId, 
					ocd.Cantidad AS CantidadOrdenado, 
					p.Descripcion AS ProveedorNombre, 
					oc.Fecha AS FechaOrden, 
					sp.TipoEvento AS TipoCompra,
					ia.Fecha AS FechaIngresoAlmacen,
					ia.Cantidad AS CantidadIngreso,
					ea.Cantidad AS CantidadEgreso
				FROM
					dbo.ControlAbastecimiento AS ca
					INNER JOIN
					dbo.Materiales AS m
					ON 
						ca.MaterialId = m.MaterialId
					LEFT JOIN
					dbo.SolicitudPedidoDetalle AS spd
					ON 
						m.MaterialId = spd.MaterialId
					LEFT JOIN
					dbo.SolicitudPedido AS sp
					ON 
						spd.SolicitudPedidoId = sp.SolicitudPedidoId
					LEFT JOIN
					dbo.OrdenCompraDetalle AS ocd
					ON 
						spd.MaterialId = ocd.MaterialId AND
						spd.OrdenCompraId = ocd.OrdenCompraId
					LEFT JOIN dbo.OrdenCompra AS oc ON ocd.OrdenCompraId = oc.OrdenCompraId
					LEFT JOIN dbo.Proveedores AS p ON  oc.ProveedorId = p.ProveedorId
					LEFT JOIN (
							SELECT
								ia.OrdenCompraId,
								ia.MaterialId,
								SUM(ia.Cantidad) AS Cantidad,
								Max(ia.Fecha) AS Fecha 
							FROM
								dbo.MovimientosAbastecimiento AS ia
							WHERE 
								TipoMovimiento = 'ING' 
							GROUP BY
								ia.MaterialId, 
								ia.OrdenCompraId
					) AS ia ON ia.MaterialId = ca.MaterialId AND ia.OrdenCompraId = ocd.OrdenCompraId
					LEFT JOIN (
							SELECT	
								ea.OrdenCompraId, 
								ea.MaterialId,
								SUM(ea.Cantidad) AS Cantidad, 
								Max(ea.Fecha) AS Fecha
							FROM	
								dbo.MovimientosAbastecimiento AS ea
							WHERE 
								TipoMovimiento = 'EGR' 
							GROUP BY
								ea.MaterialId, 
								ea.OrdenCompraId
					) AS ea ON ea.MaterialId = ca.MaterialId AND ea.OrdenCompraId = ocd.OrdenCompraId
                    ";
			if (MaterialId != null)
			{
				query += " WHERE ca.MaterialId = @MaterialId";
				parameters.Add(new SqlParameter("@MaterialId", MaterialId));
			}
			query = @"Abastecimiento @MaterialId";
			var dto = new List<ControlAbastecimientoDTOIndex>();
			
			if (MaterialId != null)
				 dto = db.Database.SqlQuery<ControlAbastecimientoDTOIndex>(query, parameters.ToArray()).ToList();
				
			return PartialView("_IndexGrid", dto);
		}

		public ActionResult JsFilterInicialMaterialesIndex()
        {
			var dto = db.InicialAbastecimiento
				.Include("Material")
				.Include("Proveedor")
				.Select(x => new InicioAbastecimientoDTOIndex()
				{
					DestinoId = 0,
					MaterialCodigo = x.Material.Codigo,
					MaterialDescripcion = x.Material.Descripcion,
					ProveedorId = x.ProveedorId,
					ProveedorNit = x.Proveedor.Nit,
					ProveedorNombre = x.Proveedor.Descripcion,
					MaterialId = x.MaterialId,
					Cantidad = x.Cantidad 
				}).ToList();
			return PartialView("_IndexMaterialGrid", dto);
        }

		public ActionResult JsListaIngresos(int MaterialId, string OrdenCompraId)
		{
			if (string.IsNullOrEmpty(OrdenCompraId))
            {
				OrdenCompraId = "INICIAL";
            }
			//var dto = db.IngresosAbastecimiento.Where(x => x.MaterialId == MaterialId && x.OrdenCompraId == OrdenCompraId).Select(x => new ControlAbastecimientoDTOListaIngresos()
			var dto = db.MovimientosAbastecimiento.Where(x => 
				x.MaterialId == MaterialId && 
				x.OrdenCompraId == OrdenCompraId &&
				x.TipoMovimiento == "ING"
			).Select(x => new ControlAbastecimientoDTOListaIngresos()
			{
				MaterialId = x.MaterialId,				
				IngresoAbastecimientoId = x.MovimientoAbastecimientoId,
				Cantidad = x.Cantidad,
				Fecha = x.Fecha
			}).ToList();

			return PartialView("_Ingresos", dto);
		}

		public ActionResult JsListaEgresos(int MaterialId, string OrdenCompraId)
		{
			var dto = ControlAbastecimientoRep.GetEgresos(MaterialId, OrdenCompraId);			

			return PartialView("_Egresos", dto);
		}

		public ActionResult JsListaIniciales(int MaterialId)
        {
			var dto = db.InicialAbastecimiento.Include("Proveedor").Where(x => x.MaterialId == MaterialId).ToList();
			return PartialView("_Iniciales", dto);
        }

		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsDeleteIngreso(int? id)
		{
			string[] permisosRequeridos = { "control_abastecimiento.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				//var reg = db.IngresosAbastecimiento.Find(id);
				var reg = db.MovimientosAbastecimiento.Find(id);
				db.MovimientosAbastecimiento.Remove(reg);
				db.SaveChanges();
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}

		public async Task<JsonResult> JsDeleteEgreso(int? id)
		{
			string[] permisosRequeridos = { "control_abastecimiento.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				//var reg = db.IngresosAbastecimiento.Find(id);
				var reg = db.MovimientosAbastecimiento.Find(id);
                if (reg.SolicitudMaterialSalaId != null)
                {
					throw new ArgumentException("No es posible eliminar este registro");
                }	
				db.MovimientosAbastecimiento.Remove(reg);
				db.SaveChanges();
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}

		public async Task<JsonResult> JsDeleteInicial(int MaterialId, int ProveedorId)
        {
			string[] permisosRequeridos = { "control_abastecimiento.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				var reg = db.InicialAbastecimiento.Find(MaterialId, ProveedorId);
				db.InicialAbastecimiento.Remove(reg);

				var regs = db.MovimientosAbastecimiento.Where(x => x.MaterialId == MaterialId && x.ProveedorId == ProveedorId && x.OrdenCompraId == "INICIAL").ToList();
				if (regs != null)
                    foreach (var item in regs)
                    {
						db.MovimientosAbastecimiento.Remove(item);
					}				
				db.SaveChanges();
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}

		public ActionResult JsGetEgresoForm()
		{
			var dto = new IngresoAbastecimientoDTOBase() {
				Fecha = DateTime.Now.Date
				};
			return PartialView("_CEditEgreso", dto);
		}

		public ActionResult JsGetIngresoForm()
		{
			var dto = new IngresoAbastecimientoDTOBase()
			{
				Fecha = DateTime.Now.Date
			};
			return PartialView("_CEditIngreso", dto);
		}

		public ActionResult JsGetInicialForm(int? MaterialId)
        {
			var dto = new InicialAbastecimientoDTOCEdit();
			if (MaterialId != null)
            {
				dto = db.Materiales
					.Where(x => x.MaterialId == MaterialId)
					.Select(x => new InicialAbastecimientoDTOCEdit() { MaterialId = x.MaterialId, MaterialCodigo = x.Codigo, MaterialNombre = x.Descripcion })
					.FirstOrDefault();
			}
			 
			return PartialView("_AddInicial", dto);
        }

		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsCEditInicial(InicialAbastecimientoDTOCEdit dto)
		{
			try
			{
				if (!db.InicialAbastecimiento.Any(x => x.MaterialId == dto.MaterialId && x.ProveedorId == dto.ProveedorId))
				{
					// INSERT
					ControlAbastecimientoRep.AddMaterial(dto.MaterialId);										

					var reg = new InicialAbastecimiento()
					{
						MaterialId = dto.MaterialId,
						ProveedorId = dto.ProveedorId,
						Cantidad = dto.Cantidad
					};
					db.InicialAbastecimiento.Add(reg);
					await MovimientosAbastecimientoRep.SaveIngresoAsync(
							dto.MaterialId,
							dto.ProveedorId,
							"INICIAL",
							dto.Cantidad,
							DateTime.Now.Date
					);					
					db.SaveChanges();
					return Json(new { success = true, message = Resources.Msg.success_create });
				}
				else
				{
					// UPDATE
					return Json(new { success = false, message = "Este proveedor ya tiene una existencia incial" });
					//var reg = db.InicialAbastecimiento.Find(dto.MaterialId, dto.ProveedorId);
					//reg.Cantidad = dto.Cantidad;
					//db.SaveChanges();
					//return Json(new { success = true, message = Resources.Msg.success_create });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}
		}


		public JsonResult JsGetInicial(int? DestinoId, int? MaterialId) // GET
		{
			try
			{
				var res = db.ControlAbastecimiento.Include("Material").Where(x => x.MaterialId == MaterialId).FirstOrDefault(); //x.DestinoId == DestinoId &&
				if (res == null)
                {
					throw new ArgumentException("Sin Datos");
                }
				//res.Inicial = res.Inicial ?? 0;
				return Json(new { success = true, result = res }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
			}		
		}


		public JsonResult JsUpdateInicial(int DestinoId, int MaterialId, int Inicial)
        {
            try
            {
				var reg = db.ControlAbastecimiento.Where(x => x.MaterialId == MaterialId).FirstOrDefault(); //x.DestinoId == DestinoId &&
				//reg.Inicial = Inicial;
				db.SaveChanges();
				return Json(new { success = true, message = Resources.Msg.success_edit }, JsonRequestBehavior.DenyGet);
			}
            catch (Exception ex)
            {
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
        }


		[ValidateAntiForgeryToken]		
		public async Task<JsonResult> JsSaveIngreso(MovimientoAbastecimiento dto)
		{
			string[] permisosRequeridos = { "control_abastecimiento.editar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
                if (dto.MaterialId == 0 || string.IsNullOrEmpty(dto.OrdenCompraId))
                {
					return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
				}
				if (dto.MovimientoAbastecimientoId == 0)
				{
					// INSERT 

					try
					{
						await MovimientosAbastecimientoRep.SaveIngresoAsync(
							dto.MaterialId,
							dto.ProveedorId??0,
							dto.OrdenCompraId,
							dto.Cantidad,
							dto.Fecha							
						);
						//db.IngresosAbastecimiento.Add(reg);
						//await db.SaveChangesAsync();
					}
					catch (Exception)
					{
						throw;
					}

					return Json(new { success = true, message = Resources.Msg.success_create });
				}
				else
				{
					// UPDATE 
					var reg = await db.IngresosAbastecimiento.FindAsync(dto.MovimientoAbastecimientoId);
					reg.Fecha = dto.Fecha;
					reg.Cantidad = dto.Cantidad;

					await db.SaveChangesAsync();
					return Json(new { success = true, message = Resources.Msg.success_edit });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}

		}


		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveEgreso(MovimientoAbastecimiento dto)
		{
			string[] permisosRequeridos = { "control_abastecimiento.editar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
				if (dto.MaterialId == 0 || string.IsNullOrEmpty(dto.OrdenCompraId))
				{
					return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
				}
				if (dto.MovimientoAbastecimientoId == 0)
				{
					// INSERT
					await MovimientosAbastecimientoRep.SaveEgresoAsync(
						dto.MaterialId,
						dto.ProveedorId??0,
						dto.OrdenCompraId,
						dto.Cantidad,
						dto.Fecha
					);

					return Json(new { success = true, message = Resources.Msg.success_create });
				}
				else
				{
                    // UPDATE                    
					MovimientosAbastecimientoRep.UpdateEgreso(
						dto.MovimientoAbastecimientoId,
						dto.Cantidad,
						dto.Fecha
					);						
					
					return Json(new { success = true, message = Resources.Msg.success_edit });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}

		}


		public ActionResult Print()
        {
			var regsReport = ControlAbastecimientoRep.GetExistencias();
			ReportDocument rd = new ReportDocument();
			rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptAbastecimientoExistencia.rpt")));
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


	}
}