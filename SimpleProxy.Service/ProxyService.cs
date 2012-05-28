using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using SimpleProxy.Library;
using SimpleProxy.Library.Configuration;
using SimpleProxy.Library.Filters;
using SimpleProxy.Library.Plugins;

namespace SimpleProxy.Service
{
    public partial class ProxyService : ServiceBase
    {
        private Proxy _proxy;

        public ProxyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var configuration = SimpleProxyConfigurationSection.GetConfigSection();

            _proxy = new Proxy(
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
                _proxy.AddRequestFilter(filter);
            }
            Trace.TraceInformation("Starting proxy server...");
            _proxy.Start();
        }

        protected override void OnStop()
        {
            _proxy.Stop();
        }
    }
}
