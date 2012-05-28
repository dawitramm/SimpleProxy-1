namespace SimpleProxy.Library.Plugins
{
    /// <summary>
    /// Provides the ability to filter a web request based on request data.
    /// </summary>
    public interface IWebRequestFilter
    {
        /// <summary>
        /// Name of the filter
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates whether the filter is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Filter the request.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns></returns>
        FilterResult Filter(IWebRequestData request);
    }
}