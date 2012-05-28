using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using SimpleProxy.Library.Plugins;

namespace SimpleProxy.Library
{
    public class ProxyRequestProcessor
    {
        private static readonly string[] PropertyHeaders = new[]
        {
            "Content-Encoding",
            "Content-Length",
            "Content-Type",
            "Location",
            "Keep-Alive",
            "Transfer-Encoding"
        };

        private readonly IWebRequestFactory _webRequestFactory;

        public ProxyRequestProcessor(IWebRequestFactory webRequestFactory)
        {
            _webRequestFactory = webRequestFactory;
        }

        public void ProcessRequest(IWebRequest request, 
            IMutableWebResponse response, IPrincipal user, 
            IEnumerable<IWebRequestFilter> requestFilters, 
            IEnumerable<IWebResponseFilter> responseFilters)
        {
            Trace.TraceInformation("Responding to request: {0}", request.RawUrl);
            try
            {
                // Filter request
                var requestFilterResult = FilterRequest(request, requestFilters);
                if (requestFilterResult.Action == (requestFilterResult.Action | FilterResultAction.Block))
                {
                    Trace.TraceInformation("Blocking request.");
                    if (requestFilterResult.Action == (requestFilterResult.Action | FilterResultAction.Redirect))
                    {
                        Redirect(response, requestFilterResult.RedirectUrl);
                        return;
                    }
                    if (requestFilterResult.Action == (requestFilterResult.Action | FilterResultAction.CustomResult))
                    {
                        CustomAction(response, requestFilterResult.CustomResultStatus, requestFilterResult.CustomResultStream);
                        return;
                    }
                    var message = String.Format("Request for {0} was blocked by a filter", request.RawUrl);
                    var data = Encoding.Default.GetBytes(message);
                    var messageStream = new MemoryStream(data);

                    CustomAction(response, HttpStatusCode.OK, messageStream);
                    return;
                }
                Trace.TraceInformation("Proxying request");

                // Build proxy request
                var requestBuilder = new ProxyRequestBuilder(_webRequestFactory, request);
                var proxyRequest = requestBuilder.Build();

                // Get proxy response
                Trace.TraceInformation("Getting proxy response");
                using (var proxyResponse = proxyRequest.GetResponse())
                {
                    var responseFilterResult = FilterResponse(proxyResponse, responseFilters);
                    if (responseFilterResult.Action == (responseFilterResult.Action | FilterResultAction.Block))
                    {
                        Trace.TraceInformation("Blocking request.");
                        if (responseFilterResult.Action == (responseFilterResult.Action | FilterResultAction.Redirect))
                        {
                            Redirect(response, responseFilterResult.RedirectUrl);
                            return;
                        }
                        if (responseFilterResult.Action == (responseFilterResult.Action | FilterResultAction.CustomResult))
                        {
                            CustomAction(response, responseFilterResult.CustomResultStatus, responseFilterResult.CustomResultStream);
                            return;
                        }
                        var message = String.Format("Request for {0} was blocked by a filter", request.RawUrl);
                        var data = Encoding.Default.GetBytes(message);
                        var messageStream = new MemoryStream(data);

                        CustomAction(response, HttpStatusCode.OK, messageStream);
                        return;
                    }
                    Trace.TraceInformation("Forwarding proxy response");
                    BuildResponse(response, proxyResponse);
                }
            }
            catch (HttpListenerException hEx)
            {
                if (hEx.ErrorCode == 64)
                {
                    Trace.TraceWarning("Stream aborted");
                }
                else
                {
                    Trace.TraceError("Error processing request: {0}", hEx);

                    var data = Encoding.Default.GetBytes("SimpleProxy: Error Processing Request.");
                    var messageStream = new MemoryStream(data);

                    CustomAction(response, HttpStatusCode.InternalServerError, messageStream);
                }
            }
            catch (ProxyResponseException wEx)
            {
                if (wEx.Response != null)
                {
                    BuildResponse(response, wEx.Response);
                }
                if (wEx.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    Trace.TraceWarning("Name Resolution Failure, redirecting to search: {0}", request.RawUrl);
                    Redirect(response, String.Format("http://www.google.com/search?q={0}", HttpUtility.UrlEncode(request.RawUrl)));
                }
                else
                {
                    Trace.TraceError("Error processing request: {0}", wEx);

                    var data = Encoding.Default.GetBytes(String.Format("SimpleProxy: Error Processing Request: {0}", wEx.Message));
                    var messageStream = new MemoryStream(data);

                    CustomAction(response, HttpStatusCode.InternalServerError, messageStream);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error processing request: {0}", ex);

                var data = Encoding.Default.GetBytes("SimpleProxy: Error Processing Request.");
                var messageStream = new MemoryStream(data);

                CustomAction(response, HttpStatusCode.InternalServerError, messageStream);
            }
        }

        private static FilterResult FilterRequest(IWebRequestData request, IEnumerable<IWebRequestFilter> requestFilters)
        {
            foreach (var filter in requestFilters)
            {
                var filterName = "Filter";
                try
                {
                    if (!filter.Enabled)
                        continue;

                    filterName = filter.Name;
                    if (String.IsNullOrWhiteSpace(filterName))
                        filterName = filter.GetType().Name;

                    Trace.TraceInformation("Applying request request filter {0}", filterName);

                    var filterResult = filter.Filter(request);

                    if (filterResult.Action == (filterResult.Action | FilterResultAction.Block))
                        return filterResult;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("{0} filter failed: {1}", filterName, ex);
                    var errorMessage = String.Format("SimpleProxy: Error Processing Request: {0} failed", filterName);
                    return FilterResult.Block(errorMessage, HttpStatusCode.InternalServerError);
                }
            }
            return FilterResult.Allow;
        }

        private static FilterResult FilterResponse(IWebResponseData proxyResponse, IEnumerable<IWebResponseFilter> responseFilters)
        {
            foreach (var filter in responseFilters)
            {
                var filterName = "Filter";
                try
                {
                    if (!filter.Enabled)
                        continue;

                    filterName = filter.Name;
                    if (String.IsNullOrWhiteSpace(filterName))
                        filterName = filter.GetType().Name;

                    Trace.TraceInformation("Applying response filter {0}", filterName);

                    FilterResult filterResult;

                    if (filter.RequiresResponseBody)
                        throw new NotImplementedException();
                    else
                        filterResult = filter.Filter(proxyResponse);

                    if (filterResult.Action == (filterResult.Action | FilterResultAction.Block))
                        return filterResult;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("{0} filter failed: {1}", filterName, ex);
                    var errorMessage = String.Format("SimpleProxy: Error Processing Request: {0} failed", filterName);
                    return FilterResult.Block(errorMessage, HttpStatusCode.InternalServerError);
                }
            }
            return FilterResult.Allow;
        }

        private void BuildResponse(IMutableWebResponse response, IWebResponse proxyResponse)
        {
            response.ProtocolVersion = proxyResponse.ProtocolVersion;
            response.ContentType = proxyResponse.ContentType;
            response.StatusCode = (int)proxyResponse.StatusCode;
            response.StatusDescription = proxyResponse.StatusDescription;

            if (proxyResponse.ContentLength >= 0)
                response.ContentLength = proxyResponse.ContentLength;
            if (proxyResponse.Headers.ContainsKey("Content-Encoding") && !String.IsNullOrWhiteSpace(proxyResponse.ContentEncoding))
                response.ContentEncoding = Encoding.GetEncoding(proxyResponse.ContentEncoding);
            if (proxyResponse.Headers.ContainsKey("Location"))
                response.RedirectLocation = proxyResponse.Headers["Location"];
            if (proxyResponse.Headers.ContainsKey("Keep-Alive"))
            {
                bool keepAlive;
                if (Boolean.TryParse(proxyResponse.Headers["Keep-Alive"], out keepAlive))
                    response.KeepAlive = keepAlive;
            }

            foreach (var cookie in proxyResponse.Cookies)
            {
                response.Cookies.Add(cookie);
            }
            foreach (var header in proxyResponse.Headers)
            {
                if (header.Key.StartsWith("Proxy-"))
                    continue; // Ignore proxy headers
                if (PropertyHeaders.Contains(header.Key))
                    continue;

                response.Headers.Add(header.Key, header.Value);
            }

            var proxyStream = proxyResponse.GetStream();
            var requestStream = response.GetStream();

            proxyStream.CopyTo(requestStream);
            requestStream.Close();
        }

        private static void CustomAction(IMutableWebResponse response, HttpStatusCode customResultStatus, Stream customResultStream)
        {
            if (customResultStatus == 0)
                customResultStatus = HttpStatusCode.OK;

            response.StatusCode = (int)customResultStatus;

            var responseStream = response.GetStream();
            customResultStream.CopyTo(responseStream);
            responseStream.Close();
        }

        private static void Redirect(IMutableWebResponse response, string redirectUri)
        {
            Trace.TraceInformation("Redirecting to {0}", redirectUri);

            response.StatusCode = 302;
            response.RedirectLocation = redirectUri;
        }
    }
}