using Apphr.Application.Usuarios.DTOs;
using Apphr.WebUI.Controllers;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using Apphr.Application.Facilitadores.DTOs;
using Apphr.Domain.Entities;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Apphr.WebUI.CustomAttributes;
using Apphr.Domain.Enums;

namespace Apphr.WebUI.Areas.General.Controllers
{
    [Authorize]
    public class UsuariosController : DbController
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

        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Index(UsuarioDTOIndex vm, List<string> Toast) // GET
        {
            
            List<UsuarioDTOIxRow> regs;
            string query = @"SELECT * FROM
	                        (SELECT u.Id, u.UserName, p.Name, 
			                    ( SELECT STRING_AGG ( r.Name, ', ' ) AS RoleNames 
                                    FROM dbo.AppUserRole AS ur LEFT JOIN dbo.AppRole AS r ON ur.RoleId = r.Id 
	                                WHERE ur.UserId = u.Id ) AS RoleNames
		                        FROM dbo.AppUser AS u INNER JOIN dbo.Personas AS p ON  u.PersonaId = p.PersonId
	                        ) AS tr";

            if (vm.F != null)
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Buscar", "%" + vm.F.Buscar + "%"));
                query = query + @" WHERE tr.UserName LIKE @Buscar OR tr.Name LIKE @Buscar OR tr.RoleNames LIKE @Buscar";
                regs = await db.Database.SqlQuery<UsuarioDTOIxRow>(query, parameters.ToArray()).ToListAsync();
            }
            else
            {
                regs = await db.Database.SqlQuery<UsuarioDTOIxRow>(query).ToListAsync();
            }

            //var regs = (from user in db.Users
            //      join person in db.Personas on user.PersonaId equals person.PersonId select new { user, person });
            //if (vm.F != null)
            //{
            //    if (!string.IsNullOrEmpty(vm.F.Buscar))
            //        regs = regs.Where(x => x.user.UserName.Contains(vm.F.Buscar));
            //    //if (!string.IsNullOrEmpty(vm.F.Apellido))
            //    //    regs = regs.Where(x => x.NombrePaciente.Contains(vm.F.Apellido));
            //    //if (vm.F.ServicioId != null)
            //    //    regs = regs.Where(x => x.ServicioId == vm.F.ServicioId);               
            //}
            //else
            //{
            //    //regs = regs.Where(x => x.FechaEgreso == null);
            //}
            //vm.Result = regs.ToList()
            //            .Select(p => new UsuarioDTOIxRow()
            //            {
            //                Id = p.user.Id,
            //                UserName = p.user.UserName,
            //                Name = p.person,
            //                RoleNames = string.Join(", ", p.RoleNames)
            //            });
            ////vm.C = C;
            //// Errores personalizados
            //DTT.Add("error.nofacilitador", new ToastTemplate()
            //{
            //    Title = "Error",
            //    Message = "Debe ser asignado a facilitadores para continuar",
            //    Type = "error"
            //});
            ////vm.C.ToastTemplates = GetToastList(Toast);


            //regs = (from user in db.Users
            //            join person in db.Personas on user.PersonaId equals person.PersonId                        
            //            select new
            //            {
            //                UserId = user.Id,
            //                Username = user.UserName,
            //                Name = person.Name,
            //                RoleNames = (from userRole in user.Roles
            //                             join role in db.Roles on userRole.RoleId equals role.Id
            //                             select role.Name).ToList()
            //            })
            //            .ToList()
            //            .Select(p => new UsuarioDTOIxRow()
            //            {
            //                Id = p.UserId,
            //                UserName = p.Username,
            //                Name = p.Name,
            //                RoleNames = string.Join(", ", p.RoleNames)
            //            });

