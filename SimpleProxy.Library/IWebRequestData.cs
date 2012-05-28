using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Defines the metadata of a web request.
    /// </summary>
    public interface IWebRequestData
    {
        /// <summary>
        /// Gets a string array of client-supported MIME accept types.
        /// </summary>
        string[] AcceptTypes { get; }

        /// <summary>
        /// Gets or sets the character set of the entity-body.
        /// </summary>
        Encoding ContentEncoding { get; }

        /// <summary>
        /// Specifies the length, in bytes, of content sent by the client.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the MIME content type of the incoming request.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets a collection of cookies sent by the client.
        /// </summary>
        IEnumerable<Cookie> Cookies { get; }

        /// <summary>
        /// Gets a value that indicating whether the request has associated body data.
        /// </summary>
        bool HasEntityBody { get; }

        /// <summary>
        /// Gets a collection of HTTP headers.
        /// </summary>
        IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the DNS name of the remote client.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Gets a value indicating whether the HTTP connection uses secure sockets (that is, HTTPS).
        /// </summary>
        bool IsSecureConnection { get; }

        /// <summary>
        /// Gets a value indicating whether the client requests a persistent connection.
        /// </summary>
        bool KeepAlive { get; }

        /// <summary>
        /// Gets the HTTP data transfer method (such as GET, POST, or HEAD) used by the client.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the version of HTTP to use for the request.
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the collection of HTTP query string variables.
        /// </summary>
        IDictionary<string, string> QueryString { get; }

        /// <summary>
        /// Gets the raw URL of the current request.
        /// </summary>
        string RawUrl { get; }

        /// <summary>
        /// Gets information about the URL of the current request.
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets information about the URL of the client's previous request that linked to the current URL.
        /// </summary>
        Uri Referer { get; }

        /// <summary>
        /// Gets the raw user agent string of the client browser.
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Gets a sorted string array of client language preferences.
        /// </summary>
        IEnumerable<string> UserLanguages { get; }
    }
}