using System.Collections.Generic;

namespace Apphr.Application.Common
{
    public class LinkInfo
    {
        public LinkInfo(string name, string icon, string page, bool showName, Dictionary<string, string> routeParams = null)
        {
            Name = name;
            Icon = icon;
            Page = page;
            ShowName = showName;
            RouteParams = routeParams;
        }
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Page { get; set; }

        public bool ShowName { get; set; }

        public Dictionary<string, string> RouteParams { get; set; }
    }
}
