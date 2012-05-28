using System;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Defines an HTTP Web response.
    /// </summary>
    public interface IWebResponse : IWebResponseData, IWebStreamProvider, IDisposable
    {
    }
}