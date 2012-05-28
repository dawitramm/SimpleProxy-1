using System.Collections.Generic;
using System.Net;

namespace SimpleProxy.Library
{
    public class HttpListenerFactory : IHttpListenerFactory
    {
        public IHttpListener CreateListener(IEnumerable<string> prefixes, AuthenticationSchemes authenticationSchemes)
        {
            return new HttpListenerAdapter(prefixes, authenticationSchemes);
        }
    }
}