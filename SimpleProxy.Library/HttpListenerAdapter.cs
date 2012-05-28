using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace SimpleProxy.Library
{
    internal sealed class HttpListenerAdapter : IHttpListener
    {
        private readonly HttpListener _listener;

        public HttpListenerAdapter(IEnumerable<string> prefixes, AuthenticationSchemes authenticationSchemes)
        {
            _listener = new HttpListener
            {
                IgnoreWriteExceptions = true,
                AuthenticationSchemes = authenticationSchemes
            };
            foreach (var prefix in prefixes)
                _listener.Prefixes.Add(prefix);
        }

        public event EventHandler<ListenerRequestEventArgs> Request;

        public IEnumerable<string> Prefixes
        {
            get { return _listener.Prefixes; }
        }

        public AuthenticationSchemes AuthenticationSchemes
        {
            get { return _listener.AuthenticationSchemes; }
        }

        public void Start()
        {
            _listener.Start();
            _listener.BeginGetContext(GetContextCallback, null);
        }

        private void GetContextCallback(IAsyncResult result)
        {
            try
            {
                if (_listener.IsListening)
                    _listener.BeginGetContext(GetContextCallback, null);

                var context = _listener.EndGetContext(result);
                var temp = Request;
                if (temp != null)
                    temp(this, new ListenerRequestEventArgs(
                        context.User, 
                        new WebRequestAdapter(context.Request), 
                        new MutableWebResponseAdapter(context.Response)));
            }
            catch (Exception ex)
            {
                Trace.TraceError("An error occured while processing the request: {0}", ex);
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}