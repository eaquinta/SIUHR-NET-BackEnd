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

			if (!String.IsNullOrEmpty(dto?.F?.Buscar))
			{
				regs = regs.Where(s =>
				s.CODIGO.Contains(dto?.F?.Buscar) ||
				s.DESCRI.ToUpper().Contains(dto?.F?.Buscar.ToUpper())
				).ToList();
			}
			
			dto.Result = regs.ToPagedList(pageIndex, pageSize);
			return View(dto);

		}

