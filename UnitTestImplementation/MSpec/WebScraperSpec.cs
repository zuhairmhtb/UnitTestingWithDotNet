using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Net.Http;
using UnitTestImplementation.Code;
using UnitTestImplementation.Domain;

namespace UnitTestImplementation.MSpec
{
    internal class ScrapingBaseContext : WithFakes
    {
        OnEstablish context = ctx =>
        {
            The<IHttpRequestHandler>().WhenToldTo(x => x.CreateClient(Param<IHttpClientFactory>.IsAnything))
                .Return(The<IHttpClientWrapper>());
        };
    }
    [Subject(typeof(WebScraper))]
    public class When_scraping_web : WithFakes
    {
        It should_return_the_http_response = () =>
        {
            Result.ShouldEqual(Response.Content);
        };

        It should_create_a_http_client_with_http_request_handler = () =>
        {
            The<IHttpRequestHandler>().WasToldTo(x => x.CreateClient(The<IHttpClientFactory>())).OnlyOnce();
        };

        It should_fetch_the_http_response_of_the_url = () =>
        {
            The<IHttpClientWrapper>().WasToldTo(x => x.Get(
                Param<string>.Matches(y => y == WebScraper.ScrapingUrl
                ))).OnlyOnce();
        };

        It should_dispose_the_http_client = () =>
        {
            The<IHttpClientWrapper>().WasToldTo(x => x.Dispose()).OnlyOnce();
        };

        Establish context = () =>
        {
            With<ScrapingBaseContext>();
            The<IHttpClientWrapper>().WhenToldTo(x => x.Get(Param<string>.IsAnything))
            .Return(Response);
        };

        Because of = () =>
        {
            Subject = new WebScraper(The<IHttpRequestHandler>(), The<IHttpClientFactory>());
            Result = Subject.Scrape();
        };
        static WebScraper Subject;
        static string Result;
        static HttpResponse Response = new HttpResponse()
        {
            Content = "<!DOCTYPE html><html><head><title>Sample HTML Content</title></head><body>This is a demo HTML response</body></html>",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    [Subject(typeof(WebScraper))]
    public class When_scraping_web_and_unsuccessful_response_is_returned : WithFakes
    {
        It should_return_empty_result = () =>
        {
            Result.ShouldBeNull();
        };

        It should_create_a_http_client_with_http_request_handler = () =>
        {
            The<IHttpRequestHandler>().WasToldTo(x => x.CreateClient(The<IHttpClientFactory>())).OnlyOnce();
        };

        It should_fetch_the_http_response_of_the_url = () =>
        {
            The<IHttpClientWrapper>().WasToldTo(x => x.Get(Param<string>.Matches(
                y => y == WebScraper.ScrapingUrl
                ))).OnlyOnce();
        };

        It should_dispose_the_http_client = () =>
        {
            The<IHttpClientWrapper>().WasToldTo(x => x.Dispose()).OnlyOnce();
        };

        Establish context = () =>
        {
            With<ScrapingBaseContext>();
            The<IHttpClientWrapper>().WhenToldTo(x => x.Get(Param<string>.IsAnything))
            .Return(Response);
        };

        Because of = () =>
        {
            Subject = new WebScraper(The<IHttpRequestHandler>(), The<IHttpClientFactory>());
            Result = Subject.Scrape();
        };
        static WebScraper Subject;
        static string Result;
        static HttpResponse Response = new HttpResponse()
        {
            Content = "",
            StatusCode = System.Net.HttpStatusCode.NotFound
        };
    }

    [Subject(typeof(WebScraper))]
    public class When_scraping_web_and_client_throws_exception : WithFakes
    {
        It should_return_empty_result = () =>
        {
            Result.ShouldBeNull();
        };

        It should_dispose_the_http_client = () =>
        {
            The<IHttpClientWrapper>().WasToldTo(x => x.Dispose()).OnlyOnce();
        };

        Establish context = () =>
        {
            With<ScrapingBaseContext>();
            The<IHttpClientWrapper>().WhenToldTo(x => x.Get(Param<string>.IsAnything))
            .Throw(new Exception("The client threw an exception"));
        };

        Because of = () =>
        {
            Subject = new WebScraper(The<IHttpRequestHandler>(), The<IHttpClientFactory>());
            Result = Subject.Scrape();
        };
        static WebScraper Subject;
        static string Result;
    }

    [Subject(typeof(WebScraper))]
    public class When_scraping_web_and_http_request_handler_throws_exception : WithFakes
    {
        It should_return_empty_result = () =>
        {
            Result.ShouldBeNull();
        };

        It should_not_fetch_the_http_response_of_the_url = () =>
        {
            The<IHttpClientWrapper>().WasNotToldTo(x => x.Get(Param<string>.IsAnything));
        };

        Establish context = () =>
        {
            With<ScrapingBaseContext>();
            The<IHttpRequestHandler>().WhenToldTo(x => x.CreateClient(Param<IHttpClientFactory>.IsAnything))
            .Throw(new Exception("The request handler threw an exception"));
        };

        Because of = () =>
        {
            Subject = new WebScraper(The<IHttpRequestHandler>(), The<IHttpClientFactory>());
            Result = Subject.Scrape();
        };
        static WebScraper Subject;
        static string Result;
    }
}
