using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnitTestImplementation.Domain;

namespace UnitTestImplementation.CodeNotAdapatableToUnitTest
{
    public class WebScrapper
    {
        readonly IHttpClientFactory _httpClientFactory;
        public const string ScrappingUrl = "https://en.wikipedia.org/wiki/Unit_testing";
        public WebScrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        HttpResponse get(string url, HttpClient client)
        {
            var task = Task.Run(() => {
                try
                {
                    return client.GetAsync(url);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Http Get request error for url {url}.");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
            });
            task.Wait();
            var response = task.Result;
            if (response != null && response.IsSuccessStatusCode)
            {
                var responseGenerator = Task.Run(() => {
                    try
                    {
                        var content = response.Content.ReadAsStringAsync();
                        return content;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error creating response for the request sent to {url}");
                    }
                    return null;
                });
                responseGenerator.Wait();
                var contentString = responseGenerator.Result;
                return new HttpResponse()
                {
                    StatusCode = response.StatusCode,
                    Content = contentString
                };
            }
            else
            {
                if (response == null) Console.Write($"No response returned for url {url}");
                else if (!response.IsSuccessStatusCode) Console.WriteLine($"Unsuccessful request sent to {url}. Status code: {((int)response.StatusCode)}");
            }
            return null;
        }

        public string Scrape()
        {
            string result = null;
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = get(ScrappingUrl, client);
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
                        Console.WriteLine($"Could not retrieve article of {ScrappingUrl}. {message}");
                    }
                }

            } catch(Exception e)
            {
                Console.WriteLine($"Received an exception: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
            return result;
        }
    }
}
