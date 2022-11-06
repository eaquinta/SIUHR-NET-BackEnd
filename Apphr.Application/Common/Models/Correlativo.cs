using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Common.Models
{
    public class Correlativo
    {
        public static string Get()
        {
            string Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            return Usuario.ToUpper().Substring(0, 3) + " " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        }
    }
}
