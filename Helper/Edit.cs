using System.Data;
using System.Data.Entity;
public async Task<ActionResult> Edit(int? id) // GET
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

	var dto = mapper.Map<BodegaDTOEdit>(reg);
	return View(dto);
}

[HttpPost, ValidateAntiForgeryToken]
public async Task<ActionResult> Edit([Bind(Include = "BodegaId,Nombre,Descripcion")] BodegaDTOEdit dto) // POST
{
	if (ModelState.IsValid)
	{
		var reg = mapper.Map<Bodega>(dto);
		db.Entry(reg).State = EntityState.Modified;
		await db.SaveChangesAsync();
		return RedirectToAction("Details", new { id = dto.BodegaId, Toast = "success.edit" });
	}
	return View(dto);
}

