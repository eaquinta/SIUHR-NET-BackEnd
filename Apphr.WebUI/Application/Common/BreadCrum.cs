using System.Collections.Generic;

namespace Apphr.Application.Common
{
    public class BreadCrum : LinkInfo
    {
        public BreadCrum(string name, string page, string animation = "fade-left", string icon = "", bool active = false, bool showName = true, Dictionary<string, string> routeParams = null) :
            base(name, icon, page, showName, routeParams)
        {
            Animation = animation;
            Active = active;
        }
        public string Animation { get; set; }

        public bool Active { get; set; }
    }
}
