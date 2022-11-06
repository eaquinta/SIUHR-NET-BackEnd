//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Apphr.WebUI.Services
//{
//    public class CurrenRequestService // : ICurrenRequestService
//    {
//        public string User
//        {
//            get
//            {
//                //return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) == null ? -2147483648 : System.Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
//                //return !string.IsNullOrEmpty(HttpContext.Current?.User?.Identity?.Name)
//                //? HttpContext.Current.User.Identity.Name
//                //: "Anonymous";
//                if (HttpContext.Current == null)
//                    return "Anonymous";
//                else
//                    return "si";
//            }
//        }
//    }
//}