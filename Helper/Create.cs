public async Task<ActionResult> Create()  //GET
{            
	return View();
}

[HttpPost, ValidateAntiForgeryToken]
public async Task<ActionResult> Create([Bind(Exclude = "MaterialId")] BodegaDTOCreate dto) //POST
{
	if (ModelState.IsValid)
	{
		Bodega reg = new Bodega();
	 
		mapper.Map(dto, reg);

		db.Bodegas.Add(reg);
		await db.SaveChangesAsync();

		return RedirectToAction("Index");

	}
	return View(dto);
}