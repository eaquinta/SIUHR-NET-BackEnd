using System;
using System.Collections.Generic;
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
                using (var db = new ApphrDbContext())
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