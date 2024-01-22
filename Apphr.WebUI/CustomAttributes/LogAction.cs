using Apphr.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Apphr.WebUI.CustomAttributes
{
    public class LogAction : ActionFilterAttribute
    {
        private Stopwatch stopWatch;
        private int UserId;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous";
            UserId = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.GetUserId<int>() : -1;
            var IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            var action = (string)filterContext.RouteData.Values["action"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var browser = filterContext.HttpContext.Request.UserAgent;
            var url = filterContext.HttpContext.Request.Url.ToString();
            var method = filterContext.HttpContext.Request.HttpMethod;
            //filterContext.HttpContext.Items["timer"] = Stopwatch.StartNew();
            stopWatch = new Stopwatch();

            LogDatabase(UserName, IPAddress, action, controller, browser, url, method);
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

        private void LogDatabase(string userName, string ipAddress, string action, string controller, string browser, string url, string method)
        {
            var log = new AppAuditLog()
            {
                AuditLogID = Guid.NewGuid(),
                //UserName = userName, //C20230613
                UserId = this.UserId,
                Browser = browser,
                //Client = "", //C20230613
                Duration = "1",
                IpAddress = ipAddress,
                Action = action,
                Controller = controller,
                Date = DateTime.Now,
                Url = url,
                Method = method
            };
            using (var db = new ApphrDbContext(this.UserId)) // userName))
            {
                db.AppAuditLogs.Add(log);
                db.SaveChanges();
            }
        }
    }
}