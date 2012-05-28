
using System;
using System.Net;

namespace SimpleProxy.Library
{
    public sealed class ProxyResponseException : Exception
    {
        public ProxyResponseException(IWebResponse response)
        {
            Response = response;
        }

        public ProxyResponseException(IWebResponse response, string message, Exception innerException) 
            : base (message, innerException)
        {
            Response = response;
        }

        internal ProxyResponseException(WebException wEx) : base(wEx.Message, wEx.InnerException)
        {
            if (wEx.Response != null)
                Response = new WebResponseAdapter((HttpWebResponse)wEx.Response);

            Status = wEx.Status;
        }

        public WebExceptionStatus Status { get; private set; }

        public IWebResponse Response { get; private set; }
    }
}
