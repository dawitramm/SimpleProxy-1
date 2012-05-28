using System.Diagnostics;
using System.Linq;
using System.Net;
using SimpleProxy.Library;
using SimpleProxy.Library.Configuration;
using SimpleProxy.Library.Filters;
using SimpleProxy.Library.Plugins;

namespace SimpleProxy.Console
{
    class Program
    {
        static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var configuration = SimpleProxyConfigurationSection.GetConfigSection();

            var proxy = new Proxy(
                new HttpListenerFactory(),
                new WebRequestFactory(),
                configuration.Listeners.Cast<ListenerElement>().Select(t => t.Prefix),
                AuthenticationSchemes.Anonymous);

            foreach (RequestFilterElement requestFilter in configuration.RequestFilters)
            {
                var filter = PluginLoader.LoadRequestFilter(requestFilter.Type);

                if (filter is BlacklistFilter)
                {
                    var blackListFilter = (BlacklistFilter)filter;
                    foreach (RegexElement regex in configuration.Blacklist)
                        blackListFilter.HostRegExs.Add(regex.Regex);
                }
                if (filter is WhitelistFilter)
                {
                    var whitelistFilter = (WhitelistFilter)filter;
                    foreach (RegexElement regex in configuration.Whitelist)
                        whitelistFilter.HostRegExs.Add(regex.Regex);
                }
                proxy.AddRequestFilter(filter);
            }
            Trace.TraceInformation("Starting proxy server...");
            proxy.Start();

            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadKey();

            Trace.TraceInformation("Stopping proxy server...");
            proxy.Stop();
        }
    }
}
