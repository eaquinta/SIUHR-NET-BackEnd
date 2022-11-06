using AutoMapper;
using FluentValidation.Results;
using Apphr.Application;
using Apphr.Application.Personas.Commands;
using Apphr.Application.Personas.DTOs;
using Apphr.Domain.Entities;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Apphr.Domain.Enums;
using System.Drawing;
using Apphr.WebUI.Controllers;
using Apphr.Application.Usuarios.DTOs;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Apphr.WebUI.Services;
using Apphr.WebUI.Common;

namespace Apphr.WebUI.Areas.General.Controllers
{
    public class VisTimeline
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime start { get; set; }
        public string group { get; set; }
        public string title { get; set; }
    }

    [Authorize]
    [LogAction]
    public class PeopleController : DbController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public JsonResult GetEventsList(string id)
        {
            if(id == null) { return Json(new { Datos = "Error" }); }
            List<VisTimeline> data = new List<VisTimeline>();
            List<AppEntityLog> logs;
            int cnt = 0;

            //using (var db = new ApphrDbContext())
            //{
                logs = db.AppLogs.Where(s => s.RecordID == id).OrderBy(o => o.Created_by).ToList();
            //}
            foreach(var log in logs)
            {
                cnt++;
                data.Add(new VisTimeline()
                {
                    id = cnt,
                    content = log.EventType + "<br/>por <i class=\"fas fa - edit\"></i>" + log.Created_by + "<br/>" + log.Created_date,
                    group = "1",
                    start = log.Created_date,
                    title = log.EventType
                });
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            IQueryable<Persona> regs;
            List<Persona> Data;
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
                    regs = (from Persona in db.Personas select Persona);
                    if (!(string.IsNullOrEmpty(sortColumn) &&string.IsNullOrEmpty(sortColumnDir)))
                    {
                        regs = regs.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        regs = regs.Where(m => m.PersonId.ToString().Contains(searchValue)|| m.FirstName.Contains(searchValue) || m.LastName.Contains(searchValue));
                    }                    
                    recordsTotal = regs.Count();

                    Data = regs.Skip(skip).Take(pageSize).ToList();                    
               // }
                
                
            }
            catch (Exception)
            {
                throw;
            }
            return Json( new { data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }
        [AppAuthorization(Permit.View)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Personas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound("Error en URL..."); ;
            var vm = new PersonaDTOView();
            try
            {                
                //using (var db = new ApphrDbContext())
                //{
                    //var reg = db.Personas.Where(x => x.PersonId == id).FirstOrDefault();
                    var reg = db.Personas.Find(id);
                    if (reg == null) return HttpNotFound("Error: El registro solicitado es inexistente");
                    vm = mapper.Map<PersonaDTOView>(reg);
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return View(vm);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            FillList();

            var vm = new PersonaDTOCreate() { IsActive = true };

            return View(vm);
        }

        
        [HttpPost]
        public ActionResult Create(PersonaDTOCreate vm)  // POST
        {
            try
            {
                var reg = mapper.Map<Persona>(vm);
                //var reg = new Persona()
                //{
                //    FirstName = vm.FirstName,
                //    LastName = vm.LastName
                //};
                //using (var db = new ApphrDbContext())
                //{
                    //db.Users.Where(x => x.PersonaId == )
                    db.Personas.Add(reg);
                    db.SaveChanges();
               // }
                // TODO: Add insert logic here
                return RedirectToAction("Index");
                //return Json(new { success = true, message = "Registro guardado con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

                
        public ActionResult Edit(int? id) // GET
        {
            if (id == null)
                return HttpNotFound();

            FillList();

            var vm = new PersonaDTOEdit();
            try
            {
                //using (var db = new ApphrDbContext())
                //{
                var reg = db.Personas.Where(x => x.PersonId == id).FirstOrDefault();
                if (reg == null)
                {
                    return HttpNotFound();
                }
                vm = mapper.Map<PersonaDTOEdit>(db.Personas.Find(id));
                vm.Usuario = mapper.Map<UsuarioDTOEdit>(db.Users.Where(x => x.PersonaId == id).FirstOrDefault());
            //    }
            }
            catch (Exception)
            {
                throw;
            }
            return View(vm);
        }

        
        [HttpGet]
        public ActionResult Image(int id)
        {
            try
            {
//                using (var db = new ApphrDbContext())
//                {
                    var reg = db.Personas.Find(id);
                    if (reg == null || reg.Image == null)
                        return HttpNotFound();

                    var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());
                    var FileName = reg.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + reg.Image;
                    var UploadFilePath = Path.Combine(UploadPath, FileName);

                    var theFile = new FileInfo(UploadFilePath);

                    if (!theFile.Exists)
                    {
                        return HttpNotFound();                        
                        //or return base.File(path, "image/png");
                    }
                    byte[] myByte = System.IO.File.ReadAllBytes(UploadFilePath);
                    Image i;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(myByte, 0, myByte.Length);
                        i = System.Drawing.Image.FromStream(ms);
                    }
                    return File(imageToByteArray(i.GetThumbnailImage(300, 300, () => false, IntPtr.Zero)), "image/jpeg");
                    //return File(System.IO.File.ReadAllBytes(UploadFilePath), "image/png");
  //              }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(PersonaDTOEdit vm, int id)
        {
            if (!ModelState.IsValid)
                return View(vm);

            ViewData["GenderList"] = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
                .ToList();

            ViewData["EstadoCivilList"] = Enum.GetValues(typeof(EstadoCivil))
                .Cast<EstadoCivil>()
                .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
                .ToList();
            

            PersonaValidator validator = new PersonaValidator();
            ValidationResult result = validator.Validate(vm);
            if (result.IsValid == false)
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View(vm);
            }
            if(vm.ImageFile != null) {
                string FileName = Path.GetFileNameWithoutExtension(vm.ImageFile.FileName);
                string FileExtension = Path.GetExtension(vm.ImageFile.FileName);

                vm.ImageDate = DateTime.Now;
                vm.Image = FileName.Trim() + FileExtension;


                FileName = vm.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + vm.Image;
                               
                var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());

                
                var UploadFilePath = Path.Combine(UploadPath, FileName);                
                vm.ImageFile.SaveAs(UploadFilePath);
            }

            try
            {
                //using (var db = new ApphrDbContext())
                //{
                    var e_reg = db.Personas.Find(vm.PersonId);
                    mapper.Map(vm, e_reg);
                    db.SaveChanges();
                //}               
                

                return RedirectToAction("View", new { id = vm.PersonId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AsignarUsuario(PersonaDTOCreateUser dto) // GET
        {
            if (ModelState.IsValid)
            {
                var nombre = db.Personas.Where(x => x.PersonId == dto.PersonaId).FirstOrDefault().Name;
                dto.Email = dto.Email.ToLower();
                var user = new AppUser { UserName = dto.Email, Email = dto.Email, PersonaId = dto.PersonaId };
                var contrasena = Utilidades.GenerarContrasenaAleatoria(6);
                var result = await UserManager.CreateAsync(user, contrasena);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771

                    // Enviar correo electrónico con este vínculo
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = "http://admision.hospitalroosevelt.gob.gt" + Url.Action("ConfirmEmail", "Account", new { Area = "", userId = user.Id, code = code });

                    //var body = //RazorHelper.RenderViewToString(this, "~/Areas/General/Views/People/_ConfirmEmail.cshtml", new { Url = callbackUrl });
                    var body = ConvertViewToString("_ConfirmEmail", new PersonaDTOEmailConfirm() { Nombre = nombre, Contrasena = contrasena, Email = dto.Email, Url = callbackUrl });
                    await UserManager.SendEmailAsync(user.Id, ConfigurationManager.AppSettings["SiteName"].ToString() +", confirmar cuenta", body); // "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    return RedirectToAction("Edit", new { id = dto.PersonaId });
                }
                AddErrors(result);
            }
            return RedirectToAction("Edit", new { id = dto.PersonaId });
        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // POST: Personas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //using (var db = new ApphrDbContext())
                //{
                    var reg = db.Personas.Find(id);
                    db.Personas.Remove(reg);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Registro Eliminado correctamnete" });
                //}
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        private void FillList()
        {
            ViewData["GenderList"] = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
                .ToList();

            ViewData["EstadoCivilList"] = Enum.GetValues(typeof(EstadoCivil))
               .Cast<EstadoCivil>()
               .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
               .ToList();

            ViewData["ReligionList"] = Enum.GetValues(typeof(Religion))
               .Cast<Religion>()
               .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
               .ToList();

            ViewData["EtniaList"] = Enum.GetValues(typeof(Etnia))
               .Cast<Etnia>()
               .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
               .ToList();
        }

    }
}
