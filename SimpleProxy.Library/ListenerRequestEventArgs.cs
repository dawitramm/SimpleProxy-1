using System;
using System.Security.Principal;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Provides data for a request event
    /// </summary>
    public class ListenerRequestEventArgs : EventArgs
    {
        /// <param name="user">Identity, authentication, and security information of the client making the request.</param>
        /// <param name="request">Client request</param>
        /// <param name="response">Response that will be sent back to the client</param>
        public ListenerRequestEventArgs(IPrincipal user, IWebRequest request, IMutableWebResponse response)
        {
            User = user;
            Request = request;
            Response = response;
        }

        /// <summary>
        /// Gets identity, authentication, and security information of the client making the request.
        /// </summary>
        public IPrincipal User { get; private set; }

        /// <summary>
        /// Represents a clients request.
        /// </summary>
        public IWebRequest Request { get; private set; }

        /// <summary>
        /// Represents the response that will be sent back to the client.
        /// </summary>
        public IMutableWebResponse Response { get; private set; }
    }
}