using Apphr.Application.Common.Models;
using Apphr.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Medica.Controllers
{
    public class SADCController : Controller
    {        
        public JsonResult JsGetPaciente(string rm)   // POST
        {
            SADCPacienteDTOBase dto = new SADCPacienteDTOBase();
            try
            {
                dto = GetPacienteByHCA(rm);
                return Json(new { success = true, data = dto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, data = dto }, JsonRequestBehavior.AllowGet);
            }           
        }

        public SADCPacienteDTOBase GetPacienteByHCA(string rm)
        {
            using (var db = new SADCDbContext())
            {
                string query = "SELECT pa.Pac_NumHC,pa.Pac_NumHCAntiguo,pe.Per_PrimerNombre,pe.Per_SegundoNombre,pe.Per_TercerNombre,pe.Per_PrimerApellido,pe.Per_SegundoApellido,pe.Per_ApellidoCasada,pe.Per_FechaNacimiento FROM dbo.SAD_Paciente AS pa INNER JOIN dbo.ERP_Persona AS pe ON pa.Pac_Persona =pe.Per_Persona WHERE pa.Pac_NumHCAntiguo = @rm AND pa.Pac_Estado = 1";
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@rm", int.Parse(rm)));
                return db.Database.SqlQuery<SADCPacienteDTOBase>(query, p.ToArray()).FirstOrDefault();

                //var p = new List<SqlParameter>();
                //p.Add(new SqlParameter("@donador", int.Parse("0")));
                //p.Add(new SqlParameter("@opcion", int.Parse("0")));
                //p.Add(new SqlParameter("@primerapellido", "0"));
                //p.Add(new SqlParameter("@segundoapellido", "0"));
                //p.Add(new SqlParameter("@primernombre", "0"));
                //p.Add(new SqlParameter("@segundonombre", "0"));
                //p.Add(new SqlParameter("@fechanac", "0"));
                //p.Add(new SqlParameter("@histoclinic", "0"));
                //p.Add(new SqlParameter("@histoclinicAnti", rm));
                //p.Add(new SqlParameter("@pacienteXX", "0"));
                //p.Add(new SqlParameter("@cedula", "0"));

               //return db.Database.SqlQuery<SADCPacienteDTOBase>(
               //     "sp_SAD_BusquedaPaciente @primerapellido, @segundoapellido, @primernombre, @segundonombre, @fechanac, @histoclinic, @histoclinicAnti, @pacienteXX, @cedula, @opcion, @donador",
               //     p.ToArray()
               //     ).FirstOrDefault();                
            }
        }
    }
}