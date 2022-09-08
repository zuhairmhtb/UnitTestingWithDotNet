using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestImplementation.Code
{
    [Subject(typeof(WebScrapper))]
    public class When_scrapping_web
    {
        It should_return_three = () =>
        {
            Result.ShouldEqual(3);
        };

        Establish context = () =>
        {
            Subject = new WebScrapper();
        };

        Because of = () =>
        {
            Result = Subject.Scrape();
        };
        static int Result;
        static WebScrapper Subject;
    }
}
