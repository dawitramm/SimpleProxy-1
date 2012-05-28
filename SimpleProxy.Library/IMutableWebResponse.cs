using System;
using System.Net;
using System.Text;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Mutable web response object.
    /// </summary>
    public interface IMutableWebResponse : IWebStreamProvider, IDisposable
    {
        /// <summary>
        /// Gets the method that is used to encode the body of the response.
        /// </summary>
        Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets the content type of the response.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets the length of the content returned by the request. 
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>
        /// Gets the cookies that are associated with this response.
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets the headers that are associated with this response from the server.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the server requests a persistent connection.
        /// </summary>
        bool KeepAlive { get; set; }
        
        /// <summary>
        /// Gets the version of the HTTP protocol that is used in the response.
        /// </summary>
        Version ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the value of the HTTP Location header in this response.
        /// </summary>
        string RedirectLocation { get; set; }
        
        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Gets the description of the response.
        /// </summary>
        string StatusDescription { get; set; }
    }
}