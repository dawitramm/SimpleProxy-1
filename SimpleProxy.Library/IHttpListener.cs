using System;
using System.Collections.Generic;
using System.Net;

namespace SimpleProxy.Library
{
    public interface IHttpListener
    {
        event EventHandler<ListenerRequestEventArgs> Request;
        IEnumerable<string> Prefixes { get; }
        AuthenticationSchemes AuthenticationSchemes { get; }
        void Start();
        void Stop();
    }
}