using System.Web.Mvc;

namespace Apphr.WebUI.Areas.Ortopedia
{
    public class OrtopediaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ortopedia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ortopedia_default",
                "Ortopedia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}