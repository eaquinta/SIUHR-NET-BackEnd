using Apphr.Application.Common.DTOs;
using Apphr.Application.DespachosInventario.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Inventario.Controllers
{
    [Authorize]
    public class DespachosInventarioController : DbController
    {
        private string IdAutoNum = "DespachoInventario";
		private MaterialRepository MaterialRep;
		private SolicitudDespachoRepository SolicitudDespachoRep;
		private BodegaRepository BodegaRep;
		private SolicitudMaterialSalaRepository SolicitudMaterialSalaRep;
		private DespachoInventarioRepository DespachoInventarioRep;

		public DespachosInventarioController()
        {
			MaterialRep = new MaterialRepository(db);
			SolicitudDespachoRep = new SolicitudDespachoRepository(db);
			BodegaRep = new BodegaRepository(db);
			SolicitudMaterialSalaRep = new SolicitudMaterialSalaRepository(db);
			DespachoInventarioRep = new DespachoInventarioRepository(db);
        }

		[Can("despacho_inventario.ver")]
		public ActionResult Index( DespachoInventarioDTOIndex dto, int? page) //GET
		{
			IQueryable<DespachoInventario> regs;

			int pageIndex = 1;
			if (dto?.F == null) dto.F = new DespachoInventarioDTOIxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			//            DespachoInventarioDTOIndex dto = new DespachoInventarioDTOIndex();


			regs = (from p in db.DespachosInventario select p);
			if (dto.F != null)
			{
				if (!string.IsNullOrEmpty(dto.F.Buscar))
					regs = regs.Where(x => x.DespachoInventarioId.Contains(dto.F.Buscar)); //|| x.DestinoId.Contains(dto.F.Buscar));
			}

			regs = regs
				//.Include(x => x.Bodega)
				.Include(x => x.Departamento)
				.OrderBy(x => x.DespachoInventarioId);

			var rows = mapper.Map<List<DespachoInventarioDTOIxRow>>(regs.ToList());
			dto.Result = (PagedList<DespachoInventarioDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;

			return View(dto);
		}

		[Can("despacho_inventario.editar")]
		public async Task<ActionResult> CEdit(string id)  //GET
		{
			var dto = new DespachoInventarioDTOCEdit();
			if (!string.IsNullOrEmpty(id))
			{
				// Update
				var reg = await db.DespachosInventario
					 //.Include("Bodega")
					 .Include("Departamento")
					 .Where(x => x.DespachoInventarioId == id)
					 .FirstOrDefaultAsync();
				if (reg == null)
				{
					return HttpNotFound();
				}

				dto = mapper.Map<DespachoInventarioDTOCEdit>(reg);
				// Special Map
				dto.DepartamentoNombre = reg.Departamento.Descripcion;
				dto.DepartamentoCodigo = reg.Departamento.Codigo;
				dto.Child.DespachoInventarioId = dto.DespachoInventarioId;
			}
			else
			{
				// Insert
				dto.FechaDocumentoReferencia = DateTime.Now;
				dto.Fecha = DateTime.Now;
			}

			ViewBag.TipoDocumentoReferencia = new List<SelectListItem>() {
				new SelectListItem { Text = "Solicitud Despacho", Value = "SD" },
				new SelectListItem { Text = "Solicitud Maeteriales Sala", Value = "SM" },
			};

			return View(dto);
		}
		
		[Can("despacho_inventario.ver")]
		public async Task<ActionResult> Details(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var reg = await db.DespachosInventario
				.Include(i => i.Departamento)
				//.Include(i => i.Bodega)
				.Include(i => i.Detalle)
				.Include("Detalle.Material")
				.Where(x => x.DespachoInventarioId == id)
				.FirstOrDefaultAsync();
			if (reg == null)
			{
				return HttpNotFound();
			}

			var dto = mapper.Map<DespachoInventarioDTODetails>(reg);

			return View(dto);
		}

		

        #region Json
		
		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveMaster(DespachoInventarioDTOCEdit dto)
		{
            string[] permisosRequeridos = { "despacho_inventario.editar" };
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
				if (string.IsNullOrEmpty(dto.DespachoInventarioId))
				{
					// INSERT
					var reg = await DespachoInventarioRep.InsertMaster(dto, IdAutoNum);
					
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
				}
				else
				{
					// UPDATE
					var reg = await DespachoInventarioRep.UpdateMaster(dto);
					
					return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveChild(DespachoInventarioDetalleDTOCEdit dto)
        {
            string[] permisosRequeridos = { "despacho_inventario.editar" };
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
                if (dto.DespachoInventarioDetalleId == 0)
                {
                    // INSERT
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Actualizacion Master
                            var masterReg = await db.DespachosInventario.FindAsync(dto.DespachoInventarioId);
                            masterReg.Lineas += 1;
                            // Crear Movimiento Inventario
                            var movInv = new MovimientoInventario()
                            {
                                DocumentoReferencia = dto.DocumentoReferencia,
								Documento = dto.DespachoInventarioId,
                                MaterialId = dto.MaterialId,
                                BodegaId = dto.BodegaId,
                                Lote = dto.Lote,
                                FechaVencimiento = dto.FechaVencimiento,
                                Fecha = dto.Fecha,
                                ProveedorId = dto.ProveedorId,
                                Observacion = dto.Observacion,
								TipoMovimientoInventarioId = 2,
								DestinoId = dto.DestinoId,
								Line = masterReg.Lineas,
                                Efecto = -1,
                                Cantidad = dto.Cantidad,
                                Precio = dto.MaterialPrecio,
                                Valor = dto.MaterialPrecio * dto.Cantidad,
								PacienteId = dto.PacienteId,
							};
                            // Crear Child 
                            var childReg = new DespachoInventarioDetalle()
                            {
                                DespachoInventarioId = dto.DespachoInventarioId,
                                Linea = masterReg.Lineas,
                                MaterialId = dto.MaterialId,
                                Cantidad = dto.Cantidad,
                                Precio = dto.MaterialPrecio,
                                Valor = dto.Cantidad * dto.MaterialPrecio,
                                ProveedorId = dto.ProveedorId,
                                FechaVencimiento = dto.FechaVencimiento,
                                MovimientoInventario = movInv,
								BodegaId = dto.BodegaId,
								PacienteId = dto.PacienteId
                            };

                            db.DespachosInventarioDetalle.Add(childReg);
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
                    var reg = await db.DespachosInventarioDetalle
                        .Where(x => x.DespachoInventarioDetalleId == dto.DespachoInventarioDetalleId)
                        .Include("MovimientoInventario")
                        .FirstOrDefaultAsync();
                    reg.MaterialId = dto.MaterialId;
                    reg.Cantidad = dto.Cantidad;
                    reg.Precio = dto.MaterialPrecio;
                    reg.Valor = dto.MaterialPrecio * dto.Cantidad;
                    reg.ProveedorId = dto.ProveedorId;
                    reg.FechaVencimiento = dto.FechaVencimiento;
                    reg.Lote = dto.Lote;
                    reg.Observacion = dto.Observacion;
					reg.PacienteId = dto.PacienteId;
					reg.BodegaId = dto.BodegaId;
					// Movmiento Inventario
					reg.MovimientoInventario.DocumentoReferencia = dto.DocumentoReferencia;
                    reg.MovimientoInventario.MaterialId = dto.MaterialId;
                    reg.MovimientoInventario.BodegaId = dto.BodegaId;
                    reg.MovimientoInventario.PacienteId = dto.PacienteId;
                    reg.MovimientoInventario.DestinoId = dto.DestinoId;
                    reg.MovimientoInventario.FechaVencimiento = dto.FechaVencimiento;
                    reg.MovimientoInventario.Fecha = dto.Fecha;
                    reg.MovimientoInventario.ProveedorId = dto.ProveedorId;
                    reg.MovimientoInventario.Observacion = dto.Observacion;
                    reg.MovimientoInventario.Cantidad = dto.Cantidad;
                    reg.MovimientoInventario.Precio = dto.MaterialPrecio;
                    reg.MovimientoInventario.Valor = dto.MaterialPrecio * dto.Cantidad;
					reg.MovimientoInventario.BodegaId = dto.BodegaId;

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
		public async Task<ActionResult> JsDeleteMaster(string id) // POST
		{
            string[] permisosRequeridos = { "despacho_inventario.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
			{
				await DespachoInventarioRep.DeleteMaster(id);
				return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}


		[ValidateAntiForgeryToken]
		public async Task<ActionResult> JsDeleteChild(int? id)
		{
            string[] permisosRequeridos = { "despacho_inventario.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
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
						var reg = await db.DespachosInventarioDetalle.Where(x => x.DespachoInventarioDetalleId == id).FirstOrDefaultAsync();
						var mov = await db.MovimientosInvnentario.FindAsync(reg.MovimientoInventarioId);
						db.MovimientosInvnentario.Remove(mov);
						db.DespachosInventarioDetalle.Remove(reg);
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
				return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.DenyGet);
			}
		}

		public async Task<ActionResult> JsGetMaterialDIByCodigo(string SolicitudDespachoId, string MaterialCodigo, int BodegaId)
		{
			var mat = await MaterialRep.GetMaterialByCodigoAsync(MaterialCodigo);
			var dto = new
			{
				MaterialId = mat.MaterialId,
				MaterialNombre = mat.Descripcion,
				MaterialUM = mat.UnidadMedida,
				MaterialPrecio = mat.Precio,
				MaterialExistencia = MaterialRep.GetMateialExistencia( mat.MaterialId, BodegaId),
				MaterialMinimo = mat.Minimo,
				MaterialSolicitado = SolicitudDespachoRep.GetCantidadSolicitada(mat.MaterialId, SolicitudDespachoId)
			};
			return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult JsGetChild(int? id1, int? id2, string id3)
		// id1 = ChildId    id2 = BodegaId   id3 = DocumentoReferencia
		{
			try
			{
				var reg = db.DespachosInventarioDetalle
					.Include("Material")
					.Include("Proveedor")
					.Include("Paciente")
					.Include("Paciente.Persona")
					.Where(x => x.DespachoInventarioDetalleId == id1)
					.FirstOrDefault();
				var Existencia = MaterialRep.GetMateialExistencia(reg.MaterialId, id2) + reg.Cantidad;
				var regMaster = db.DespachosInventario.Find(reg.DespachoInventarioId);				
				var Solicitado = SolicitudDespachoRep.GetCantidadSolicitada(reg.MaterialId, regMaster.DocumentoReferencia);
				var dto = mapper.Map<DespachoInventarioDetalleDTOCEdit>(reg);
				// Special Map
				dto.MaterialCodigo = reg.Material.Codigo;
				dto.MaterialNombre = reg.Material.Descripcion;
				dto.MaterialPrecio = reg.Material.Precio; // ?? 0;
				dto.ProveedorNit = reg.Proveedor?.Nit ?? "";
				dto.ProveedorNombre = reg.Proveedor?.Descripcion ?? "";
				dto.PacienteRM =  reg.Paciente?.RefPac_NumHCAntiguo.ToString();
				dto.PacienteNombreCompleto = reg.Paciente?.Persona?.Name;
				dto.MaterialExistencia = Existencia;
				dto.MaterialSolicitado = Solicitado;

				return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Apphr.Resources.Msg.failure, exmessage = ex.Message, exinner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
			}

		}

		public ActionResult JsGrid(string id)
		{
			if (string.IsNullOrEmpty(id))
			{ return null; }
			var regs = db.DespachosInventarioDetalle.Include("Material").Where(x => x.DespachoInventarioId == id).ToList();
			var dto = mapper.Map<IEnumerable<DespachoInventarioDetalleDTOBase>>(regs);
			return PartialView("_Grid", dto);
		}

		[HttpPost]
		public JsonResult JsValidateDocumentRef(string Value, string Mode, string TipoDocumentoReferencia)
		{
			if ( Mode != "INS")
            {
				return Json(true);
            }
			if (string.IsNullOrEmpty(Value))
			{
				Value = Request.Params[0];
			}
			if (string.IsNullOrEmpty(TipoDocumentoReferencia))
			{
				return Json(false);
			}
			switch (TipoDocumentoReferencia)
			{
				case "SD": // Solicitud Despacho
					return Json(db.SolicitudesDespacho.Any(u => u.SolicitudDespachoId == Value && u.DespachoInventarioId == null));
				case "SM": // Solicitud Material Sala
                    return Json(db.SolicitudMaterialesSala.Any(u => u.SolicitudMaterialSalaId.ToString() == Value && u.DespachoInventarioId == null));
                default:
					return Json(false);
			}
		}


		public async Task<ActionResult> JsGetMaterialData(string MaterialCodigo, int? BodegaId, string DocRef, string DocTipo)
		{
			try
			{
				object dto = null;
				Paciente pac = null;
				if (string.IsNullOrEmpty(MaterialCodigo) || BodegaId == null || string.IsNullOrEmpty(DocRef) || string.IsNullOrEmpty(DocTipo))
					throw new ArgumentException("Erorr en parametros");
				var mat = await MaterialRep.GetMaterialByCodigoAsync(MaterialCodigo);
				var exi = BodegaRep.GetExistencia(BodegaId??0, mat?.MaterialId??0);
				decimal sol;
				if (DocTipo == "SD")
					sol = SolicitudDespachoRep.GetCantidadSolicitada(mat.MaterialId, DocRef);
				else
				{
					sol = SolicitudMaterialSalaRep.GetCantidadSolicitada(mat?.MaterialId, DocRef);
					pac = await SolicitudMaterialSalaRep.GetPacienteInfo(DocRef);
				}

				if (mat != null)
				{
					dto = new
					{
						MaterialId = mat.MaterialId,
						MaterialNombre = mat.Descripcion,
						MaterialUM = mat.UnidadMedida,
						MaterialPrecio = mat.Precio,
						MaterialExistencia = exi,
						MaterialMinimo = mat.Minimo??0,
						MaterialSolicitado = sol,
						PacienteId = pac?.PacienteId??0,
						PacienteRM = pac?.RefPac_NumHCAntiguo??0,
						PacienteNombreCompleto = pac?.Persona.Name
					};
				}

				return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Apphr.Resources.Msg.failure, exmessage = ex.Message, exinner = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult JsGetMaterialesInSDByFilter(string filter, string document)
		{
			var result = new List<DTOAutocompleteItem>();
			if (!string.IsNullOrEmpty(filter))
			{
				result = (from sd in db.SolicitudesDespachoDetalle
						  join mt in db.Materiales on sd.MaterialId equals mt.MaterialId
						  where sd.SolicitudDespachoId.Equals(document) || mt.Codigo.Contains(filter) || mt.Descripcion.Contains(filter)
						  select new DTOAutocompleteItem()
						  {
							  id = mt.Codigo,
							  text = mt.Descripcion
						  }).ToList();
			}
			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult JsGetMaterialesInSMByFilter(string filter, string document)
		{
			var result = new List<DTOAutocompleteItem>();
			if (!string.IsNullOrEmpty(filter))
			{
				result = (from sd in db.SolicitudMaterialesSalaDetalle
						  join mt in db.Materiales on sd.MaterialId equals mt.MaterialId
						  where sd.SolicitudMaterialSalaId.Equals(document) || mt.Codigo.Contains(filter) || mt.Descripcion.Contains(filter)
						  select new DTOAutocompleteItem()
						  {
							  id = mt.Codigo,
							  text = mt.Descripcion
						  }).ToList();
			}
			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}