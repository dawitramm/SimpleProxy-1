using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SimpleProxy.Library
{
    internal sealed class WebRequestAdapter : IWebRequest
    {
        private readonly HttpListenerRequest _request;
        private Dictionary<string, string> _headers;
        private Dictionary<string, string> _queryString;

        public WebRequestAdapter(HttpListenerRequest request)
        {
            _request = request;
        }

        public string[] AcceptTypes
        {
            get { return _request.AcceptTypes; }
        }
        
        public Encoding ContentEncoding
        {
            get { return _request.ContentEncoding; }
        }

        public long ContentLength
        {
            get { return _request.ContentLength64; }
        }

        public string ContentType
        {
            get { return _request.ContentType; }
        }

        public IEnumerable<Cookie> Cookies
        {
            get 
            {
                for (var i = 0; i < _request.Cookies.Count; i++)
                    yield return _request.Cookies[i];
            }
        }
        
        public bool HasEntityBody
        {
            get { return _request.HasEntityBody; }
        }

        public IDictionary<string, string> Headers
        {
            get 
            { 
                if (_headers == null)
                {
                    _headers = new Dictionary<string, string>();

                    for (var i = 0; i < _request.Headers.Count; i++)
                        _headers.Add(_request.Headers.GetKey(i), _request.Headers.Get(i));
                }
                return _headers.ReadOnly();
            }
        }

        public string Host
        {
            get { return _request.UserHostName; }
        }

        public Stream InputStream
        {
            get { return _request.InputStream; }
        }
        
        public bool IsSecureConnection
        {
            get { return _request.IsSecureConnection; }
        }

        public bool KeepAlive
        {
            get { return _request.KeepAlive; }
        }

        public string Method
        {
            get { return _request.HttpMethod; }
        }
        
        public Version ProtocolVersion
        {
            get { return _request.ProtocolVersion; }    
        }

        public IDictionary<string, string> QueryString
        {
            get
            {
                if (_queryString == null)
                {
                    _queryString = new Dictionary<string, string>();

                    for (var i = 0; i < _request.QueryString.Count; i++)
                        _queryString.Add(_request.QueryString.GetKey(i), _request.QueryString.Get(i));
                }
                return _queryString.ReadOnly();
            }
        }

        public string RawUrl
        {
            get { return _request.RawUrl; }
        }
        
        public Uri Url
        {
            get { return _request.Url; }
        }

        public Uri Referer
        {
            get { return _request.UrlReferrer; }
        }

        public string UserAgent
        {
            get { return _request.UserAgent; }
        }

        public IEnumerable<string> UserLanguages
        {
            get { return _request.UserLanguages; }
        }

        public Stream GetStream()
        {
            return _request.InputStream;
        }
    }
}