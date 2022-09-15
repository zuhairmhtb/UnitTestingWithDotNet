using System;
using System.Net.Http;

namespace UnitTestImplementation.Code
{
    public class WebScraper
    {
        readonly IHttpRequestHandler _requestHandler;
        readonly IHttpClientFactory _httpClientFactory;
        public const string ScrapingUrl = "https://en.wikipedia.org/wiki/Unit_testing";
        public WebScraper(IHttpRequestHandler requestHandler, IHttpClientFactory httpClientFactory)
        {
            _requestHandler = requestHandler;
            _httpClientFactory = httpClientFactory;
        }
        public string Scrape()
        {
            string result = null;
            try
            {
                using (var client = _requestHandler.CreateClient(_httpClientFactory))
                {
                    var response = client.Get(ScrapingUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Fetched web content:");
                        Console.WriteLine(response.Content);
                        result = response.Content;

                    }
                    else
                    {
                        var message = "Client returned an error";
                        if (response != null) message = $"Client returned a status code of {response.StatusCode}";
                        Console.WriteLine($"Could not retrieve article of {ScrapingUrl}. {message}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Received an exception: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }            
            return result;
        }
    }
}
