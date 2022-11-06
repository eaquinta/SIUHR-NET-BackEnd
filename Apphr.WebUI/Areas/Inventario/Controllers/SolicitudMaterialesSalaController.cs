using Apphr.Application.SolicitudMaterialesSala.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using CrystalDecisions.CrystalReports.Engine;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
	[Authorize]
    public class SolicitudMaterialesSalaController : DbController
    {
		private SolicitudMaterialSalaRepository SolicitudMaterialSalaRep;
		

		public SolicitudMaterialesSalaController()
        {
			SolicitudMaterialSalaRep = new SolicitudMaterialSalaRepository(db);
        }
		[AppAuthorization(Permit.View)]
		public ActionResult Index() // GET
        {
			ViewBag.PLROpions = PagedListOptions;
			return View();
        }

		[AppAuthorization(Permit.View)]
		public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var reg = await SolicitudMaterialSalaRep.GetDetailsAsync(id);
			
			var dto = mapper.Map<SolicitudMaterialSalaDTODetails>(reg);
			// mappeo especial
			dto.PacienteRM = reg.Paciente?.RefPac_NumHCAntiguo.ToString();
			dto.PacienteNombreCompleto = reg.Paciente?.Persona?.Name;
			return View(dto);
        }

		[AppAuthorization(Permit.Edit)]
		public async Task<ActionResult> CEdit(string id) // GET
		{
			var dto = new SolicitudMaterialSalaDTOCEdit();
            if (id == null)
            {
				//INSERT
				dto.FechaOperacion = DateTime.Now.Date;
				dto.Fecha = DateTime.Now.Date;
			}
			else
            {
				//UPDATE
				var reg = await SolicitudMaterialSalaRep.GetDetailsAsync(id);

				// map
				dto = mapper.Map<SolicitudMaterialSalaDTOCEdit>(reg);

				// special map
				dto.PacienteRM = reg.Paciente?.RefPac_NumHCAntiguo.ToString();
				dto.PacienteNombreCompleto = reg.Paciente?.Persona?.Name;
			}

			return View(dto);
		}

		public async Task<ActionResult> Print(string id) // GET
		{
			var def = await SolicitudMaterialSalaRep.GetDetailsAsync(id);

			var regsReport = new List<SolicitudMaterialSalaDTORpt>();
            foreach (var item in def.Detalle)
            {
				regsReport.Add(new SolicitudMaterialSalaDTORpt()
				{
					PacienteNombreCompleto = def.Paciente.Persona.Name,
					PacienteRM = def.Paciente.RefPac_NumHCAntiguo.ToString(),
					Servicio = def.Servicio,
					Cama  = def.Cama,
					Fecha = def.Fecha,
					Cirujano = def.Cirujano,
					AuxInstrumentista = def.AuxiliarEnfermeriaInstrumentista,
					AuxCirculante = def.AuxiliarEnfermeriaCirculante,
					Observacion = def.Observacion,
					MaterialNombre = "["+item.Material.Codigo+"] "+item.Material.Descripcion,
					Cantidad = item.Cantidad
				});
            }
			ReportDocument rd = new ReportDocument();
			rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptSolicitudMaterialesSala.rpt")));
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

		#region Js
		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveMaster(SolicitudMaterialSalaDTOCEdit dto)
		{
            Permit[] permisosRequeridos = { Permit.Edit };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
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
				if (string.IsNullOrEmpty(dto.SolicitudMaterialSalaId))
				{
					// INSERT
					var reg = await SolicitudMaterialSalaRep.InsertMaster(dto);
					
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
				}
				else
				{
					// UPDATE
					var reg = await SolicitudMaterialSalaRep.UpdateMaster(dto);
					
					return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
			}
		}

		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveChild(SolicitudMaterialSalaDetalleDTOCEdit dto)
		{
            Permit[] permisosRequeridos = { Permit.Edit };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
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
				if (dto.SolicitudMaterialSalaDetalleId == 0)
				{
					// INSERT
					using (DbContextTransaction transaction = db.Database.BeginTransaction())
					{
						try
						{
							// Actualizacion Master
							var masterReg = await db.SolicitudMaterialesSala.FindAsync(dto.SolicitudMaterialSalaId);
							masterReg.Lineas += 1;						
							
							// Crear Child 
							var childReg = new SolicitudMaterialSalaDetalle()
							{
								SolicitudMaterialSalaId = dto.SolicitudMaterialSalaId,
								MaterialId = dto.MaterialId,
								Cantidad = dto.Cantidad,
								Intercambio = dto.Intercambio,
								ProveedorId = dto.ProveedorId
							};

							db.SolicitudMaterialesSalaDetalle.Add(childReg);
							await db.SaveChangesAsync();
							transaction.Commit();
						}
						catch (Exception)
						{
							transaction.Rollback();
							throw;
						}
					}
					return Json(new { success = true, message = Resources.Msg.success_create });
				}
				else
				{
					// UPDATE
					// Despacho Inventario Detalle
					var reg = await db.SolicitudMaterialesSalaDetalle
						.Where(x => x.SolicitudMaterialSalaDetalleId == dto.SolicitudMaterialSalaDetalleId)
						//.Include("MovimientoInventario")
						.FirstOrDefaultAsync();
					reg.MaterialId = dto.MaterialId;
					reg.Cantidad = dto.Cantidad;					
					reg.ProveedorId = dto.ProveedorId;
					reg.Intercambio = dto.Intercambio;

					await db.SaveChangesAsync();
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
				using (DbContextTransaction transaction = db.Database.BeginTransaction())
				{
					try
					{
						var reg = await db.SolicitudMaterialesSala.Where(x => x.SolicitudMaterialSalaId == id).FirstOrDefaultAsync();
						var det = await db.SolicitudMaterialesSalaDetalle.Where(x => x.SolicitudMaterialSalaId == reg.SolicitudMaterialSalaId).ToListAsync();
						foreach (var item in det)
						{
							db.SolicitudMaterialesSalaDetalle.Remove(item);
						}
						db.SolicitudMaterialesSala.Remove(reg);
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
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
			}
		}


		[ValidateAntiForgeryToken]
		public async Task<ActionResult> JsDeleteChild(int? id)
		{
            Permit[] permisosRequeridos = { Permit.Delete };
            bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
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
						var reg = await db.SolicitudMaterialesSalaDetalle
							.Where(x => x.SolicitudMaterialSalaDetalleId == id)
							.FirstOrDefaultAsync();

						db.SolicitudMaterialesSalaDetalle.Remove(reg);
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
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
			}
		}


		[ValidateAntiForgeryToken]
		public ActionResult JsFilterIndex(string Buscar, int? page)
		{
			int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			IQueryable<SolicitudMaterialSala> regs;

			regs = db.SolicitudMaterialesSala
				.Include(x => x.Paciente)
				.Include("Paciente.Persona");

			if (!String.IsNullOrEmpty(Buscar))
			{
				regs = regs.Where(s =>
				s.HojaControlSala.Contains(Buscar) ||
				//s.NIT.ToUpper().Contains(Buscar.ToUpper()) ||
				s.Fecha.ToString("d").Contains(Buscar)
				);
			}
			regs = regs.OrderBy(x => x.Fecha);

			var rows = regs
				.Select(x => new SolicitudMaterialSalaDTOIndex()
				{
					SolicitudMaterialSalaId = x.SolicitudMaterialSalaId,
					HojaControlSala = x.HojaControlSala,
					Fecha = x.Fecha,
					PacienteRM = x.Paciente.RefPac_NumHCAntiguo.ToString(),
					PacienteNombreCompleto = x.Paciente.Persona.Name
				})
				.ToList();
			var dto = (PagedList<SolicitudMaterialSalaDTOIndex>)rows.ToPagedList(pageIndex, pageSize);
			
			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}

		public async Task<ActionResult> JsGrid(string id)
		{
			if (id == null)
			{ return null; }
			var regs = await SolicitudMaterialSalaRep.GetChildGridAsync(id);
			var dto = mapper.Map<IEnumerable<SolicitudMaterialSalaDetalleDTOBase>>(regs);
			return PartialView("_CEditGrid", dto);
		}

		public async Task<ActionResult> JsGetChild(int? id)
		{
			try
			{
				var reg = await db.SolicitudMaterialesSalaDetalle
					.Include("Material")
					.Include("Proveedor")
					.Where(x => x.SolicitudMaterialSalaDetalleId == id)
					.FirstOrDefaultAsync();
				var dto = mapper.Map<SolicitudMaterialSalaDetalleDTOCEdit>(reg);
				// Special Map
				dto.MaterialCodigo = reg.Material.Codigo;
				dto.MaterialNombre = reg.Material.Descripcion;
				dto.MaterialUM = reg.Material.UnidadMedida;
				//dto.MaterialPrecio = reg.Material.Precio ?? 0;
				dto.ProveedorNit = reg.Proveedor?.Nit ?? "";
				dto.ProveedorNombre = reg.Proveedor?.Descripcion ?? "";

				return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Apphr.Resources.Msg.failure, exmessage = ex.Message, exinner = ex.InnerException }, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion
	}
}