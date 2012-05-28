using System;
using System.Net;

namespace SimpleProxy.Library
{
    public interface IMutableWebRequest : IWebStreamProvider
    {
        /// <summary>
        /// Gets or sets the value of the Accept HTTP header.
        /// </summary>
        string Accept { get; set; }

        /// <summary>
        /// Gets or sets the type of decompression that is used.
        /// </summary>
        DecompressionMethods AutomaticDecompression { get; set; }

        /// <summary>
        /// Gets or sets the value of the Connection HTTP header.
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Gets or sets the Content-length HTTP header.
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the value of the Content-type HTTP header.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets the cookies associated with the request.
        /// </summary>
        CookieContainer Cookies { get; }

        /// <summary>
        /// Get or set the Date HTTP header value to use in an HTTP request.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the value of the Expect HTTP header.
        /// </summary>
        string Expect { get; set; }

        /// <summary>
        /// Specifies a collection of the name/value pairs that make up the HTTP headers.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Get or set the Host header value to use in an HTTP request independent from the request URI.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the value of the If-Modified-Since HTTP header.
        /// </summary>
        DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the method for the request
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Gets or sets the version of HTTP to use for the request.
        /// </summary>
        Version ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the value of the Referer HTTP header.
        /// </summary>
        string Referer { get; set; }

        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header.
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Get response from the requested resource.
        /// </summary>
        IWebResponse GetResponse();
    }
}