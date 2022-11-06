using Apphr.Application.Common.Models;
using Apphr.Application.PacienteEventos.DTOs;
using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Controllers;
using Apphr.WebUI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Apphr.WebUI.Areas.Admision.Controllers
{
    [Authorize]
    [LogAction]
    public class PacienteEventosController : DbController
    {
        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Index(PacienteEventoDTOIndex vm, List<string> Toast) // GET
        {
            // Fill DropDown Lists
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            ViewBag.Activo = new List<SelectListItem>() {
                new SelectListItem { Text = "Activos", Value = "A" },
                new SelectListItem { Text = "Inactivos", Value = "I" },               
                new SelectListItem { Text = "Todos", Value = "T" },               
            } ;
            // Fill DropDown Lists

            IQueryable<PacienteEvento> regs;
            regs = (from p in db.PacienteEventos select p);
            if (vm.F != null)
            {
                if (!string.IsNullOrEmpty(vm.F.Nombre))
                    regs = regs.Where(x => x.NombrePaciente.Contains(vm.F.Nombre));
                if (!string.IsNullOrEmpty(vm.F.Apellido))
                    regs = regs.Where(x => x.NombrePaciente.Contains(vm.F.Apellido));
                if (vm.F.ServicioId != null)
                    regs = regs.Where(x => x.ServicioId == vm.F.ServicioId);
                switch (vm.F.Activo)
                {
                    case "A":
                        regs = regs.Where(x => x.FechaEgreso == null);
                        break;
                    case "I":
                        regs = regs.Where(x => x.FechaEgreso != null);
                        break;
                    //case "T":
                    default:
                        break;
                }
                //if (vm.F.Activo)
                  //  
            } 
            else
            {
                regs = regs.Where(x => x.FechaEgreso == null);
            }
            vm.Result = await regs.Include(p => p.Servicio).ToListAsync();
            vm.C = C;
            // Errores personalizados
            DTT.Add("error.nofacilitador", new ToastTemplate()
            {
                Title = "Error",
                Message = "Debe ser asignado a facilitadores para continuar",
                Type = "error"
            });
            vm.C.ToastTemplates = GetToastList(Toast);            
            return View(vm);
        }

        public async Task<ActionResult> Traslado(int? id) // GET
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEventoDTOTraslado vm = new PacienteEventoDTOTraslado
            {
                Paciente = await db.PacienteEventos.Include(i => i.Servicio).Where(w => w.PacienteEventoId == id).FirstOrDefaultAsync(),
                Fecha = DateTime.Now               
            };
            vm.CamaDestino = vm.Paciente.Cama;
            vm.ServicioDestinoId = vm.Paciente.ServicioId;
            // Llenado de Drowdown's
            ViewBag.ServicioDestinoId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            return View(vm);
        }

        public async Task<ActionResult> Diagnostico(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reg = await db.PacienteEventos.Include(i => i.Servicio).Where(w => w.PacienteEventoId == id).FirstOrDefaultAsync();
            if (reg == null)
            {
                return HttpNotFound();
            }
            PacienteEventoDTODiagnostico vm = new PacienteEventoDTODiagnostico() 
            {
                Paciente = reg,
                Fecha = DateTime.Now
            };
            vm.UserId = this.GetUserId();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Diagnostico(PacienteEventoDTODiagnostico vm) // POST
        {
            if (ModelState.IsValid)
            {

                var reg0 = await db.PacienteEventos.FindAsync(vm.Paciente.PacienteEventoId);
                reg0.Diagnostico = vm.DiagnosticoNuevo;
                var reg2 = new PacienteEventoHistorial()
                {
                    PacienteEventoId = vm.Paciente.PacienteEventoId,
                    Fecha = vm.Fecha,
                    ServicioOrigenId = vm.Paciente.ServicioId,
                    DiagnosticoAnterior = vm.DiagnosticoOrignal,
                    EstadoObservacion = vm.DiagnosticoNuevo,
                    Tipo = Domain.Enums.PacienteEventoTipo.Actualizacion,
                    UserId = this.GetUserId()
                };
                using (var dbTrn = db.Database.BeginTransaction())
                {
                    db.PacienteEventos.Attach(reg0);
                    db.Entry(reg0).Property(x => x.Diagnostico).IsModified = true;
                    db.PacienteEventoHistoriales.Add(reg2);
                    await db.SaveChangesAsync();

                    dbTrn.Commit();
                }
                return RedirectToAction("Index");
            }
                return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Traslado(PacienteEventoDTOTraslado vm) // POST
        {
            // Llenado de Drowdown's
            ViewBag.ServicioDestinoId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            // Validaciones Server Side
            //if (vm.Paciente.ServicioId == vm.ServicioDestinoId)
            //{                
            //    ModelState.AddModelError("ServicioDestinoId", "El servicio destino debe ser diferente al actual.");             
            //}
            if (vm.Paciente.Cama == vm.CamaDestino && vm.Paciente.ServicioId == vm.ServicioDestinoId)
            {
                ModelState.AddModelError("", "El número de cama o el servicio debe ser diferente para continuar.");
            }
            if (ModelState.IsValid)
            {                
               
                var reg0 = await db.PacienteEventos.FindAsync(vm.Paciente.PacienteEventoId);
                reg0.ServicioId = vm.ServicioDestinoId;
                var reg = new PacienteEventoTraslado() 
                { 
                    PacienteEventoId =vm.Paciente.PacienteEventoId,
                    Fecha = vm.Fecha,
                    Motivo = vm.Motivo 
                };
                var reg2 = new PacienteEventoHistorial()
                {
                    PacienteEventoId = vm.Paciente.PacienteEventoId,
                    Fecha = vm.Fecha,
                    ServicioOrigenId = vm.Paciente.ServicioId,
                    ServicioDestinoId = vm.ServicioDestinoId,
                    CamaOrigen = vm.Paciente.Cama,
                    CamaDestino = vm.CamaDestino,
                    EstadoObservacion = vm.Motivo,
                    Tipo = Domain.Enums.PacienteEventoTipo.Traslado,
                    UserId = this.GetUserId()
                };
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    db.PacienteEventos.Attach(reg0);
                    db.Entry(reg0).Property(x => x.ServicioId).IsModified = true;
                    db.PacienteEventoTraslados.Add(reg);
                    db.PacienteEventoHistoriales.Add(reg2);
                    await db.SaveChangesAsync();

                    dbContextTransaction.Commit();
                }
                
                return RedirectToAction("Index");
            }
            // FillData
            vm.Paciente = await db.PacienteEventos.Include(i => i.Servicio).Where(w => w.PacienteEventoId == vm.Paciente.PacienteEventoId).FirstOrDefaultAsync();
            vm.Fecha = DateTime.Now;
            return View(vm);
        }

        public async Task<ActionResult> Egreso(int? id) // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEventoDTOEgreso vm = new PacienteEventoDTOEgreso
            {
                Paciente = await db.PacienteEventos.FindAsync(id),
                Fecha = DateTime.Now
            };
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            return View(vm);
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Egreso(PacienteEventoDTOEgreso vm) // PHOST
        {
            if (ModelState.IsValid)
            {
                var reg0 = await db.PacienteEventos.FindAsync(vm.Paciente.PacienteEventoId);
                reg0.FechaEgreso = vm.Fecha;
                var reg = new PacienteEventoTraslado()
                {
                    PacienteEventoId = vm.Paciente.PacienteEventoId,
                    Fecha = vm.Fecha,
                    Motivo = vm.Motivo
                };
                var reg2 = new PacienteEventoHistorial()
                {
                    PacienteEventoId = vm.Paciente.PacienteEventoId,
                    Fecha = vm.Fecha,
                    ServicioOrigenId = vm.Paciente.ServicioId,
                    ServicioDestinoId = vm.ServicioId,
                    EstadoObservacion = vm.Motivo,
                    Tipo = Domain.Enums.PacienteEventoTipo.Egreso,
                    UserId = this.GetUserId()
                };
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    db.PacienteEventos.Attach(reg0);
                    db.Entry(reg0).Property(x => x.FechaEgreso).IsModified = true;
                    //db.PacienteEventoTraslados.Add(reg);
                    db.PacienteEventoHistoriales.Add(reg2);
                    await db.SaveChangesAsync();

                    dbContextTransaction.Commit();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        [AppAuthorization(Permit.View)]
        public async Task<ActionResult> Details(int? id, List<string> Toast) // GET
        {
            ViewData["Toast"] = GetToastList(Toast);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PacienteEvento reg = await db.PacienteEventos
                .Include(p => p.Servicio)
                .Include(p => p.Historial)
                .Include("Historial.ServicioDestino")
                .Include("Historial.ServicioOrigen")
                .Include("Historial.Usuario")
                .Include("Historial.Usuario.Persona")
                .Where(x => x.PacienteEventoId == id).FirstOrDefaultAsync();
            
            if (reg == null)
            {
                return HttpNotFound();
            }
            var vm = mapper.Map<PacienteEventoDTODetail>(reg);

            return View(vm);
        }

        [AppAuthorization(Permit.Edit)]
        public ActionResult Create() // GET
        {
            var vm = new PacienteEventoDTOCreate() { FechaIngreso = DateTime.Now, TieneTarjeta = false };
            // Validar si el usuario es un Facilitador
            int UserId = Int32.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            int? PersonId = db.Users.Find(UserId).PersonaId;
            if(!db.Facilitadores.Any(x => x.FacilitadorId == PersonId))
            {
                return RedirectToAction("Index", new { Toast = "error.nofacilitador" });
            }
            else
            {
                vm.FacilitadorId = PersonId;
                vm.NombreFacilitador = db.Facilitadores.Include(i => i.Persona).Where(x => x.FacilitadorId==PersonId).FirstOrDefault().Persona.Name;
            }
            // Asignar Facilitador   HttpContext.Current.User.Identity.Name.ToString();

            //ViewBag.FacilitadorId = new SelectList(db.Facilitadores.Include(x => x.Persona), "FacilitadorId", "Persona.Name");
            ViewBag.RegistroMedicoId = new SelectList(db.RegistrosMedicos, "RegistroMedicoId", "CreatedBy");
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre");
            return View(vm);
        }

        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PacienteEventoId,NombrePaciente,NombreFamiliar,FechaIngreso,Cama,Registro,Edad,FacilitadorId,ServicioId,Diagnostico,Sintomas,Telefono,TieneTarjeta,Observaciones,Procedencia")] PacienteEvento pacienteEvento)
        {
            if (ModelState.IsValid)
            {
                var reg2 = new PacienteEventoHistorial()
                {
                    Fecha = pacienteEvento.FechaIngreso,
                    EstadoObservacion = pacienteEvento.Sintomas,
                    Tipo = Domain.Enums.PacienteEventoTipo.Ingreso,
                    UserId = this.GetUserId()
                };
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    db.PacienteEventoHistoriales.Add(reg2);
                    db.PacienteEventos.Add(pacienteEvento);
                    await db.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }

                return RedirectToAction("Index");
            }

            //ViewBag.FacilitadorId = new SelectList(db.Facilitadores.Include(x => x.Persona), "FacilitadorId", "Persona.Name", pacienteEvento.FacilitadorId);
            ViewBag.RegistroMedicoId = new SelectList(db.RegistrosMedicos, "RegistroMedicoId", "CreatedBy", pacienteEvento.RegistroMedicoId);
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", pacienteEvento.ServicioId);
            return View(pacienteEvento);
        }

        [AppAuthorization(Permit.Edit)]
        public async Task<ActionResult> Edit(int? id)  // GET
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEvento pacienteEvento = await db.PacienteEventos.FindAsync(id);
            
            if (pacienteEvento == null)
            {
                return HttpNotFound();
            }
            //ViewBag.FacilitadorId = new SelectList(db.Facilitadores.Include(x => x.Persona), "FacilitadorId", "Persona.Name", pacienteEvento.FacilitadorId);
            ViewBag.RegistroMedicoId = new SelectList(db.RegistrosMedicos, "RegistroMedicoId", "CreatedBy", pacienteEvento.RegistroMedicoId);
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", pacienteEvento.ServicioId);
            var vm = mapper.Map<PacienteEventoDTOEdit>(pacienteEvento);
            // Validar si el usuario es un Facilitador
            int UserId = Int32.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (!db.Facilitadores.Any(x => x.FacilitadorId == UserId))
            {
                return RedirectToAction("Index", new { Toast = "error.nofacilitador" });
            }
            else
            {
                vm.FacilitadorId = UserId;
                vm.NombreFacilitador = db.Facilitadores.Include(i => i.Persona).Where(x => x.FacilitadorId == UserId).FirstOrDefault().Persona.Name;
            }
            return View(vm);
        }

        [AppAuthorization(Permit.Edit)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PacienteEventoId,NombrePaciente,NombreFamiliar,FechaIngreso,FechaEgreso,Cama,Registro,RegistroMedicoId,FacilitadorId,ServicioId,Diagnostico,Sintomas,Telefono,TieneTarjeta,Observaciones,Edad,Procedencia")] PacienteEventoDTOEdit vm)
        {
            if (ModelState.IsValid)
            {
                var reg = db.PacienteEventos.Find(vm.PacienteEventoId);
//                db.PacienteEventos.Attach(pacienteEvento);
                mapper.Map(vm, reg);
                //db.Entry(pacienteEvento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = vm.PacienteEventoId, Toast = "success.edit" });
            }
            //ViewBag.FacilitadorId = new SelectList(db.Facilitadores, "FacilitadorId", "FacilitadorId", vm.FacilitadorId);
            //ViewBag.RegistroMedicoId = new SelectList(db.RegistrosMedicos, "RegistroMedicoId", "CreatedBy", vm.RegistroMedicoId);
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "Nombre", vm.ServicioId);
            return View(vm);
        }

        // GET: Admision/PacienteEventos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEvento pacienteEvento = await db.PacienteEventos.FindAsync(id);
            if (pacienteEvento == null)
            {
                return HttpNotFound();
            }
            return View(pacienteEvento);
        }

        // POST: Admision/PacienteEventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PacienteEvento pacienteEvento = await db.PacienteEventos.FindAsync(id);
            db.PacienteEventos.Remove(pacienteEvento);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }        
    }
}
