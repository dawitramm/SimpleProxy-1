using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using SimpleProxy.Library.Plugins;

namespace SimpleProxy.Library
{
    public sealed class Proxy
    {
        private readonly List<IWebRequestFilter> _requestFilters;
        private readonly List<IWebResponseFilter> _responseFilters;
        private readonly ProxyRequestProcessor _requestProcessor;
        private readonly IHttpListener _listener;
        
        public Proxy(IHttpListenerFactory listenerFactory, IWebRequestFactory webRequestFactory,
            IEnumerable<string> prefixes, AuthenticationSchemes authenticationSchemes)
        {
            _requestFilters = new List<IWebRequestFilter>();
            _responseFilters = new List<IWebResponseFilter>();

            _requestProcessor = new ProxyRequestProcessor(webRequestFactory);
            _listener = listenerFactory.CreateListener(prefixes, authenticationSchemes);
            _listener.Request += ListenerRequest;
        }

        public void AddRequestFilter(IWebRequestFilter filter)
        {
            _requestFilters.Add(filter);
        }

        public void AddResponseFilter(IWebResponseFilter filter)
        {
            _responseFilters.Add(filter);
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void ListenerRequest(object sender, ListenerRequestEventArgs e)
        {
            try
            {
                _requestProcessor.ProcessRequest(e.Request, e.Response, e.User, _requestFilters, _responseFilters);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error processing request: {0}", ex);
            }
            finally
            {
                e.Response.Dispose();
            }
        }
    }
}
