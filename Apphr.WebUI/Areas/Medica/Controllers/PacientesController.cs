using Apphr.Application.Common.Models;
using Apphr.Application.Pacientes.DTOs;
using Apphr.Domain.Entities;
using Apphr.WebUI.Controllers;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Medica.Controllers
{
    public class PacientesController : DbController
    {
        // GET: Medica/Pacientes
        public ActionResult Index()
        {
            return View();
        }

        // HCA = Historial Clínico Antiguo
        public async Task<ActionResult> JsGetPacienteByHCA(string val)
        {
            bool isNew = false;
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }
            if (string.IsNullOrEmpty(val))
            {
                object r = null;
                return Json(new { success = true, data = r, isNew }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var reg = await db.Pacientes.Include("Persona").Where(x => x.RefPac_NumHCAntiguo.ToString().Equals(val)).FirstOrDefaultAsync();
                if (reg == null)
                {
                    // Importar
                    var sadcDto = new SADCController().GetPacienteByHCA(val);
                    if (sadcDto != null)
                    {
                        reg = new Paciente();
                        reg.Persona = new Persona();
                        reg.RefPac_NumHC = sadcDto.pac_numhc;
                        reg.RefPac_NumHCAntiguo = sadcDto.pac_numhcantiguo;
                        reg.Persona.FirstName = sadcDto.per_primernombre;
                        reg.Persona.MiddleName = sadcDto.Per_SegundoNombre;
                        reg.Persona.LastName = sadcDto.Per_PrimerApellido;
                        reg.Persona.MatriName = sadcDto.Per_SegundoApellido;
                        db.Pacientes.Add(reg);
                        await db.SaveChangesAsync();
                        isNew = true;
                    }
                }
                
                var dto = mapper.Map<PacienteDTOBase>(reg);
                return Json(new { success = true, data = dto , isNew }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, isNew }, JsonRequestBehavior.AllowGet);                
            }            
        }

        [HttpPost]
        public JsonResult JsPacienteRMExist(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }            
            return Json(db.Pacientes.Where(x => x.RefPac_NumHCAntiguo.ToString() == val).Any());
        }

    }
}