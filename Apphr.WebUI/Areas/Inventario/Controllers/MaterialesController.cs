using Apphr.Application.Materiales.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.EntitiesDBF;
using Apphr.Domain.Enums;
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
    public class MaterialDTOSelectItem
    {
		public string id { get; set; }
		public string text { get; set; }
    }

	[Authorize]
	[LogAction]
	public class MaterialesController : DbController
    {
		private MaterialRepository MaterialRep;
        public MaterialesController()
        {
			MaterialRep = new MaterialRepository(db);
        }
		public ActionResult IndexDBF(MaterialDTOIndexDBF dto, string currentFilter, string searchString, int? page) // GET
		{
			int pageIndex = 1;
			if (dto?.F == null) dto.F = new Application.Common.IxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

			var regs = this.dbfContext.GetMateriales();

			//DataTable dt = this.dbfContext.GetDataSQL("SELECT * FROM SOLENC.dbf ORDER BY NUMSOL");			

			//var regs = (from DataRow dr in dt.Rows
			//			select new SolicitudPedidoDefinicionDBF()
			//			{
			//				NUMSOL = dr["NUMSOL"].ToString(),
			//				CORSOL = dr["CORSOL"].ToString(),
			//				DIASOL = Convert.ToInt32(dr["DIASOL"].ToString()),
			//				MESSOL = Convert.ToInt32(dr["MESSOL"].ToString()),
			//				ANOSOL = Convert.ToInt32(dr["ANOSOL"].ToString()),
			//				DEPSOL = dr["DEPSOL"].ToString(),
			//			});

			if (!String.IsNullOrEmpty(dto?.F?.Buscar))
			{
				regs = regs.Where(s =>
				s.CODIGO.Contains(dto?.F?.Buscar) ||
				s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
				).ToList();
			}
			
			dto.Result = regs.ToPagedList(pageIndex, pageSize);
			ViewBag.PLROpions = PagedListOptions;
			return View(dto);

		}
		[AppAuthorization(Permit.View)]
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

		[AppAuthorization(Permit.View)]
		public ActionResult Index(MaterialDTOIndex dto, int? page) //GET
		{
			IQueryable<Material> regs;

			int pageIndex = 1;
			if (dto?.F == null) dto.F = new MaterialDTOIxFilter();
			if (dto.F.Buscar != dto.F._Buscar)
			{
				page = 1;
				dto.F._Buscar = dto.F.Buscar;
			}

			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			//            MaterialDTOIndex dto = new MaterialDTOIndex();


			regs = (from p in db.Materiales select p);
			if (dto.F != null)
			{
				if (!string.IsNullOrEmpty(dto.F.Buscar))
					regs = regs.Where(x => x.Codigo.Contains(dto.F.Buscar) || x.Descripcion.Contains(dto.F.Buscar));
			}

			regs = regs.OrderBy(x => x.Codigo);

			var rows = mapper.Map<List<MaterialDTOIxRow>>(regs.ToList());
			dto.Result = (PagedList<MaterialDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);
			return View(dto);
		}

		[AppAuthorization(Permit.Edit)]
		public ActionResult Create()  //GET
		{
			return View();
		}

		[AppAuthorization(Permit.Edit)]
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

		[AppAuthorization(Permit.View)]
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

			MaterialDTODetails dto = mapper.Map<MaterialDTODetails>(reg);

			return View(dto);
		}

		[AppAuthorization(Permit.Edit)]
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

		[AppAuthorization(Permit.Edit)]
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

			var dto = mapper.Map<MaterialDTODetails>(reg);
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



		//public ActionResult FiltradoMaterialesDBFById(string cat)
		//{
		//	var result = new List<MaterialDBF>();
		//	// to do : Load the list of products to result
		//	//return Json(result,JsonRequestBehavior.AllowGet); 
		//	return PartialView("_productsFilteredPartial", result);
		//}


		#region Js
		public JsonResult JsGetMateriales()
		{
			List<MaterialDBF> result = dbfContext.GetMateriales();
			result = result.Take(100).ToList();
			//var customer = db.Customers.ToList();
			return Json(new { data = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult JsGetMaterialesByFilter(string id)
        {			
			var result = new List<MaterialDTOSelectItem>();
			if (!string.IsNullOrEmpty(id))
			{
				 result = db.Materiales.Where(x => x.Codigo.Contains(id) || x.Descripcion.Contains(id))
					.Take(autoCompleteSize)
					.Select(p => new MaterialDTOSelectItem { id = p.Codigo, text = p.Descripcion})
					.ToList();					
			}
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);         
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
			//return Json(false);
		}
		
		public async Task<JsonResult> JsGetMaterialByCodigo(string id, int? BodegaId)
		{
			//decimal Existencia;
            try
            {
				var reg = await MaterialRep.GetMaterialByCodigoAsync(id);   //db.Materiales.Where(x => x.Codigo == id).FirstOrDefault();
                if (reg == null)
                {
					throw new ArgumentException("");
                }
				
				//Existencia = MaterialRep.GetMateialExistencia(reg.MaterialId, BodegaId);

				//var dto = mapper.Map<MaterialDTOBaseExt>(reg);
				// aditional data
				//dto.Existencia = Existencia;

				return Json(new { success = true, data = reg }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception ex)
            {
                return Json(new { success = false, exeptionmessage = ex.Message, innerexeption = ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

		public JsonResult JsGetMaterialById(int id)
		{
            try
            {
				var reg = db.Materiales.Find(id);
				return Json(new { result = true, data = reg }, JsonRequestBehavior.AllowGet);
			}
            catch (Exception)
            {
				return Json(new { result = false }, JsonRequestBehavior.AllowGet);
			}
			
		}

		public JsonResult JsSyncMaterial(int id)
		{
            try
            {
				var reg = db.Materiales.Find(id);
				var regDbf = dbfContext.GetMaterial(reg.Codigo).FirstOrDefault();
				mapper.Map(regDbf, reg);
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
				{
					Material reg = mapper.Map<Material>(MaterialDBF);
					db.Materiales.Add(reg);
				}
				else
				{
					var reg = db.Materiales.Where(x => x.Codigo == MaterialDBF.CODIGO).FirstOrDefault();
					if (reg != null)
					{
						mapper.Map(MaterialDBF, reg);
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

	}
}