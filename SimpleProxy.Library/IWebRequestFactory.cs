
using System.Net;

namespace SimpleProxy.Library
{
    public interface IWebRequestFactory
    {
        IMutableWebRequest CreateRequest(string url);
    }

    public sealed class WebRequestFactory : IWebRequestFactory
    {
        public IMutableWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            return new MutableWebRequestAdapter(request);
        }
    }
}
