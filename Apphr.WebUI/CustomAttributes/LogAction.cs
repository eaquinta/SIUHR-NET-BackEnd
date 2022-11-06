using Apphr.WebUI.Models;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace Apphr.WebUI.CustomAttributes
{
    public class LogAction : ActionFilterAttribute
    {
        private Stopwatch stopWatch;        

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous";
            var IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            var action = (string)filterContext.RouteData.Values["action"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var browser = filterContext.HttpContext.Request.UserAgent;
            //filterContext.HttpContext.Items["timer"] = Stopwatch.StartNew();
            stopWatch = new Stopwatch();

            LogDatabase(UserName, IPAddress, action, controller, browser);
            Log("OnActionExecuting", filterContext.RouteData);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopWatch.Stop();
            var duration = stopWatch.Elapsed.ToString();
            Log("OnActionExecuted", filterContext.RouteData);
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
            base.OnResultExecuted(filterContext);
        }


        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
            
        }

        private void LogDatabase(string userName, string ipAddress, string action, string controller, string browser)
        {
            var log = new AppAuditLog()
            {
                AuditLogID = Guid.NewGuid(),
                UserName = userName,
                Browser = browser,
                Client = "",
                Duration = "1",
                IpAddress = ipAddress,
                Action = action,
                Controller = controller,
                Date = DateTime.Now
            };
            using (var db = new ApphrDbContext(userName))
            {
                db.AppAuditLogs.Add(log);
                db.SaveChanges();
            }
        }
    }
}