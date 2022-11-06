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