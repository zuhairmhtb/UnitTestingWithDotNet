using System;
using System.Net.Http;

namespace UnitTestImplementation.Code
{
    public class WebScrapper
    {
        IHttpRequestHandler _requestHandler;
        IHttpClientFactory _httpClientFactory;
        public WebScrapper(IHttpRequestHandler requestHandler, IHttpClientFactory httpClientFactory)
        {
            _requestHandler = requestHandler;
            _httpClientFactory = httpClientFactory;
        }
        public int Scrape()
        {
            var url = "https://en.wikipedia.org/wiki/Unit_testing";
            using (var client = _requestHandler.CreateClient(_httpClientFactory))
            {
                var response = client.Get(url);
                if(response != null && response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Fetched web content:");
                    Console.WriteLine(response.Content);

                } else
                {
                    var message = "Client returned an error";
                    if (response != null) message = $"Client returned a status code of {response.StatusCode}";
                    Console.WriteLine($"Could not retrieve article of {url}. {message}");
                }
            }

            var a = 1;
            var b = 2;
            return a + b;
        }
    }
}
