using System;
using System.Collections.Generic;
using System.Net;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Defines metadata of a web response.
    /// </summary>
    public interface IWebResponseData
    {
        /// <summary>
        /// Gets the method that is used to encode the body of the response.
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// Gets the content type of the response.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the length of the content returned by the request. 
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the cookies that are associated with this response.
        /// </summary>
        IEnumerable<Cookie> Cookies { get; }

        /// <summary>
        /// Gets the headers that are associated with this response from the server.
        /// </summary>
        IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the method that is used to return the response.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the version of the HTTP protocol that is used in the response.
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the URI of the Internet resource that responded to the request.
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets the name of the server that sent the response.
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        string StatusDescription { get; }
    }
}