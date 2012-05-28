using System;
using System.IO;
using System.Net;
using System.Text;

namespace SimpleProxy.Library
{
    internal sealed class MutableWebResponseAdapter : IMutableWebResponse
    {
        private readonly HttpListenerResponse _response;

        public MutableWebResponseAdapter(HttpListenerResponse response)
        {
            _response = response;
        }

        public Encoding ContentEncoding
        {
            get { return _response.ContentEncoding; }
            set { _response.ContentEncoding = value; }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public long ContentLength
        {
            get { return _response.ContentLength64; }
            set { _response.ContentLength64 = value; }
        }

        public CookieCollection Cookies
        {
            get
            {
                if (_response.Cookies == null)
                    _response.Cookies = new CookieCollection();

                return _response.Cookies;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                if (_response.Headers == null)
                    _response.Headers = new WebHeaderCollection();

                return _response.Headers;
            }
        }

        public bool KeepAlive
        {
            get { return _response.KeepAlive; }
            set { _response.KeepAlive = value; }
        }

        public Version ProtocolVersion
        {
            get { return _response.ProtocolVersion; }
            set { _response.ProtocolVersion = value; }
        }

        public string RedirectLocation
        {
            get { return _response.RedirectLocation; }
            set { _response.RedirectLocation = value; }
        }

        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        public string StatusDescription
        {
            get { return _response.StatusDescription; }
            set { _response.StatusDescription = value; }
        }


        public Stream GetStream()
        {
            return _response.OutputStream;
        }

        public void Dispose()
        {
            _response.Close();
        }
    }
}