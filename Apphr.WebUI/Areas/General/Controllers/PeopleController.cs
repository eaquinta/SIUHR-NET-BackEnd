using Apphr.Application.Common.DTOs;
using Apphr.Application.Personas.DTOs;
using Apphr.Application.Usuarios.DTOs;
using Apphr.Domain.Enums;
using Apphr.WebUI.Common;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using Apphr.WebUI.Models;
using Apphr.WebUI.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
                logs = db.AppLogs.Where(s => s.RecordID == id).OrderBy(o => o.CreatedByUser).ToList();  // C20230613
            //}
            foreach(var log in logs)
            {
                cnt++;
                data.Add(new VisTimeline()
                {
                    id = cnt,
                    content = log.EventType + "<br/>por <i class=\"fas fa - edit\"></i>" + log.CreatedByUser + "<br/>" + log.Created_date,
                    group = "1",
                    start = log.Created_date,
                    title = log.EventType
                });
            }

            return Json(data);
        }

    #region SQL
        [Can("persona.ver")]
        public ActionResult Index()                                                         // GET 
        {
            ViewBag.Permissions = Utilidades.GetPermissions(ControllerContext, userName);
            return View();
        }

        [HttpPost]
        public JsonResult JsGetDataTable(DTOJqueryDatatableParam param)                     // GET 
        {
            List<Persona> Data;

            var draw = param.draw;
            var start = param.start;
            var length = param.length;
            var sortColumnName = param.columns[param.order[0].column].name;
            var sortColumnDir = param.order[0].dir;
            var searchValue = param.search.value;
            int pageSize = param.length;
            int skip = param.start;
            int recordsTotal = 0;


            try
            {
                var regs = from p in db.Personas select p;

                if (!(string.IsNullOrEmpty(sortColumnName) && string.IsNullOrEmpty(sortColumnDir)))
                    regs = regs.OrderBy(sortColumnName + " " + sortColumnDir);

                if (!string.IsNullOrEmpty(searchValue))

                    regs = regs.Where(m => m.PersonId.ToString().Contains(searchValue) || m.FirstName.Contains(searchValue) || m.LastName.Contains(searchValue));

                recordsTotal = regs.Count();

                Data = regs.Skip(skip).Take(pageSize).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { draw = draw, data = Data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, });
        }


        public async Task<ActionResult> JsViewMaster(int? id)                               // GET 
        {
            var reg = await db.Personas
                .Where(x => x.PersonId == id)
                .FirstOrDefaultAsync();

            if (reg == null)
                return PartialView("_RegisterNotFound");

            return PartialView("_ViewMaster", mapper.Map<PersonaDTOView>(reg));
        }

        public async Task<ActionResult> JsAsingUserMaster(int? id)
        {
            var reg = await db.Personas
                .Where(x => x.PersonId == id)
                .FirstOrDefaultAsync();

            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = new PersonaDTOCreateUser {
                PersonaId = reg.PersonId
            };

            return PartialView("_CrearUsuario", dto);
        }

        public async Task<ActionResult> JsCEditMaster(int? id)                              // GET 
        {
            string[] permisosRequeridos = { "persona.editar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.AllowGet);
            }

            FillList();

            if (id == null)            
               return PartialView("_CEditMaster", new PersonaDTOCEdit { IsActive = true });
            

            var reg = await db.Personas.Where(x => x.PersonId == id).FirstOrDefaultAsync();
            if (reg == null)
                return PartialView("_RegisterNotFound");

            var dto = mapper.Map<PersonaDTOCEdit>(db.Personas.Find(id));

            
            return PartialView("_CEditMaster", dto);
        }

        public JsonResult JsUploadImage(HttpPostedFileBase ImageFile, int PersonId)
        {
            try
            {
                var reg = db.Personas.Find(PersonId);

                if (ImageFile == null)
                    return Json(new { success = false, message = "No hay imagen para Guardar"}, JsonRequestBehavior.DenyGet);
                
                    string FileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string FileExtension = Path.GetExtension(FileName);

                    DateTime? ImageDate = DateTime.Now;
                    var Image = FileName.Trim() + FileExtension;


                    FileName = ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + Image;

                    var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());
                    var UploadFilePath = Path.Combine(UploadPath, FileName);
                    ImageFile.SaveAs(UploadFilePath);
                reg.Image = Image;
                reg.ImageDate = ImageDate;
                db.SaveChanges();
                return Json(new { success = true, message = Resources.Msg.success_edit, data = "" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }            
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsSaveMaster(PersonaDTOCEdit dto)                     // POST 
        {
            List<string> ListPermit = new List<string>();

            if (dto.PersonId == 0)
                ListPermit.Add("personas.crear");
            else
                ListPermit.Add("personas.editar");

            bool hasPermit = await Utilidades.Can(ListPermit.ToArray(), userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = Resources.Msg.failure_model_invalid });
                }

                if (dto.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(dto.ImageFile.FileName);
                    string FileExtension = Path.GetExtension(FileName);

                    dto.ImageDate = DateTime.Now;
                    dto.Image = FileName.Trim() + FileExtension;


                    FileName = dto.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + dto.Image;

                    var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());
                    var UploadFilePath = Path.Combine(UploadPath, FileName);
                    dto.ImageFile.SaveAs(UploadFilePath);
                }


                if (dto.PersonId == 0)
                { // INSERT

                    // Validación Adicional
                    //if (db.Personas.Any(x => x.I == dto.Nombre))
                    //    return Json(new { success = false, message = "El Cirujano ya esta registrado." });

                    var reg = mapper.Map<Persona>(dto);
                    db.Personas.Add(reg);                    
                    await db.SaveChangesAsync();

                    return Json(new { success = true, message = Resources.Msg.success_create, data = reg }, JsonRequestBehavior.DenyGet);
                }
                else
                { // UPDATE

                    var reg = db.Personas.Find(dto.PersonId);
                    mapper.Map(dto, reg);
                    await db.SaveChangesAsync();
                    
                    return Json(new { success = true, message = Resources.Msg.success_edit, data = reg }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, messageEx = ex.Message, messageInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> JsDeleteMaster(int id)                                // POST 
        {
            string[] permisosRequeridos = { "persona.eliminar" };
            bool hasPermit = await Utilidades.Can(permisosRequeridos, userId);
            if (!hasPermit)
            {
                return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
            }
            try
            {
                using (DbContextTransaction t = db.Database.BeginTransaction())
                {
                    try
                    {
                        var reg = await db.Personas
                            .Where(x => x.PersonId == id)
                            .FirstOrDefaultAsync();                    

                        db.Personas.Remove(reg);

                        await db.SaveChangesAsync();
                        t.Commit();
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                }
                return Json(new { success = true, message = Resources.Msg.success_delete }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = Resources.Msg.failure, exMessage = ex.Message, exInner = ex.InnerException }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public ActionResult Image(int id)                                                   // GET 
        {
            try
            {
                var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());
                var UnknowPath = Path.Combine(UploadPath, "unknow-person.jpg");
                Image img;

                var reg = db.Personas.Find(id);

                if (reg == null || reg.Image == null)
                {
                    img = FileToImage(UnknowPath);
                    return File(ImageToByteArray(img.GetThumbnailImage(200, 200, () => false, IntPtr.Zero)), "image/png");
                }



                var FileName = reg.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + reg.Image;
                var FilePath = Path.Combine(UploadPath, FileName);

                var theFile = new FileInfo(FilePath);

                if (!theFile.Exists)
                {
                    return base.File(UnknowPath, "image/png");
                }

                // byte[] myByte = System.IO.File.ReadAllBytes(FilePath);
                img = FileToImage(FilePath);

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    ms.Write(myByte, 0, myByte.Length);
                //    i = System.Drawing.Image.FromStream(ms);
                //}
                return File(ImageToByteArray(img.GetThumbnailImage(200, 200, () => false, IntPtr.Zero)), "image/jpeg");

            }
            catch (Exception)
            {
                throw;
            }
        }
        private byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private Image FileToImage(string FilePath)
        {
            byte[] biteArray = System.IO.File.ReadAllBytes(FilePath);
            Image img;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(biteArray, 0, biteArray.Length);
                img = System.Drawing.Image.FromStream(ms);
            }
            return img;
        }
        #endregion



        public ActionResult Details(int? id)                // GET
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
        
        public ActionResult Create()                        // GET
        {
            FillList();
            var vm = new PersonaDTOCreate() { IsActive = true };
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult Create(PersonaDTOCreate dto)    // POST
        {
            try
            {
                var reg = mapper.Map<Persona>(dto);
                db.Personas.Add(reg);
                db.SaveChanges();
               
                return RedirectToAction("Index");
                //return Json(new { success = true, message = "Registro guardado con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
                
        public ActionResult Edit(int? id)                   // GET
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
       

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(PersonaDTOEdit vm, int id)
        //{
        //    if (!ModelState.IsValid)
        //        return View(vm);

        //    ViewData["GenderList"] = Enum.GetValues(typeof(Gender))
        //        .Cast<Gender>()
        //        .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
        //        .ToList();

        //    ViewData["EstadoCivilList"] = Enum.GetValues(typeof(EstadoCivil))
        //        .Cast<EstadoCivil>()
        //        .Select(p => new SelectListItem { Value = Convert.ToString((int)p), Text = p.ToString() })
        //        .ToList();
            

        //    PersonaValidator validator = new PersonaValidator();
        //    ValidationResult result = validator.Validate(vm);
        //    if (result.IsValid == false)
        //    {
        //        foreach (ValidationFailure failure in result.Errors)
        //        {
        //            ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        //        }
        //        return View(vm);
        //    }
        //    if(vm.ImageFile != null) {
        //        string FileName = Path.GetFileNameWithoutExtension(vm.ImageFile.FileName);
        //        string FileExtension = Path.GetExtension(vm.ImageFile.FileName);

        //        vm.ImageDate = DateTime.Now;
        //        vm.Image = FileName.Trim() + FileExtension;


        //        FileName = vm.ImageDate.Value.ToString("yyyyMMddhhmmss") + "-" + vm.Image;
                               
        //        var UploadPath = Server.MapPath(ConfigurationManager.AppSettings["AppImagePath"].ToString());

                
        //        var UploadFilePath = Path.Combine(UploadPath, FileName);                
        //        vm.ImageFile.SaveAs(UploadFilePath);
        //    }

        //    try
        //    {
        //        //using (var db = new ApphrDbContext())
        //        //{
        //            var e_reg = db.Personas.Find(vm.PersonId);
        //            mapper.Map(vm, e_reg);
        //            db.SaveChanges();
        //        //}               
                

        //        return RedirectToAction("View", new { id = vm.PersonId });
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View(vm);
        //    }
        //}

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> AsignarUsuario(PersonaDTOCreateUser dto) // GET
        {
            string Errores = "";
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
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { Area = "", userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    //var body = //RazorHelper.RenderViewToString(this, "~/Areas/General/Views/People/_ConfirmEmail.cshtml", new { Url = callbackUrl });
                    var body = ConvertViewToString("_ConfirmEmail", new PersonaDTOEmailConfirm() { 
                        Nombre = nombre, 
                        Contrasena = contrasena, 
                        Email = dto.Email, Url = callbackUrl 
                    });
                    await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", body); // "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    return Json(new { success = true, message = "Se envió un correo de confirmación a su cuenta, revice su correo" });
                }
                Errores = result.Errors.FirstOrDefault();
            }
            return Json(new { success = false, message = Errores });           
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
