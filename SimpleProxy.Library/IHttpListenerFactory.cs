using System.Collections.Generic;
using System.Net;

namespace SimpleProxy.Library
{
    public interface IHttpListenerFactory
    {
        IHttpListener CreateListener(IEnumerable<string> prefixes, AuthenticationSchemes authenticationSchemes);
    }
}