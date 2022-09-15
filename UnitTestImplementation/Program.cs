using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using UnitTestImplementation.Code;
//using UnitTestImplementation.CodeNotAdapatableToUnitTest;


namespace UnitTestImplementation
{

    class Program
    {
        static IHost _host;
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureServices((context, services) => {
                    services.AddSingleton<IHttpRequestHandler, HttpRequestHandler>();
                    services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
                    services.AddHttpClient();
                });
            return host;
        }

        static void BuildServices(string[] args)
        {
            _host = CreateHostBuilder(args).Build();
        }
        static void Main(string[] args)
        {
            BuildServices(args);

            var scrapper = new WebScrapper(_host.Services.GetService<IHttpRequestHandler>(), _host.Services.GetService<IHttpClientFactory>());
            //var scrapper = new CodeNotAdapatableToUnitTest.WebScrapper( _host.Services.GetService<IHttpClientFactory>());
            Console.WriteLine(scrapper.Scrape());
        }
    }
}
