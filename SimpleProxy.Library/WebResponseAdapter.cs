using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SimpleProxy.Library
{
    internal sealed class WebResponseAdapter : IWebResponse
    {
        private readonly HttpWebResponse _response;
        private Dictionary<string, string> _headers;

        public WebResponseAdapter(HttpWebResponse response)
        {
            _response = response;
        }

        public string ContentEncoding
        {
            get { return _response.ContentEncoding; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
        }

        public long ContentLength
        {
            get { return _response.ContentLength; }
        }

        public IEnumerable<Cookie> Cookies
        {
            get { return _response.Cookies.Cast<Cookie>(); }
        }
        
        public IDictionary<string, string> Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = new Dictionary<string, string>();

                    for (var i = 0; i < _response.Headers.Count; i++)
                        _headers.Add(_response.Headers.GetKey(i), _response.Headers.Get(i));
                }
                return _headers.ReadOnly();
            }
        }

        public string Method
        {
            get { return _response.Method; }
        }

        public Version ProtocolVersion
        {
            get { return _response.ProtocolVersion; }
        }

        public Uri ResponseUri
        {
            get { return _response.ResponseUri; }
        }

        public string Server
        {
            get { return _response.Server; }
        }
        
        public HttpStatusCode StatusCode
        {
            get { return _response.StatusCode; }
        }

        public string StatusDescription
        {
            get { return _response.StatusDescription; }
        }

        public Stream GetStream()
        {
            return _response.GetResponseStream();
        }

        public void Dispose()
        {
            _response.Close();
        }
    }
}