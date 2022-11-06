
public ActionResult Index(BodegaDTOIndex dto, int? page) //GET
{
	IQueryable<Bodega> regs;

	int pageIndex = 1;
	if (dto?.F == null) dto.F = new BodegaDTOIxFilter();
	if (dto.F.Buscar != dto.F._Buscar)
	{
		page = 1;
		dto.F._Buscar = dto.F.Buscar;
	}

	pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
	//            BodegaDTOIndex dto = new BodegaDTOIndex();


	regs = (from p in db.Bodegas select p);
	if (dto.F != null)
	{
		if (!string.IsNullOrEmpty(dto.F.Buscar))
			regs = regs.Where(x => x.Nombre.Contains(dto.F.Buscar) || x.Descripcion.Contains(dto.F.Buscar));
	}

	regs = regs.OrderBy(x => x.Codigo);
                        
    var rows = mapper.Map<List<DestinoDTOIxRow>>(regs.ToList());
    dto.Result = (PagedList<DestinoDTOIxRow>)rows.ToPagedList(pageIndex, pageSize);
    return View(dto);
}