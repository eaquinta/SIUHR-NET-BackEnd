using Apphr.Application.Common;
using Apphr.Application.Common.Models;
using Apphr.Application.SolicitudesDespachos.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities;
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
	public class SolicitudesDespachoController : DbController
	{
		public ActionResult Index(SolicitudDespachoDTOIndex dto, int? page) //GET
		{
			IQueryable<SolicitudDespacho> regs;

			int pageIndex = 1;
			if (dto?.F == null) dto.F = new IxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			//            BodegaDTOIndex dto = new BodegaDTOIndex();


			regs = (from p in db.SolicitudesDespacho.Include("Departamento") select p);
			if (dto.F != null)
			{
				if (!string.IsNullOrEmpty(dto.F.Buscar))
					regs = regs.Where(x => x.SolicitudDespachoId.Contains(dto.F.Buscar) ||
					(DbFunctions.Right("0" + SqlFunctions.DatePart("d", x.Fecha), 2) + "/" + DbFunctions.Right("0" + SqlFunctions.DatePart("m", x.Fecha), 2) + "/" + SqlFunctions.DatePart("yyyy", x.Fecha)).Contains(dto.F.Buscar) ||
					x.Departamento.Descripcion.Contains(dto.F.Buscar));

			}

			regs = regs.OrderBy(x => x.SolicitudDespachoId);

			var rows = mapper.Map<List<SolicitudDespachoDTOBase>>(regs.ToList());
			dto.Result = (PagedList<SolicitudDespachoDTOBase>)rows.ToPagedList(pageIndex, pageSize);
			ViewBag.PLROpions = PagedListOptions;
			return View(dto);
		}


		public ActionResult IndexDBF() // GET
		{
			return View();
		}


		public async Task<ActionResult> CEdit(string id) // GET
		{
			var dto = new SolicitudDespachoDTOCEdit();
			if (string.IsNullOrEmpty(id))
			{   
				// INSERT
				dto.Correlativo = Correlativo.Get();
				dto.Fecha = DateTime.Now;
				dto.year = DateTime.Now.Year.ToString();

			}
			else
			{   
				// UPDATE
				var reg = await db.SolicitudesDespacho.Where(x => x.SolicitudDespachoId == id)
					.Include("Departamento")
					.Include("Seccion")
					.FirstOrDefaultAsync();
				if (reg == null)
				{
					return HttpNotFound();
				}
				dto = mapper.Map<SolicitudDespachoDTOCEdit>(reg);
				// Aditional Mappings
				dto.DepartamentoCodigo = reg.Departamento.Codigo;
				dto.DepartamentoNombre = reg.Departamento.Descripcion;
				dto.SeccionCodigo = reg.Seccion.Codigo;
				dto.SeccionNombre = reg.Seccion.Descripcion;
			}

			ViewBag.Tipo = TipoPrioridad.GetSelectList();
			return View(dto);
		}


		public async Task<ActionResult> CEditMS(string id) //GET
        {
			var dto = new SolicitudDespachoDTOCEditMS();
			if (string.IsNullOrEmpty(id))
			{
				// INSERT
				dto.Correlativo = Correlativo.Get();
				dto.Fecha = DateTime.Now;
				dto.year = DateTime.Now.Year.ToString();

			}
			else
			{
				// UPDATE
				var reg = await db.SolicitudesDespacho.Where(x => x.SolicitudDespachoId == id)
					.Include("Departamento")
					.Include("Seccion")
					.Include("Paciente")
					.Include("Paciente.Persona")
					.FirstOrDefaultAsync();
				if (reg == null)
				{
					return HttpNotFound();
				}
				dto = mapper.Map<SolicitudDespachoDTOCEditMS>(reg);
				// Aditional Mappings
				dto.year = dto.SolicitudDespachoId.Substring(0, 4);
				dto.serie = reg.SolicitudDespachoId.Substring(5, 6);
				dto.DepartamentoCodigo = reg.Departamento.Codigo;
				dto.DepartamentoNombre = reg.Departamento.Descripcion;
				dto.SeccionCodigo = reg.Seccion.Codigo;
				dto.SeccionNombre = reg.Seccion.Descripcion;
				//dto.PacienteRM = reg.Paciente?.RefPac_NumHCAntiguo.ToString();
				//dto.PacienteNombreCompleto = reg.Paciente?.Persona?.Name;
			}

			ViewBag.Tipo = TipoPrioridad.GetSelectList();
			return View(dto);
		}


		[Can("solicitud_despacho.ver")]
		public async Task<ActionResult> Details(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var reg = await db.SolicitudesDespacho
				.Where(x => x.SolicitudDespachoId == id)
				.Include("Detalle")
				.Include("Detalle.Material")
				.Include("Seccion")
				.Include("Departamento")
				.FirstOrDefaultAsync();
			if (reg == null)
			{
				return HttpNotFound();
			}

			var dto = mapper.Map<SolicitudDespachoDTODetails>(reg);

			return View(dto);
		}


		[Can("solicitud_despacho.ver")]
		public ActionResult DetailsDBF(string id)
		{			
			if (string.IsNullOrEmpty(id))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var dtoRegs = new List<SolicitudDespachoDetalleDTODetailsDBF>();
			id = FixFormatId(id);
			var reg = this.dbfContext.GetSolicitudDespacho(id);
			var regs = this.dbfContext.GetSolicitudDespachoDetalle(id,reg.Fecha.Value.Month.ToString("d2"));
			var dto = new SolicitudDespachoDTODetailsDBF() {
				  NumeroDespacho = reg.NUMDES,
				  //Fecha = reg.Fecha
			};
            foreach (var item in regs)
            {
				dtoRegs.Add( new SolicitudDespachoDetalleDTODetailsDBF() { 
				  MaterialCodigo = item.MATDES,
				  CantidadSolicitada = item.CANSOL??0,
				  CantidadDespachada = item.CANDES??0,
				  UnidadMedida = item.UMEDES
				});
            }
			dto.Detalle = dtoRegs;
			return View(dto);
		}


		public ActionResult PrintDBF(string id) // GET
		{
			id = FixFormatId(id);

			List<SolicitudDespachoDTORpt> regsReport = new List<SolicitudDespachoDTORpt>();
			var def = new List<SolicitudDespachoDBF>();
			var defReg = this.dbfContext.GetSolicitudDespacho(id);
			def.Add(defReg);
			var det = this.dbfContext.GetSolicitudDespachoDetalle(id, defReg.Fecha.Value.Month.ToString("d2"));
			//List<DestinoDBF> des = this.dbfContext.GetDestinos();
			var mat = this.dbfContext.GetMateriales();
			var exi = this.dbfContext.GetExistenciasTotales();


			var joined = from df in def
						 join dt in det on df.NUMDES equals dt.NUMDES
						 //join ex in exi on new { A = dt.MATDES, B = dt.CODIGI } equals new { A = ex.CODIGI, B = ex.CODIGI } 
						 //			 join ds in des on df.CPROVE equals ds.CODIGO
						 join dm in mat on new { A = dt.MATDES, B = dt.CODIGI } equals new { A = dm.CODIGO, B = dm.CODIGI }
						 //			 select new { df, dt, ds, dm };
						 select new { df, dt, dm };

			foreach (var j in joined)
			{
				regsReport.Add(new SolicitudDespachoDTORpt()
				{
					SolicitudDespachoNumero = j.df.NUMDES,
					Fecha = j.df.Fecha ?? DateTime.MinValue,
					CantidadSolicitada = j.dt.CANSOL ?? 0,
					UnidadMedida = j.dt.UMEDES,
					Descripcion = "[" + j.dt.MATDES + "] - " + j.dt.CODIGI + " " + j.dm.DESCRI,
					CantidadExistencia = this.dbfContext.GetExistenciaBodega(j.dt.MATDES, j.dt.CODIGI, j.dt.BODEXI, true)?.CANTI ?? 0,
					Departamento = j.df.DEPDES,
					TipoDespacho = j.df.TIPDES,
					Solicito = j.df.SOLDES,
					Jefe = j.df.JEFDES,
					Gerente = j.df.GERDES,
					Bodega = j.dt.BODEXI,
					NumeroFormatoAutorizado = j.df.NumeroFormatoAutorizado
					//SolicitudNumero = j.df.NUMSOL,
					//Elaboro = j.df.OTRSOL,
					//Jefe = j.df.JEFSOL,
					//Gerente = j.df.GERSOL,
					//Director = j.df.DIRSOL,
					//Cantidad = Convert.ToDecimal(j.dt.CANSOL),
					//UnidadMedida = j.dt.UMESOL,
					//Valor = Convert.ToDecimal(j.dt.VALSOL),
					//Fecha = j.df.Fecha ?? DateTime.MinValue,
					//Descripcion = j.dm.DESCRI,
					//MaterialCodigo = j.dt.MATSOL,
					//SigesCodigo = "" ?? "",
					////        RenglonCodigo = dme.dm.m.RenglonCodigo,
					//Observaciones = j.df.OBSSOL,
					//CPROVE = j.df.CPROVE,
					//DestinoDescripcion = j.ds.DESCRI,

				});
			}


			ReportDocument rd = new ReportDocument();
			rd.Load(Path.Combine(Server.MapPath("~/Reportes/Inventario/rptSolicitudDespacho.rpt")));
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
		public ActionResult JsFilterIndexDBF(string Buscar, int? page)
		{
			int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			var regs = this.dbfContext.GetSolicitudesDespacho();
			if (regs == null)
				return PartialView("_ErrorSiahr");

			if (!String.IsNullOrEmpty(Buscar))
			{
				regs = regs.Where(s =>
				s.NUMDES.Contains(Buscar) ||
				//s.NIT.ToUpper().Contains(Buscar.ToUpper()) ||
				(s.Fecha.HasValue ? s.Fecha.Value.ToString("d") : "").Contains(Buscar)
				);
			}

			regs = regs.ToList();
			var dto = regs.ToPagedList(pageIndex, pageSize);
			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexDBFGrid", dto);
		}


		[HttpPost]
		public JsonResult JsSolicitudAvailable(string serie, string year, string SolicitudDespachoId)
		{
			if (string.IsNullOrEmpty(SolicitudDespachoId))
			{
				serie = year + "-" + serie.PadLeft(6, '0');
				return Json(!db.SolicitudesDespacho.Any(u => u.SolicitudDespachoId == serie));
			}
			else
			{
				return Json(true);
			}
		}


		[Can("solicitud_despacho.editar")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult JsSaveMaster([Bind (Exclude = "")] SolicitudDespachoDTOCEdit dto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
				}
				if (string.IsNullOrEmpty(dto.SolicitudDespachoId))
				{
					dto.SolicitudDespachoId = dto.year + "-" + dto.serie.PadLeft(6, '0');
					var reg = new SolicitudDespacho() { 
						SolicitudDespachoId = dto.SolicitudDespachoId,
						Correlativo = dto.Correlativo,
						Fecha =dto.Fecha,
						Tipo = dto.Tipo,
						DepartamentoId = dto.DepartamentoId,
						SeccionId = dto.SeccionId
					};
					//var reg = mapper.Map<SolicitudDespacho>(dto);
					db.SolicitudesDespacho.Add(reg);
					db.SaveChanges();
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg });
				}
				else
				{
					var reg = db.SolicitudesDespacho.Find(dto.SolicitudDespachoId);
					reg.Correlativo = dto.Correlativo;
					reg.Fecha = dto.Fecha;
					reg.Tipo = dto.Tipo;
					reg.DepartamentoId = dto.DepartamentoId;
					reg.SeccionId = dto.SeccionId;
					//mapper.Map(dto, reg);
					db.SaveChanges();
					return Json(new { success = true, message = "Se ha actualizado corrctamente el Registro", data = reg });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "ha ocurrido algun problema, intente nuevamenete, si el problema persiste comuniquese con el administrador.", messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}
		}



		[Can("solicitud_despacho.editar")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult JsSaveMasterMS([Bind(Exclude = "")] SolicitudDespachoDTOCEditMS dto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
				}
				if (string.IsNullOrEmpty(dto.SolicitudDespachoId))
				{
					// INSERT
					dto.SolicitudDespachoId = dto.year + "-" + dto.serie.PadLeft(6, '0');
					var reg = new SolicitudDespacho()
					{
						SolicitudDespachoId = dto.SolicitudDespachoId,
						Correlativo = dto.Correlativo,
						Fecha = dto.Fecha,
						Tipo = dto.Tipo,
						DepartamentoId = dto.DepartamentoId,
						SeccionId = dto.SeccionId,
						//Material Sala
						//HojaControlSala = dto.HojaControlSala,
						//FechaOperacion =dto.FechaOperacion,
						//Servicio = dto.Servicio,
						//Cama  = dto.Cama,
						//PacienteId = dto.PacienteId,
						//Cirujano = dto.Cirujano,
						//AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista,
						//AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante
					};
					//var reg = mapper.Map<SolicitudDespacho>(dto);
					db.SolicitudesDespacho.Add(reg);
					db.SaveChanges();
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg });
				}
				else
				{
					// UPDATE
					var reg = db.SolicitudesDespacho.Find(dto.SolicitudDespachoId);
					reg.Correlativo = dto.Correlativo;
					reg.Fecha = dto.Fecha;
					reg.Tipo = dto.Tipo;
					reg.DepartamentoId = dto.DepartamentoId;
					reg.SeccionId = dto.SeccionId;
					reg.Observacion = dto.Observacion;
                    // Material Sala
                    //reg.HojaControlSala = dto.HojaControlSala;
                    //reg.FechaOperacion = dto.FechaOperacion;
                    //reg.Servicio = dto.Servicio;
                    //reg.Cama = dto.Cama;
                    //reg.PacienteId = dto.PacienteId;
                    //reg.Cirujano = dto.Cirujano;
                    //reg.AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista;
                    //reg.AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante;
                    db.SaveChanges();
					return Json(new { success = true, message = "Se ha actualizado corrctamente el Registro", data = reg });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "ha ocurrido algun problema, intente nuevamenete, si el problema persiste comuniquese con el administrador.", messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}
		}




		[Can("solicitud_despacho.editar")]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult JsSaveChild( SolicitudDespachoChildDTOCEdit dto)
        {
            try
            {
				if (!ModelState.IsValid)
				{
					return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
				}
				if (dto.SolicitudDespachoDTId == 0)
				{
					// Insert
					//var reg = mapper.Map<SolicitudDespachoDT>(dto);
					var reg = new SolicitudDespachoDetalle()
					{
						MaterialId = dto.MaterialId,
						Cantidad = dto.Cantidad ?? 0,
						SolicitudDespachoId = dto.SolicitudDespachoId
					};
					
					//reg.Material = null;
					db.SolicitudesDespachoDetalle.Add(reg);
					db.SaveChanges();
					return Json(new { success = true, message = Resources.Msg.success_create });
				}
				else
				{
					// Update
					var reg = db.SolicitudesDespachoDetalle.Find(dto.SolicitudDespachoDTId);
					//mapper.Map(dto, reg);
					reg.MaterialId = dto.MaterialId;
					reg.Cantidad = dto.Cantidad ?? 0;
					//reg.Material = null;
					db.SaveChanges();
					return Json(new { success = true, message = Resources.Msg.success_edit });
				}
			}
            catch (Exception ex)
            {
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message });
			}
        }
		

		public ActionResult JsGrid(string id)
		{
			if (string.IsNullOrEmpty(id))
			{ return null; }
			var regs = db.SolicitudesDespachoDetalle.Include("Material").Where(x => x.SolicitudDespachoId == id).ToList();
			var dto = mapper.Map<IEnumerable<SolicitudDespachoDTDTOBase>>(regs);
			return PartialView("_Grid", dto);
		}

		public ActionResult jsGetChild(int? id)
        {
            try
            {
				var reg = db.SolicitudesDespachoDetalle
					.Include("Material")
					.Where(x => x.SolicitudDespachoDTId == id)
					.FirstOrDefault();
				var dto = mapper.Map<SolicitudDespachoChildDTOCEdit>(reg);
				// Special Map
				dto.MaterialCodigo = reg.Material.Codigo;
				dto.MaterialNombre = reg.Material.Descripcion;
				dto.MaterialPrecio = reg.Material.Precio; //??0;

				return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception ex)
            {
				return Json(new { success = false, message = Apphr.Resources.Msg.failure, exmessage = ex.Message, exinner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
			}
			
        }


		[ValidateAntiForgeryToken]
		public async Task<ActionResult> JsDeleteMaster(string id) // POST
		{
			string[] permisosRequeridos = { "solicitud_despacho.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				var reg = await db.SolicitudesDespacho
				.Include("Detalle")
				.Where(x => x.SolicitudDespachoId == id).FirstOrDefaultAsync();
				
				db.SolicitudesDespacho.Remove(reg);
				await db.SaveChangesAsync();
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
			}
		}


		[ValidateAntiForgeryToken]
		public async Task<ActionResult> JsDeleteChild(int? id)
        {
			string[] permisosRequeridos = { "solicitud_despacho.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				var reg = db.SolicitudesDespachoDetalle.Find(id);
				db.SolicitudesDespachoDetalle.Remove(reg);
				await db.SaveChangesAsync();
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
			}
		}
		#endregion

		#region Private
		private string FixFormatId(string id)
		{
			id = id.PadLeft(5);
			return id.Substring(id.Length - 5);
		}
		#endregion
	}
}