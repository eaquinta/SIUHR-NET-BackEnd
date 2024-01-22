using Apphr.Application.Common.DTOs;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Apphr.WebUI.Areas.Medica.Controllers;
using Apphr.WebUI.Controllers;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Ortopedia.Controllers
{
    public class PacienteController : DbController
    {
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
                var reg = await db.ORTPacientes
                    //.Include("Persona")
                    .Where(x => x.RefPac_NumHCAntiguo.ToString().Equals(val))
                    .FirstOrDefaultAsync();
                if (reg == null)
                {
                    // Importar
                    var SADCDTO = new SADCController().GetPacienteByHCA(val);
                    if (SADCDTO != null)
                    {
                        reg = new ORTPaciente();                        
                        reg.RefPac_NumHC = SADCDTO.pac_numhc;
                        reg.RefPac_NumHCAntiguo = SADCDTO.pac_numhcantiguo;
                        reg.FechaNacimiento = SADCDTO.Per_FechaNacimiento;
                        reg.Nombre = SADCDTO.NombreCompleto;
                        
                        db.ORTPacientes.Add(reg);
                        await db.SaveChangesAsync();
                        isNew = true;
                    }
                }

                return Json(new { success = true, data = reg, isNew }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, isNew, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult JsPacienteRMExist(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = Request.Params[0];
            }
            return Json(db.ORTPacientes.Where(x => x.RefPac_NumHCAntiguo.ToString() == val).Any());
        }

        public async Task<JsonResult> JsGetById(int? id)
        {
            var res = await db.Pacientes.Include(i => i.Persona).Where(x => x.PacienteId == id).FirstOrDefaultAsync();
            return Json(new { success = (res != null), data = res }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> JsGetByFilter(string f, string tipo = "AC")
        {
            Object res = null;
            var result = from r in db.Pacientes select r;

            if (!string.IsNullOrEmpty(f))
                result = result.Where(x => x.Persona.Name.Contains(f) || x.RefPac_NumHCAntiguo.ToString().Contains(f));

            result = result.Take(autoCompleteSize);
            if (tipo == "AC")
            {
                res = await result.Select(p => new DTOAutocompleteItem { id = p.PacienteId.ToString(), text = p.Persona.Name })
                    .ToListAsync();
            }
            if (tipo == "S")
            {
                res = await result.Select(s => new DTOSelect2
                {
                    id = s.PacienteId.ToString(),
                    text = s.RefPac_NumHCAntiguo.ToString(),
                    html = "<div style=\"font-weight: bold;\">" + s.RefPac_NumHCAntiguo.ToString() + "</div><div style=\"font-size: 0.75em;\">" + s.Persona.Name + "</div>"
                })
                .ToListAsync();
            }

            return Json(new { data = res }, JsonRequestBehavior.AllowGet);
        }
    }
}