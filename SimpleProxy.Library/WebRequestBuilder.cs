using System;
using System.Linq;
using System.Net;

namespace SimpleProxy.Library
{
    public class ProxyRequestBuilder
    {
        private static readonly string[] PropertyHeaders = new[]
        {
            "Accept",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Expect",
            "Date",
            "Host",
            "If-Modified-Since",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent"
        };

        private readonly IWebRequestFactory _webRequestFactory;
        private readonly IWebRequest _request;

        public ProxyRequestBuilder(IWebRequestFactory webRequestFactory, IWebRequest request)
        {
            _webRequestFactory = webRequestFactory;
            _request = request;
        }

        public IMutableWebRequest Build()
        {
            var proxyRequest = _webRequestFactory.CreateRequest(_request.RawUrl);

            // Set property headers
            proxyRequest.Method = _request.Method;
            proxyRequest.ProtocolVersion = _request.ProtocolVersion;
            proxyRequest.Accept = String.Join(";", _request.AcceptTypes);
            proxyRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            proxyRequest.ContentLength = _request.ContentLength;
            proxyRequest.ContentType = _request.ContentType;
            proxyRequest.Host = _request.Host;
            proxyRequest.KeepAlive = _request.KeepAlive;
            proxyRequest.UserAgent = _request.UserAgent;

            // Set property headers from headers
            if (_request.Headers.ContainsKey("Connection"))
            {
                proxyRequest.Connection = _request.Headers["Connection"];
            }
            if (_request.Headers.ContainsKey("Date"))
            {
                DateTime date;
                if (DateTime.TryParse(_request.Headers["Date"], out date))
                    proxyRequest.Date = date;
            }
            if (_request.Headers.ContainsKey("Expect"))
            {
                proxyRequest.Expect = _request.Headers["Expect"];
            }
            if (_request.Headers.ContainsKey("If-Modified-Since"))
            {
                DateTime ifModifiedSince;
                if (DateTime.TryParse(_request.Headers["If-Modified-Since"], out ifModifiedSince))
                    proxyRequest.IfModifiedSince = ifModifiedSince;
            }
            if (_request.Headers.ContainsKey("Referer"))
            {
                proxyRequest.Referer = _request.Headers["Referer"];
            }

            // Set cookies
            foreach (var cookie in _request.Cookies)
            {
                if (String.IsNullOrWhiteSpace(cookie.Domain))
                    cookie.Domain = _request.Host;

                proxyRequest.Cookies.Add(cookie);
            }
            // Set other headers
            foreach (var header in _request.Headers)
            {
                if (header.Key.StartsWith("Proxy-"))
                    continue; // Ignore proxy headers
                if (PropertyHeaders.Contains(header.Key))
                    continue; // Ignore headers set in properties.

                proxyRequest.Headers.Add(header.Key, header.Value);
            }

            if (_request.HasEntityBody)
            {
               // Copy over request stream
                var proxyStream = proxyRequest.GetStream();
                var requestStream = _request.GetStream();
                requestStream.CopyTo(proxyStream); 
            }
            return proxyRequest;
        }
    }
}