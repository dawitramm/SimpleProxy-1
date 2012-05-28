using System;
using System.IO;
using System.Net;

namespace SimpleProxy.Library
{
    internal sealed class MutableWebRequestAdapter : IMutableWebRequest
    {
        private readonly HttpWebRequest _request;

        public MutableWebRequestAdapter(HttpWebRequest request)
        {
            _request = request;
        }

        public string Accept
        {
            get { return _request.Accept; }
            set { _request.Accept = value; }
        }

        public DecompressionMethods AutomaticDecompression
        {
            get { return _request.AutomaticDecompression; }
            set { _request.AutomaticDecompression = value; }
        }

        public string Connection
        {
            get { return _request.Connection; }
            set { _request.Connection = value; }
        }

        public long ContentLength
        {
            get { return _request.ContentLength; }
            set { _request.ContentLength = value; }
        }

        public string ContentType
        {
            get { return _request.ContentType; }
            set { _request.ContentType = value; }
        }

        public CookieContainer Cookies
        {
            get
            {
                if (_request.CookieContainer == null)
                    _request.CookieContainer = new CookieContainer();

                return _request.CookieContainer;
            }
        }
        
        public DateTime Date
        {
            get { return _request.Date; }
            set { _request.Date = value; }
        }

        public string Expect
        {
            get { return _request.Expect; }
            set { _request.Expect = value; }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                if (_request.Headers == null)
                    _request.Headers = new WebHeaderCollection();

                return _request.Headers;
            }
        }

        public string Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }

        public DateTime IfModifiedSince
        {
            get { return _request.IfModifiedSince; }
            set { _request.IfModifiedSince = value; }
        }

        public bool KeepAlive
        {
            get { return _request.KeepAlive; }
            set { _request.KeepAlive = value; }
        }

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public Version ProtocolVersion
        {
            get { return _request.ProtocolVersion; }
            set { _request.ProtocolVersion = value; }
        }
        
        public string Referer
        {
            get { return _request.Referer; }
            set { _request.Referer = value; }
        }

        public string UserAgent
        {
            get { return _request.UserAgent; }
            set { _request.UserAgent = value; }
        }
        
        public IWebResponse GetResponse()
        {
            try
            {
                return new WebResponseAdapter((HttpWebResponse) _request.GetResponse());
            }
            catch (WebException wEx)
            {
                throw new ProxyResponseException(wEx);
            }
        }

        public Stream GetStream()
        {
            return _request.GetRequestStream();
        }
    }
}