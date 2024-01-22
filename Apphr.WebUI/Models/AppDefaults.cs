using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models
{
    public class AppDefaults
    {
        public static string GetValue(string name)
        {
            try
            {
                using (var db = new ApphrDbContext((HttpContext.Current.User != null) ? HttpContext.Current.User.Identity.GetUserId<int>() : -1))
                {
                    return db.Defaults.Where(x => x.Name == name).FirstOrDefault().Value;
                }
            }
            catch (Exception)
            {

                return "";
            }
            
        }
    }
}