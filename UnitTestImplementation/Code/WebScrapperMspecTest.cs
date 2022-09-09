using Machine.Fakes;
using Machine.Specifications;
using System.Net.Http;

namespace UnitTestImplementation.Code
{
    [Subject(typeof(WebScrapper))]
    public class When_scrapping_web : WithFakes
    {
        It should_return_three = () =>
        {
            Result.ShouldEqual(3);
        };

        Establish context = () =>
        {
            Subject = new WebScrapper(The<IHttpRequestHandler>(), The<IHttpClientFactory>());
        };

        Because of = () =>
        {
            Result = Subject.Scrape();
        };
        static int Result;
        static WebScrapper Subject;
    }
}
