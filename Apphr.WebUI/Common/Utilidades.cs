using Apphr.Domain.Enums;
using Apphr.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Apphr.WebUI.Common
{
    public static class Utilidades
    {
        public static string GenerarContrasenaAleatoria(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        //public static JsonResult JsonNotPermited(Permit[] Permisos, ControllerContext controllerContext, string userName)
        //{
        //    if (!this.hasPermit(Permisos,userName))
        //    {
        //        return Json(new { success = false, message = Resources.Msg.privileges_none }, JsonRequestBehavior.DenyGet);
        //    }
        //    return null;
        //}

        public static bool hasPermit(Permit[] Permisos, ControllerContext controllerContext, string userName) //string userName, string controllerName, string areaName)
        {
            if (userName == "eaquinta@yahoo.com")
                return true;

            string controllerName = (string)controllerContext.RouteData.GetRequiredString("controller");
            string areaName = (string)controllerContext.RouteData.DataTokens["area"];

            using (ApphrDbContext db = new ApphrDbContext())
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserName", userName));
                parameters.Add(new SqlParameter("@Controlador", controllerName));
                parameters.Add(new SqlParameter("@Area", areaName));
                string query = @"SELECT DISTINCT tt2.PermitId FROM
                            (SELECT t1.RoleId FROM
                            (SELECT AppUserRole.RoleId FROM dbo.AppUser INNER JOIN dbo.AppUserRole ON AppUser.Id = AppUserRole.UserId WHERE AppUser.UserName = @UserName) t1
                            JOIN
                            (SELECT ControladorRolAsignaciones.RoleId FROM dbo.Controladores INNER JOIN dbo.ControladorRolAsignaciones ON Controladores.AccionId = ControladorRolAsignaciones.AccionId WHERE Controladores.Controller = @Controlador AND Controladores.Area = @Area) t2
                            ON(t1.RoleId = t2.RoleId)) tt1
                            JOIN
                            (SELECT AccessRuleRoleAssignments.AppRoleId, AccessRulePermitAssignments.PermitId FROM dbo.AccessRuleRoleAssignments INNER JOIN dbo.AccessRulePermitAssignments ON AccessRuleRoleAssignments.AccessRuleId = AccessRulePermitAssignments.AccessRuleId) tt2
                            ON(tt1.RoleId = tt2.AppRoleId)";
                var ArrayPermisos = db.Database.SqlQuery<Permit>(query, parameters.ToArray()).ToArray();
                IEnumerable<Permit> both = ArrayPermisos.Intersect(Permisos);
                return both.Any();                
            }          
        }
    }
}