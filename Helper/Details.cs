[AppAuthorization(Permit.View)]
public async Task<ActionResult> Details(int? id)
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