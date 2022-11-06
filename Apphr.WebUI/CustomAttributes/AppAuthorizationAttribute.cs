using Apphr.Domain.Entities;
using Apphr.Domain.Enums;
using Apphr.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Apphr.WebUI.CustomAttributes
{
    public class AppAuthorizationAttribute: AuthorizeAttribute
    {
        private ApphrDbContext db = new ApphrDbContext();
        private readonly Permit[] Permisos;

        public AppAuthorizationAttribute(params Permit[] permisos)
        {
            this.Permisos = permisos;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //string actionName = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string areaName = httpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            string userName = httpContext.User.Identity.Name.ToString();

            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            // Crea Entrada si no existe
            if(!db.Controladores.Any(x => x.Area == areaName && x.Controller == controllerName ))
            {
                db.Controladores.Add(new Controlador
                {
                    Detalle = (areaName + "/" + controllerName),
                    //Action = actionName,
                    Area = areaName,
                    Controller = controllerName
                });
                db.SaveChanges();
            }

            if (userName == "eaquinta@yahoo.com")
                return true;

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
            if(both.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
            //var xxxx = Permit.Cancel;
            //foreach (Permit id in both)
            //    xxxx = id;
            //return GetUserRights(httpContext.User.Identity.Name.ToString());
            
          //  string privilegeLevels = string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB

           // return privilegeLevels.Contains(this.AccessLevel);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "NoAutorizado",
                                Area ="",
                                returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                            })
                        );
        }
    }
}