namespace SimpleProxy.Library.Plugins
{
    /// <summary>
    /// Provides the ability to filter a web request based on response data.
    /// </summary>
    public interface IWebResponseFilter
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
        /// Indicates whether the filter requires the response body.
        /// </summary>
        /// <remarks>
        /// When <see langword="true"/>, the <see cref="Filter(IWebResponseData)"/> method will be called,
        /// when <see langword="false"/>, the <see cref="Filter(IWebResponseData, string)"/> method will be called
        /// </remarks>
        bool RequiresResponseBody { get; }

        /// <summary>
        /// Filter the request with no response body.
        /// </summary>
        /// <param name="response">The request data</param>
        FilterResult Filter(IWebResponseData response);

        /// <summary>
        /// Filter the request with no response body.
        /// </summary>
        /// <param name="response">The response data.</param>
        /// <param name="responseBody">The response body.</param>
        FilterResult Filter(IWebResponseData response, string responseBody);
    }
}
