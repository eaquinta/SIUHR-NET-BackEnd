//using Apphr.Application.Facilitadores.DTOs;
//using Apphr.WebUI.Models.Entities;
//using Apphr.Domain.Enums;
//using Apphr.WebUI.Controllers;
//using Apphr.WebUI.CustomAttributes;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web.Mvc;

//namespace Apphr.WebUI.Areas.Admision.Controllers
//{
//    [Authorize]

//    public class FacilitadoresController : DbController
//    {
//        [Can(.".ver")]
//        public async Task<ActionResult> Index() // GET
//        {

//            //            //if (!IsActionAvailable(""))
//            //            //    return RedirectToAction("NoAutorizado","Home",new { Area = ""});
//            var facilitadores = db.Facilitadores.Include(f => f.Persona);
//            return View(await facilitadores.ToListAsync());


//        }

//        //[HttpPost, ValidateAntiForgeryToken]
//        public async Task<ActionResult> Asignar(int id) // GET
//        {
//            var facilitador = new Facilitador() { FacilitadorId = id };
//            db.Facilitadores.Add(facilitador);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        public ActionResult AsignarPersona() // GET
//        {
//            FacilitadorDTOBuscarPersona vm = new FacilitadorDTOBuscarPersona();
//            vm.Lista = db.Database
//                .SqlQuery<Persona>("SELECT Personas.*FROM dbo.Personas LEFT JOIN dbo.Facilitadores ON Personas.PersonId =Facilitadores.FacilitadorId WHERE Facilitadores.FacilitadorId IS NULL")
//                .ToList();
//            return View(vm);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<ActionResult> AsignarPersona(FacilitadorDTOBuscarPersona vm)  // POST
//        {
//            var Filtro = vm.Filtro;
//            var parameters = new List<SqlParameter>();

//            string query = "SELECT Personas.*FROM dbo.Personas LEFT JOIN dbo.Facilitadores ON Personas.PersonId =Facilitadores.FacilitadorId WHERE Facilitadores.FacilitadorId IS NULL";
//            if (!string.IsNullOrEmpty(Filtro.FirstName))
//            {
//                query = query + " AND Personas.FirstName = @FirstName";
//                parameters.Add(new SqlParameter("@FirstName", Filtro.FirstName));
//            }
//            if (!string.IsNullOrEmpty(Filtro.MiddleName))
//            {
//                query = query + " AND Personas.MiddleName = @MiddleName";
//                parameters.Add(new SqlParameter("@MiddleName", Filtro.MiddleName));
//            }
//            if (!string.IsNullOrEmpty(Filtro.ThirdName))
//            {
//                query = query + " AND Personas.ThirdName = @ThirdName";
//                parameters.Add(new SqlParameter("@ThirdName", Filtro.ThirdName));
//            }
//            if (!string.IsNullOrEmpty(Filtro.LastName))
//            {
//                query = query + " AND Personas.LastName = @LastName";
//                parameters.Add(new SqlParameter("@LastName", Filtro.LastName));
//            }
//            if (!string.IsNullOrEmpty(Filtro.MatriName))
//            {
//                query = query + " AND Personas.MatriName = @MatriName";
//                parameters.Add(new SqlParameter("@MatriName", Filtro.MatriName));
//            }
//            if (!string.IsNullOrEmpty(Filtro.MarriedName))
//            {
//                query = query + " AND Personas.MarriedName = @MarriedName";
//                parameters.Add(new SqlParameter("@MarriedName", Filtro.MarriedName));
//            }

//            vm.Lista = await db.Database.SqlQuery<Persona>(query, parameters.ToArray()).ToListAsync();
//            return View(vm);
//        }

//        public async Task<ActionResult> Details(int? id) // GET
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            //Facilitador facilitador = await db.Facilitadores.Include(x => x.Persona).Where(f => f.FacilitadorId == id).FirstOrDefaultAsync();
//            Facilitador facilitador = await db.Facilitadores.Include(x => x.Persona).Where(f => f.FacilitadorId == id).FirstOrDefaultAsync();
//            if (facilitador == null)
//            {
//                return HttpNotFound();
//            }
//            var reg = mapper.Map<FacilitadorDTODetails>(facilitador);
//            return View(reg);
//        }

//        //        // GET: Admision/Facilitadores/Create
//        //        public ActionResult Create()
//        //        {
//        //            ViewBag.PersonId = new SelectList(db.Personas, "PersonId", "FirstName");
//        //            return View();
//        //        }


//        //        [HttpPost, ValidateAntiForgeryToken]
//        //        public async Task<ActionResult> Create([Bind(Include = "FacilitadorId,PersonId,Persona")] Facilitador facilitador) // POST 
//        //        {
//        //            if (ModelState.IsValid)
//        //            {
//        //                db.Facilitadores.Add(facilitador);
//        //                await db.SaveChangesAsync();
//        //                return RedirectToAction("Index");
//        //            }

//        //            ViewBag.PersonId = new SelectList(db.Personas, "PersonId", "FirstName", facilitador.PersonId);
//        //            return View(facilitador);
//        //        }


//        public async Task<ActionResult> Edit(int? id) //GET
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Facilitador facilitador = await db.Facilitadores.Include(x => x.Persona).Where(f => f.FacilitadorId == id).FirstOrDefaultAsync();

//            if (facilitador == null)
//            {
//                return HttpNotFound();
//            }
//            //ViewBag.PersonId = new SelectList(db.Personas, "PersonId", "FirstName", facilitador.PersonId);
//            var reg = mapper.Map<FacilitadorDTOEdit>(facilitador);
//            return View(reg);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "FacilitadorId,PersonId,Persona")] Facilitador facilitador) // POST
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(facilitador).State = EntityState.Modified;
//                //actualizar personas
//                db.Personas.Attach(facilitador.Persona);
//                db.Entry(facilitador.Persona).Property(x => x.FirstName).IsModified = true;
//                db.Entry(facilitador.Persona).Property(x => x.MiddleName).IsModified = true;
//                db.Entry(facilitador.Persona).Property(x => x.ThirdName).IsModified = true;
//                db.Entry(facilitador.Persona).Property(x => x.LastName).IsModified = true;
//                db.Entry(facilitador.Persona).Property(x => x.MatriName).IsModified = true;
//                db.Entry(facilitador.Persona).Property(x => x.MarriedName).IsModified = true;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            //ViewBag.PersonId = new SelectList(db.Personas, "PersonId", "FirstName", facilitador.PersonId);
//            return View(facilitador);
//        }


//        public async Task<ActionResult> Delete(int? id) //GET
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
            
//            Facilitador facilitador = await db.Facilitadores.Include(x => x.Persona).Where(f => f.FacilitadorId == id).FirstOrDefaultAsync();

//            if (facilitador == null)
//            {
//                return HttpNotFound();
//            }
//            var reg = mapper.Map<FacilitadorDTODelete>(facilitador);
//            return View(reg);
//        }


//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(int id)  // POST
//        {
//            Facilitador facilitador = await db.Facilitadores.FindAsync(id);
//            db.Facilitadores.Remove(facilitador);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//    }
//}