            return View(new UsuarioDTOIndex() { Result = regs });           
        }

        public ActionResult Details(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = db.Users.Find(id);
            if (reg == null)
            {
                return HttpNotFound();
            }
            
            var vm = mapper.Map<UsuarioDTODetails>(reg);
            return View(vm);
        }


        public async Task<ActionResult> Edit(int? id) // GET
        {
            ViewData["RoleList"] = db.Roles
              .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
              .ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await db.Users
                .Include(i => i.Persona)
                .Include(i => i.Roles)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            if (reg == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<UsuarioDTOEdit>(reg);
            vm.SelectedRoles = vm.Roles.Select(r => r.RoleId).ToList();
            return View(vm);
            
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,SelectedRoles,Roles")] UsuarioDTOEdit vm) // POST
        {
            ViewData["RoleList"] = await db.Roles
             .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() })
             .ToListAsync();

            if (ModelState.IsValid)
            {
                var RoleList = await db.Roles.ToListAsync();
                string[] RolesAsignados = { };
                if (vm.SelectedRoles != null)
                {
                    RolesAsignados = RoleList.Where(r => vm.SelectedRoles.Contains(r.Id)).Select(r => r.Name).ToArray();
                }
                

                //var reg = await db.Users
                //.Include(i => i.Persona)
                //.Include(i => i.Roles)
                //.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
                //mapper.Map(vm, reg);
                //reg.Roles = RoleList.Where(r => vm.SelectedRoles.Contains(r.Id)).Select(r => new AppUserRole() { UserId = vm.Id, RoleId = r.Id }).ToList();
                //await db.SaveChangesAsync();

                // Eliminar Roles
                var roles = await UserManager.GetRolesAsync(vm.Id);
                await UserManager.RemoveFromRolesAsync(vm.Id, roles.ToArray());

                // Asignar Nuevos Roles                
                await UserManager.AddToRolesAsync(vm.Id, RolesAsignados);
                return RedirectToAction("Index");

            }
            return View();
        }
       
        public ActionResult AsignarPersona() // GET
        {
            UsuarioDTOAsignar vm = new UsuarioDTOAsignar();
            vm.Lista = db.Database
                .SqlQuery<Persona>("SELECT Personas.*FROM dbo.Personas INNER JOIN dbo.AppUser ON Personas.PersonId =AppUser.PersonaId WHERE AppUser.Id IS NULL")
                .ToList();
            return View(vm);
        } 
                

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AsignarPersona(FacilitadorDTOBuscarPersona vm)  // POST
        {
            var Filtro = vm.Filtro;
            var parameters = new List<SqlParameter>();

            string query = "SELECT Personas.*FROM dbo.Personas INNER JOIN dbo.AppUser ON Personas.PersonId =AppUser.PersonaId WHERE AppUser.Id IS NULL";
            if (!string.IsNullOrEmpty(Filtro.FirstName))
            {
                query = query + " AND Personas.FirstName = @FirstName";
                parameters.Add(new SqlParameter("@FirstName", Filtro.FirstName));
            }
            if (!string.IsNullOrEmpty(Filtro.MiddleName))
            {
                query = query + " AND Personas.MiddleName = @MiddleName";
                parameters.Add(new SqlParameter("@MiddleName", Filtro.MiddleName));
            }
            if (!string.IsNullOrEmpty(Filtro.ThirdName))
            {
                query = query + " AND Personas.ThirdName = @ThirdName";
                parameters.Add(new SqlParameter("@ThirdName", Filtro.ThirdName));
            }
            if (!string.IsNullOrEmpty(Filtro.LastName))
            {
                query = query + " AND Personas.LastName = @LastName";
                parameters.Add(new SqlParameter("@LastName", Filtro.LastName));
            }
            if (!string.IsNullOrEmpty(Filtro.MatriName))
            {
                query = query + " AND Personas.MatriName = @MatriName";
                parameters.Add(new SqlParameter("@MatriName", Filtro.MatriName));
            }
            if (!string.IsNullOrEmpty(Filtro.MarriedName))
            {
                query = query + " AND Personas.MarriedName = @MarriedName";
                parameters.Add(new SqlParameter("@MarriedName", Filtro.MarriedName));
            }

            vm.Lista = await db.Database.SqlQuery<Persona>(query, parameters.ToArray()).ToListAsync();
            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UsuarioDTOCreate vm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = vm.Email, Email = vm.Email };
                var result = await UserManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar correo electrónico con este vínculo
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = "http://admision.hospitalroosevelt.gob.gt" + Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
                    await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta Hospital Roosevelt", "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                AddErrors(result);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario            
            return View(vm);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult SetPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioDTOSetPassword dto = new UsuarioDTOSetPassword() { UserId = id.Value };
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(UsuarioDTOSetPassword dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);          
            }
            var result = await UserManager.RemovePasswordAsync(dto.UserId);
            result = await UserManager.AddPasswordAsync(dto.UserId, dto.Password);            
            if (result.Succeeded)
            {                
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(dto);
          
        }
    }
}