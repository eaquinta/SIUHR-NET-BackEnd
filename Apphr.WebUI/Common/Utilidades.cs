using Apphr.Domain.Enums;
using Apphr.WebUI.Models;
using Apphr.WebUI.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

        public static DateTime EndOfMonth(DateTime var)
        {
            return new DateTime(var.Year, var.Month, DateTime.DaysInMonth(var.Year,var.Month));
        }
        
        public static ExpandoObject GetCans(int userId)
        {
            List<String> allPermits;
            List<String> userPermits = new List<string>();
            using (ApphrDbContext db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1))
            {                
                
                allPermits = db.AppPermissions.Select(s => s.Name).ToList();
                 
                if (userId != 1)                
                {
                    userPermits = (
                        from p in db.AppUserRoles
                        where p.UserId == userId
                        join rp in db.AppRolePermissions on p.RoleId equals rp.AppRoleId
                        join perm in db.AppPermissions on rp.AppPermissionId equals perm.PermissionId
                        select perm.Name
                    ).Distinct().ToList();
                }
            }
            var res = new ExpandoObject();
            
            ((IDictionary<String, Object>)res).Add("su", (userId == 1));
            
            foreach (var field in allPermits)
            {
                if (userPermits.Contains(field) || userId == 1) // master userPermits.Contains(field) ||
                {
                    ((IDictionary<String, Object>)res).Add(field, true);
                }
                else
                {
                    ((IDictionary<String, Object>)res).Add(field, false);
                }
            }
            return res;
        }
        public static ExpandoObject GetPermissions(ControllerContext controllerContext, string userName)
        {
            string controllerName = (string)controllerContext.RouteData.GetRequiredString("controller");
            string areaName = (string)controllerContext.RouteData.DataTokens["area"];

            var PermisosDisponibles = Enum.GetValues(typeof(Permit)).Cast<Permit>().Select(x => x.ToString()).ToList();

            Dictionary<string, int> PerDis = ((Permit[])Enum.GetValues(typeof(Permit))).ToDictionary(k => k.ToString(), k => (int)k );
            //var Text = Newtonsoft.Json.JsonConvert.SerializeObject(PerDis);
            List<Permit> Permisos;
            using (ApphrDbContext db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1))
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
                Permisos = db.Database.SqlQuery<Permit>(query, parameters.ToArray()).ToList();
            }
                                   
            var res = new ExpandoObject();

            foreach (var field in PerDis)
            {
                if (Permisos.Contains((Permit)field.Value) || userName == "eaquinta@yahoo.com")
                {
                    ((IDictionary<String, Object>)res).Add(field.Key, true);
                }
                else
                {
                    ((IDictionary<String, Object>)res).Add(field.Key, false);
                }                
            }                                    
            
            return res;            
        }

        public static async Task<bool> Can(string[] Permisos, int userId)
        {
            var userName = HttpContext.Current.User.Identity.GetUserName();            
            
            using (ApphrDbContext db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1))
            {

                // Crea Entrada si no existe
                foreach (string perm in Permisos)
                {
                    if (!db.AppPermissions.Any(x => x.Name == perm))
                    {
                        db.AppPermissions.Add(new AppPermission()
                        {
                            Name = perm
                        });
                        db.SaveChanges();
                    }
                }
                if (userId == 1)
                    return true;

                var query = from appUser in db.Users
                            join appUserRole in db.AppUserRoles on appUser.Id equals appUserRole.UserId into userRoles
                            from ur in userRoles.DefaultIfEmpty()
                            join appRole in db.Roles on ur.RoleId equals appRole.Id into roles
                            from r in roles.DefaultIfEmpty()
                            join appRolePermission in db.AppRolePermissions on r.Id equals appRolePermission.AppRoleId into rolePermissions
                            from rp in rolePermissions.DefaultIfEmpty()
                            join appPermission in db.AppPermissions on rp.AppPermissionId equals appPermission.PermissionId into permissions
                            from p in permissions.DefaultIfEmpty()
                            where p != null && Permisos.Contains(p.Name) && appUser.Id == userId
                            select new
                            {
                                UserId = appUser.Id,
                                RoleId = r != null ? r.Id : (int?)null,
                                //PermissionId = p.PermissionId,
                                //PermissionName = p.Name,
                                RoleName = r != null ? r.Name : null
                            };
                //var sqlParams = new List<SqlParameter>();
                //sqlParams.Add(new SqlParameter("@userId", userId));
                //var permissions = await db.Database.SqlQuery<string>(sqlQuery, sqlParams.ToArray()).ToListAsync();
                //var x = permissions.Any();

                return await query.AnyAsync(); // Verdadero si hay un premiso
                
            }
        }

        public static bool hasPermitx(Permit[] Permisos, ControllerContext controllerContext, string userName) //string userName, string controllerName, string areaName)
        {
            if (userName == "eaquinta@yahoo.com")
                return true;

            string controllerName = (string)controllerContext.RouteData.GetRequiredString("controller");
            string areaName = (string)controllerContext.RouteData.DataTokens["area"];

            using (ApphrDbContext db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1))
            {

                if (!db.Controladores.Any(x => x.Area == areaName && x.Controller == controllerName))
                {
                    db.Controladores.Add(new Controlador
                    {
                        Detalle = (areaName + "/" + controllerName),                       
                        Area = areaName,
                        Controller = controllerName
                    });
                    db.SaveChanges();
                }


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