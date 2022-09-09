using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestImplementation.Code
{
    public interface IHttpClientWrapper : IDisposable
    {
        HttpResponse Get(string url);
    }
    public class HttpClientWrapper : IHttpClientWrapper
    {
        readonly HttpClient _client;
        public HttpClientWrapper(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public HttpResponse Get(string url)
        {
            var task = Task.Run(() => {
                try
                {
                    return _client.GetAsync(url);
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
                    } catch(Exception e)
                    {
                        Console.WriteLine($"Error creating response for the request sent to {url}");
                    }
                    return null;
                });
                responseGenerator.Wait();
                var contentString = responseGenerator.Result;
                return new HttpResponse() { 
                    StatusCode = response.StatusCode,
                    Content = contentString
                };
            } else
            {
                if (response == null) Console.Write($"No response returned for url {url}");
                else if (!response.IsSuccessStatusCode) Console.WriteLine($"Unsuccessful request sent to {url}. Status code: {((int)response.StatusCode)}");
            }
            return null;
        }

        public void Dispose()
        {
            if(_client != null) _client.Dispose();
        }
    }
}
