using AutoMapper;
using FluentValidation.Results;
using Apphr.Application.RegistrosMedicos.DTOs;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;
using Apphr.Application;

namespace Apphr.WebUI.Controllers.RMU
{
    public class RegistroMedicoController : DbController
    {
        //readonly IMapper mapper = AutoMapperConfig._mapper;

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            IQueryable<RegistroMedico> regs;
            List<RegistroMedico> Data;
            int pageSize, skip, recordsTotal;

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var ixColumn = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + ixColumn + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            try
            {
                //using (var db = new ApphrDbContext())
                //{
                    regs = (from RegistroMedico in db.RegistrosMedicos select RegistroMedico);
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        regs = regs.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        regs = regs.Where(m => m.RegistroMedicoId.Contains(searchValue));
                    }
                    recordsTotal = regs.Count();

                    Data = regs.Skip(skip).Take(pageSize).ToList();
                //}


            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Details(string id)
        {
            if (id == null)
                return HttpNotFound();
            var vm = new RegistroMedicoDTOView();
            try
            {
                //using (var db = new ApphrDbContext())
                //{
                    var reg = db.RegistrosMedicos.Include("Person").Where(x => x.RegistroMedicoId == id).FirstOrDefault();                    
                    if (reg == null) return HttpNotFound("Error: El registro solicitado es inexistente");
                    vm = mapper.Map<RegistroMedicoDTOView>(reg);
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(RegistroMedicoDTOCreate vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            //using (var db = new ApphrDbContext())
            //{
                //vm.RegistroMedicoId = Guid.NewGuid();
                vm.RegistroMedicoId = db.GetYearlyAutonumeric("RegistroMedico", DateTime.Now.Year);
              
                var reg = mapper.Map<RegistroMedico>(vm);
                db.RegistrosMedicos.Add(reg);
                db.SaveChanges();
            
            
            return View(vm);           
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
                return HttpNotFound();

            var vm = new RegistroMedicoDTOEdit();
            try
            {
                //using (var db = new ApphrDbContext())
                //{
                var reg = db.RegistrosMedicos.Include("Person").Where(x => x.RegistroMedicoId == id).FirstOrDefault();
                if (reg == null) return HttpNotFound("Error: El registro solicitado es inexistente");
                vm = mapper.Map<RegistroMedicoDTOEdit>(reg);
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegistroMedicoDTOEdit vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            //PersonValidator validator = new PersonValidator();
            //ValidationResult result = validator.Validate(vm);
            //if (result.IsValid == false)
            //{
            //    foreach (ValidationFailure failure in result.Errors)
            //    {
            //        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
            //    }
            //    return View(vm);
            //}
            //if (vm.ImageFile != null)
            //{
            //    string FileName = Path.GetFileNameWithoutExtension(vm.ImageFile.FileName);
            //    string FileExtension = Path.GetExtension(vm.ImageFile.FileName);

            //    vm.ImageDate = DateTime.Now;
            //    vm.Image = FileName.Trim() + FileExtension;


            //    FileName = vm.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + vm.Image;

            //    var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());


            //    var UploadFilePath = Path.Combine(UploadPath, FileName);
            //    vm.ImageFile.SaveAs(UploadFilePath);
            //}

            try
            {
               // using (var db = new ApphrDbContext())
                //{
                    //var reg = db.RegistrosMedicos.Find(vm.RegMedId);
                    var reg = db.RegistrosMedicos.Include("Person").Where(x => x.RegistroMedicoId == vm.RegistroMedicoId).FirstOrDefault();
                    mapper.Map(vm, reg);
                    db.SaveChanges();
                //}


                return RedirectToAction("View", new { id = vm.RegistroMedicoId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }
    }
}