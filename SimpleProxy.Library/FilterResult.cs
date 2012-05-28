using System.IO;
using System.Net;
using System.Text;

namespace SimpleProxy.Library
{
    /// <summary>
    /// Defines the actions to be taken based on the results of a web filter
    /// </summary>
    public sealed class FilterResult
    {
        private FilterResult()
        {
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="FilterResult"/>
        /// </summary>
        /// <param name="action">Defines which action to take</param>
        /// <param name="redirectUrl">Url to be redirected to</param>
        /// <param name="customResultStatus">Custom status code to be returned</param>
        /// <param name="customResultStream">Custom stream to be returned to the client</param>
        public FilterResult(FilterResultAction action, string redirectUrl,
            HttpStatusCode customResultStatus, Stream customResultStream)
        {
            Action = action;
            RedirectUrl = redirectUrl;
            CustomResultStatus = customResultStatus;
            CustomResultStream = customResultStream;
        }

        /// <summary>
        /// Defines which action to take.
        /// </summary>
        public FilterResultAction Action { get; private set; }

        /// <summary>
        /// Url to be redirected to. 
        /// </summary>
        /// <remarks>
        /// This will only be used if <see cref="FilterResultAction.Redirect"/> is used in the <see cref="Action"/> property.
        /// </remarks>
        public string RedirectUrl { get; private set; }

        /// <summary>
        /// Custom status code to be returned.
        /// </summary>
        /// <remarks>
        /// This will only be used if <see cref="FilterResultAction.CustomResult"/> is used in the <see cref="Action"/> property.
        /// </remarks>
        public HttpStatusCode CustomResultStatus { get; private set; }
        
        /// <summary>
        /// Custom stream to be returned to the client.
        /// </summary>
        /// <remarks>
        /// This will only be used if <see cref="FilterResultAction.CustomResult"/> is used in the <see cref="Action"/> property.
        /// </remarks>
        public Stream CustomResultStream { get; private set; }

        /// <summary>
        /// Simple allow filter.
        /// </summary>
        public static FilterResult Allow
        {
            get { return new FilterResult {Action = FilterResultAction.Allow}; }
        }

        /// <summary>
        /// Simple block filter with a custom result stream.
        /// </summary>
        /// <param name="blockTextStream">Custom stream that will be returned to the client.</param>
        /// <param name="status">Custom status code to be returned</param>
        public static FilterResult Block(Stream blockTextStream, HttpStatusCode status)
        {
            return new FilterResult
            {
                Action = FilterResultAction.Block | FilterResultAction.CustomResult,
                CustomResultStream = blockTextStream,
                CustomResultStatus = status
            };
        }

        /// <summary>
        /// Simple block filter with a custom result stream.
        /// </summary>
        /// <param name="blockText">Custom text that will be returned to the client.</param>
        /// <param name="status">Custom status code to be returned</param>
        public static FilterResult Block(string blockText, HttpStatusCode status)
        {
            var data = Encoding.Default.GetBytes(blockText);
            var stream = new MemoryStream(data);

            return Block(stream, status);
        }

        /// <summary>
        /// Simple block and redirect filter.
        /// </summary>
        /// <param name="url">Url the client will be redirected to.</param>
        public static FilterResult Redirect(string url)
        {
            return new FilterResult
            {
                Action = FilterResultAction.Block | FilterResultAction.Redirect, 
                RedirectUrl = url
            };
        }
    }
}