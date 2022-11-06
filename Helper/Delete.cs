 [AppAuthorization(Permit.Delete)]
public async Task<ActionResult> Delete(int? id) //GET
{
	if (id == null)
	{
		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	}
	var reg = await db.Bodegas.FindAsync(id);
	if (reg == null)
	{
		return HttpNotFound();
	}

	var dto = mapper.Map<BodegaDTODetails>(reg);
	return View(dto);
}

[HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]        
public async Task<ActionResult> DeleteConfirmed(int id) // POST
{
	var reg = await db.Bodegas.FindAsync(id);
	db.Bodegas.Remove(reg);
	await db.SaveChangesAsync();
	return RedirectToAction("Index", new { Toast = "success.delete" });
}

[ValidateAntiForgeryToken]
public async Task<ActionResult> JsDelete(int id) // POST
{
	string userName = HttpContext.User.Identity.Name.ToString();
	Permit[] permisosRequeridos = { Permit.Delete };
	bool hasPermit = Utilidades.hasPermit(permisosRequeridos, ControllerContext, userName);
	if (!hasPermit)
	{
		return Json(new { success = false, message = "No tiene privielegios suficientes para realizar esta acción" }, JsonRequestBehavior.DenyGet);
	}
	try
	{
		Destino reg = await db.Destinos.FindAsync(id);
		db.Destinos.Remove(reg);
		await db.SaveChangesAsync();
		return Json(new { success = true, message = "El registro se ha eliminado satisfactoriamente !!" }, JsonRequestBehavior.DenyGet);
	}
	catch (Exception ex)
	{
		return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.DenyGet);
	}            
}


