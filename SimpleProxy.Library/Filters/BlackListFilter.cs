using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using SimpleProxy.Library.Plugins;

namespace SimpleProxy.Library.Filters
{
    public class BlacklistFilter : IWebRequestFilter
    {
        public BlacklistFilter()
        {
            HostRegExs = new List<String>();
        }

        public List<string> HostRegExs { get; private set; }

        public string Name
        {
            get { return "SimpleProxy blacklist filter"; }
        }

        public bool Enabled
        {
            get { return true; }
        }

        public FilterResult Filter(IWebRequestData request)
        {
            foreach (var regex in HostRegExs)
            {
                if (Regex.IsMatch(request.Host, regex) ||
                    (request.Referer != null && Regex.IsMatch(request.Referer.Host, regex)))
                {
                    return FilterResult.Block("This page is blocked by SimpleProxy blacklist filter", HttpStatusCode.Forbidden);
                }
            }
            return FilterResult.Allow;
        }
    }
}
