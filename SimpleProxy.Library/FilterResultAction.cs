using System;

namespace SimpleProxy.Library
{
    /// <summary>
    /// The action that a <see cref="FilterResult"/> can define.
    /// </summary>
    [Flags]
    public enum FilterResultAction
    {
        /// <summary>
        /// Allow the request.
        /// </summary>
        Allow = 0,
        /// <summary>
        /// Block the request.
        /// </summary>
        Block = 1,
        /// <summary>
        /// Redirect the request.
        /// </summary>
        Redirect = 2,
        /// <summary>
        /// Return a custom result to the client.
        /// </summary>
        CustomResult = 4,
    }
}