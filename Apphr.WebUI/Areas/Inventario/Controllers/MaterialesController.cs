using Apphr.Application.Common;
using Apphr.Application.Common.DTOs;
using Apphr.Application.Materiales.DTOs;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models.Entities;
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
	[LogAction]
	public class MaterialesController : DbController
    {
		private MaterialRepository MaterialRep;
        public MaterialesController()
        {
			MaterialRep = new MaterialRepository(db);
        }

		[Can("material.ver")]
		public ActionResult IndexDBF(MaterialDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
		{
			int pageIndex = 1;
			if (dto?.F == null) dto.F = new IxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

			var regs = this.dbfContext.GetMateriales();

            if (regs == null)
            {
				return View("ErrorSiahr");
            }

			if (!String.IsNullOrEmpty(dto?.F?.Buscar))
			{
				regs = regs.Where(s =>
				s.CODIGO.Contains(dto?.F?.Buscar) ||
				s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
				).ToList();
			}
			
			dto.Result = regs.ToPagedList(pageIndex, pageSize);
			ViewBag.PLROpions = PagedListOptions;			
			ViewBag.Anio = dbfContext.GetYear();
			return View(dto);

		}
		[Can("material.ver")]
		public ActionResult DetailsDBF(string id)
        {			
            if (string.IsNullOrEmpty(id))
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var dto = this.dbfContext.GetMaterial(id).FirstOrDefault();
			if(dto == null)
			{
				return HttpNotFound();
			}
			return View(dto);
        }

		[Can("material.ver")]
		public ActionResult Index()			//GET
		{
			ViewBag.Permissions = Utilidades.GetCans(userId);
			return View();
		}

		[Can("material.editar")]
		public ActionResult Create()  //GET
		{
			return View();
		}

		[Can("material.editar")]
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Exclude = "MaterialId")] MaterialDTOCreate dto) //POST
		{
			if (ModelState.IsValid)
			{
				Material reg = new Material();

				mapper.Map(dto, reg);

				db.Materiales.Add(reg);
				await db.SaveChangesAsync();

				return RedirectToAction("Index");

			}
			return View(dto);
		}

		[Can("material.ver")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Material reg = await db.Materiales.FindAsync(id);
			if (reg == null)
			{
				return HttpNotFound();
			}

			MaterialDTOView dto = mapper.Map<MaterialDTOView>(reg);

			return View(dto);
		}

		public async Task<ActionResult> JsDetails(int? id) // GET
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Material reg = await db.Materiales.FindAsync(id);
			if (reg == null)
			{
				return HttpNotFound();
			}

			MaterialDTOView dto = mapper.Map<MaterialDTOView>(reg);

			return PartialView("_DFields", dto);
		}





		[Can("material.editar")]
		public async Task<ActionResult> Edit(int? id) // GET
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Material reg = await db.Materiales.FindAsync(id);
			if (reg == null)
			{
				return HttpNotFound();
			}

			MaterialDTOEdit dto = mapper.Map<MaterialDTOEdit>(reg);
			return View(dto);
		}

		[Can("material.editar")]
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Exclude = "")] MaterialDTOEdit dto) // POST
		{
			if (ModelState.IsValid)
			{
				var reg = mapper.Map<Material>(dto);
				db.Entry(reg).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Details", new { id = dto.MaterialId, Toast = "success.edit" });
			}
			return View(dto);
		}

		[AppAuthorization(Permit.Delete)]
		public async Task<ActionResult> Delete(int? id) //GET
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var reg = await db.Materiales.FindAsync(id);
			if (reg == null)
			{
				return HttpNotFound();
			}

			var dto = mapper.Map<MaterialDTOView>(reg);
			return View(dto);
		}

		[AppAuthorization(Permit.Delete)]

		[HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(int id) // POST
		{
			var reg = await db.Materiales.FindAsync(id);
			db.Materiales.Remove(reg);
			await db.SaveChangesAsync();
			return RedirectToAction("Index", new { Toast = "success.delete" });
		}

		public ActionResult ImportMateriales(bool? update)
		{
            if (update == null)
            {
				update = false;
            }
			dbfContext.SetYear(DateTime.Now.Year);
			var dbf = dbfContext.GetMateriales().ToList();
			//db.Database.ExecuteSqlCommand($"DELETE FROM Materiales");
			//db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT ('[dbo].[Materiales]', RESEED, 0)");
			foreach (var item in dbf)
			{				
				try
				{
					if (!db.Materiales.Any(x => x.Codigo == item.CODIGO && x.Codigi == item.CODIGI))
					{
						var reg = new Material()
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
						reg.SetRenglon();
						reg.SetGrupo();
						db.Materiales.Add(reg);					
					}
                    else
                    {
						if (update.Value)
						{
							var upd = db.Materiales.Where(x => x.Codigo == item.CODIGO && x.Codigi == item.CODIGI).FirstOrDefault();
							//upd.Codigo = item.CODIGO;
							//upd.Codigi = item.CODIGI;
							upd.Descripcion = item.DESCRI;
							upd.UnidadMedida = item.UNIMED;
							upd.Producto = item.PRODUC;
							upd.Precio = item.PRECIO ?? 0;
							upd.Minimo = item.MINIMO;
							upd.Maximo = item.MAXIMO;
							upd.Ojo = item.OJO;
							upd.AlternoDe = item.ALTERNODE;
							upd.Bres = item.BRES;
							upd.FechaCreacion = item.FechaCreacion ?? DateTime.MinValue;
							upd.Usuario = item.USUA;
							upd.Estatus = item.STATUS;
							upd.UsoBodega = item.USOBOD;
							upd.VigenciaDe = item.VigenciaDe;
							upd.VigenciaA = item.VigenciaA;
							upd.UsoServicio = item.USOSER;
						}
					}
					db.SaveChanges();
				}
				catch (Exception)
				{
					Console.Write(item.CODIGO);
					//throw;
				}

			}
			ViewBag.Registros = db.Materiales.Count();
			return View();
		}


		#region DBF
		public JsonResult JsGetMaterialesFilterDBF(string f, string tipo, int? anio)
		{
			Object res = null;

			dbfContext.SetYear(anio);
			var regs = dbfContext.GetMateriales(f);

			if (!string.IsNullOrEmpty(f))
				regs = regs.Where(x => x.CODIGO.ToUpper().Contains(f.ToUpper()) || x.DESCRI.ToUpper().Contains(f.ToUpper()));

			regs = regs.Take(autoCompleteSize);

			if (tipo == "AC")
			{
				res = regs.Select(p => new DTOAutocompleteItem { id = p.CODIGO, text = p.DESCRI })
					.ToList();
			}
			if (tipo == "S")
			{
				res = regs.Select(s => new DTOSelect2
				{
					id = s.CODIGO,
					text = s.DESCRI,
					html = "<div style=\"font-weight: bold;\">" + s.CODIGO + "</div><div style=\"font-size: 0.75em;\">" + s.DESCRI + "</div>"
				})
				.ToList();
			}

			return Json(new { data = res }, JsonRequestBehavior.AllowGet);
		}
		#endregion



		//public ActionResult FiltradoMaterialesDBFById(string cat)
		//{
		//	var result = new List<MaterialDBF>();
		//	// to do : Load the list of products to result
		//	//return Json(result,JsonRequestBehavior.AllowGet); 
		//	return PartialView("_productsFilteredPartial", result);
		//}


		#region Js

		[ValidateAntiForgeryToken]
		public ActionResult JsFilterIndex(string Buscar, int? page)     // GET
		{			
			IQueryable<Material> regs;
			int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;			
			
			regs = (from p in db.Materiales select p);

			if (Buscar != null)
			{
				if (!string.IsNullOrEmpty(Buscar))
					regs = regs.Where(x => x.Codigo.Contains(Buscar) || x.Descripcion.Contains(Buscar));
			}

			regs = regs.OrderBy(x => x.Codigo);

			var rows = mapper.Map<List<MaterialDTOIxRow>>(regs.ToList());
			var dto = (PagedList<MaterialDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);

			ViewBag.PLROpions = PagedListOptions;
			return PartialView("_IndexGrid", dto);
		}


		public JsonResult JsGetMateriales()
		{
			List<MaterialDBF> result = dbfContext.GetMateriales();
			result = result.Take(100).ToList();
			//var customer = db.Customers.ToList();
			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult JsGetByFilter(string f, string tipo = "AC")
        {
			Object res = null;
			var result = from r in db.Materiales select r;
						
			if (!string.IsNullOrEmpty(f))
				result = result.Where(x => x.Codigo.Contains(f) || x.Descripcion.Contains(f));

			result = result.Take(autoCompleteSize);

			if (tipo == "AC")
			{
				res = result.Select(p => new DTOAutocompleteItem { id = p.Codigo, text = p.Descripcion })
					.ToList();
			}
			if (tipo == "S")
			{
				res = result.Select(s => new DTOSelect2
				{
					id = s.MaterialId.ToString(),
					text = s.Codigo + " (" +s.Codigi+")",
					html = "<div style=\"font-weight: bold;\">" + s.Codigo + " (" + s.Codigi + ")</div><div style=\"font-size: 0.75em;\">" + s.Descripcion + "</div>"
				})
				.ToList();
			}	
				
            return Json(new { data = res }, JsonRequestBehavior.AllowGet);         
		}

		

		public JsonResult JsGetMaterialesLikeId (string id)
        {
			
			List<MaterialDBF> result = null;
			if(!string.IsNullOrEmpty(id))
            {
				result = dbfContext.GetMateriales().Where(x => x.CODIGO.Contains(id)).Take(25).ToList();				
			}		

			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult JsGetMaterial(string id)
		{
			List<MaterialDBF> result;
			if (string.IsNullOrEmpty(id))
            {
				result = null;
			}
            else
            {
				result = dbfContext.GetMaterial(id);
				result = result.Take(100).ToList();
			}
			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		
		[HttpPost]
		public JsonResult JsMaterialCodigoExist(string codigo)
		{
			if (string.IsNullOrEmpty(codigo))
			{
				codigo = Request.Params[0];
			}
			var res = db.Materiales.Where(x => x.Codigo == codigo).Any();
			return Json(res);			
		}
		
		public async Task<JsonResult> JsGetByCodigo(string id, int? BodegaId)
		{
			//decimal Existencia;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
					return null;
				}
				var reg = await MaterialRep.GetMaterialByCodigoAsync(id);   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
					return null;
					//throw new NullReferenceException("El Material es null.");
				}
				

				return Json(new { success = true, data = reg }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

		public async Task<JsonResult> JsGetById(int id)
		{
            try
            {
				var reg = await db.Materiales.FindAsync(id);
				return Json(new { success = true, result = true, data = reg }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception)
            {
				return Json(new { success = false, result = false }, JsonRequestBehavior.AllowGet);
			}
			
		}

		public JsonResult JsSyncMaterial(int id)
		{
            try
            {
				var reg = db.Materiales.Find(id);
				var regDbf = dbfContext.GetMaterial(reg.Codigo).FirstOrDefault();
				mapper.Map(regDbf, reg);

				if (string.IsNullOrEmpty(reg.UnidadMedida))
					reg.UnidadMedida = "-ND-";

				db.SaveChanges();
				return Json(new { result = true }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception)
            {
				return Json(new { result = false }, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult JsImportMaterial(string CODIGO)
		{
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
					throw new ArgumentException("Parametro CODIGO de contener algun valor");
				}
				var MaterialDBF = dbfContext.GetMaterial(CODIGO).FirstOrDefault();
				if (!db.Materiales.Any(x => x.Codigo == MaterialDBF.CODIGO))
				{  // INSERT
					Material reg = mapper.Map<Material>(MaterialDBF);

					if (string.IsNullOrEmpty(reg.UnidadMedida))
						reg.UnidadMedida = "-ND-";

					db.Materiales.Add(reg);
				}
				else
				{ // UPDATE
					var reg = db.Materiales.Where(x => x.Codigo == MaterialDBF.CODIGO).FirstOrDefault();
					if (reg != null)
					{
						mapper.Map(MaterialDBF, reg);

						if (string.IsNullOrEmpty(reg.UnidadMedida))
							reg.UnidadMedida = "-ND-";
					}
				}
				db.SaveChanges();
				return Json(new { result = true }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception)
            {
				return Json(new { result = false }, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion




		public async  Task<ActionResult> JsGetCEditForm(int? id)
		{
			string[] permisosRequeridos = { "material.editar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
			}
            if (id == null)
            {
				return PartialView("_CEditMaster", new MaterialDTOCEdit { MaterialId = 0 });

			}
			var reg = db.Materiales.Where(x => x.MaterialId == id).FirstOrDefault();
            if (reg == null)
            {
				return PartialView("_RegisterNotFound");
			}
			var dto = new MaterialDTOCEdit()
			{
				MaterialId = reg.MaterialId,
				Codigo = reg.Codigo,
				Descripcion = reg.Descripcion,
				Precio = reg.Precio,
				Minimo = reg.Minimo,
				UnidadMedida = reg.UnidadMedida,
				SigesCodigo = reg.SigesCodigo,
			};
			return PartialView("_CEditMaster", dto);
		}

		[ValidateAntiForgeryToken]
		public async Task<JsonResult> JsSaveMaster(MaterialDTOCEdit dto)               // POST 
		{
			List<string> ListPermit = new List<string>();

			if (dto.MaterialId == 0)
				ListPermit.Add("mateiral.crear");
			else
				ListPermit.Add("mateiral.editar");

			bool hasPermit = await Utilidades.Can(ListPermit.ToArray(), userId);
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
				if (dto.MaterialId == 0)
				{
					// INSERT
					// Validación Adicional
					//if (db.ORTSolicitudesPedido.Any(x => x.Anio == dto.Fecha.Year && x.Numero == dto.Numero))
					//{
					//    return Json(new { success = false, message = "Esta Solicitud de Pedido ya esta registrada." });
					//}

					var reg = new Material()
					{
						Codigo = dto.Codigo,
						Descripcion = dto.Descripcion,
						UnidadMedida = dto.UnidadMedida,
						Precio = dto.Precio ?? 0,
						Minimo = dto.Minimo,
						SigesCodigo = dto.SigesCodigo
					};
					reg.SetGrupo();
					reg.SetRenglon();

					db.Materiales.Add(reg);
					await db.SaveChangesAsync();
					return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
				}
				else
				{
					// UPDATE
					var reg = await db.Materiales
						.Where(x => x.MaterialId == dto.MaterialId)
						.FirstOrDefaultAsync();

					reg.Codigo = dto.Codigo;
					reg.Descripcion = dto.Descripcion;
					reg.UnidadMedida = dto.UnidadMedida;
					reg.Precio = dto.Precio ?? 0;
					reg.Minimo = dto.Minimo;
					reg.SigesCodigo = dto.SigesCodigo;

					await db.SaveChangesAsync();
					return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
			}
		}

		public async Task<ActionResult> JsViewMaster(int? id)                                 // GET 
		{
			var reg = await db.Materiales
				.Where(x => x.MaterialId == id)
				.FirstOrDefaultAsync();

			var dto = new MaterialDTOView()
			{
				 Codigo = reg.Codigo,
				 Descripcion = reg.Descripcion,
				 UnidadMedida = reg.UnidadMedida,
				 Precio = reg.Precio,
				 Minimo = reg.Minimo,
				 SigesCodigo = reg.SigesCodigo,
			};

			return PartialView("_ViewMaster", dto);
		}

		public async Task<JsonResult> JsDeleteMaster(int id)                            // POST 
		{
			string[] permisosRequeridos = { "material.eliminar" };
			bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
			if (!hasPermit)
			{
				return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
			}
			try
			{
				using (DbContextTransaction t = db.Database.BeginTransaction())
				{
					try
					{
						var reg = await db.Materiales
							.Where(x => x.MaterialId == id)
							.FirstOrDefaultAsync();


						db.Materiales.Remove(reg);

						await db.SaveChangesAsync();
						t.Commit();
					}
					catch (Exception)
					{
						t.Rollback();
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

	}
}