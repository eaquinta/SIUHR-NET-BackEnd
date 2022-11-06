using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.CustomAttributes
{

    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        //private bool localAllowed;
        private string claimType;
        private string claimValue;
        public string ClaimType { get => claimType; protected set => claimType = value; }
        public string ClaimValue { get => claimValue; protected set => claimValue = value; }

        public ClaimsAuthorizeAttribute(string type, string value = "")
        {
            this.ClaimType = type;
            this.ClaimValue = value;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // assume not authorised
            bool isAuthorised = false;

            // check user exists
            if (filterContext.HttpContext.User != null)
            {
                // get user by claim principle
                var user = filterContext.HttpContext.User as System.Security.Claims.ClaimsPrincipal;

                if (user != null && user.HasClaim(ClaimType, ClaimValue))
                {
                    // user has a claim of the correct type
                    isAuthorised = true;
                }
            }

            if (isAuthorised)
            {
                filterContext.Result = null;
                base.OnAuthorization(filterContext);
            }
            else
            {
                //filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You are forbidden to access this resource");
                base.HandleUnauthorizedRequest(filterContext);
            }
        }       
    }
}