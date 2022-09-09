using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UnitTestImplementation.Code
{
    public interface IHttpRequestHandler {
        IHttpClientWrapper CreateClient(IHttpClientFactory clientFactory);
    }

    public class HttpRequestHandler : IHttpRequestHandler
    {
        public HttpRequestHandler()
        {
            
        }

        public IHttpClientWrapper CreateClient(IHttpClientFactory clientFactory)
        {
            return new HttpClientWrapper(clientFactory);
        }

    }
}
