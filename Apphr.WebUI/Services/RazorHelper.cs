using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Apphr.WebUI.Services
{
    public class RazorHelper
    {
        public static string RenderViewToString(ControllerContext controllerContext, string viewName, object model, bool isPartial = false)  //
        {


            //ViewDataDictionary vd = new ViewDataDictionary(model);
            //ViewPage vp = new ViewPage { ViewData = vd };
            //Control control = vp.LoadControl(viewName);

            //vp.Controls.Add(control);

            //StringBuilder sb = new StringBuilder();

            //using (StringWriter sw = new StringWriter(sb))
            //{
            //    using (HtmlTextWriter tw = new HtmlTextWriter(sw))
            //    {
            //        vp.RenderControl(tw);
            //    }
            //}

            //return sb.ToString();
            return "";
        }
    }
}