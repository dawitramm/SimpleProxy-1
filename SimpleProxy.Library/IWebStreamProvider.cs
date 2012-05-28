using System.IO;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Provides an HTTP stream.
    /// </summary>
    public interface IWebStreamProvider
    {
        /// <summary>
        /// Gets the stream containg the HTTP entity body.
        /// </summary>
        Stream GetStream();
    }
}