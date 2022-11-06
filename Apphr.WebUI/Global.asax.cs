//-------------------------------------------------------
// Created By: Estuardo Quintanilla
//       Date: unknow
//  Edited By: Estuardo Quintanilla
//       Date: 23/08/2022 06:26
//Information: Se agrega para soporte Web.Api
//-------------------------------------------------------

using Apphr.Application;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Apphr.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperConfig.Register();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register); // Agregado para soporte WebApi 23/08/2022
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
