using System;
using UnitTestImplementation.Code;

namespace UnitTestImplementation
{

    class Program
    {
        static void Main(string[] args)
        {
            var scrapper = new WebScrapper();
            var result = scrapper.Scrape();
            Console.WriteLine("Scrapper returned " + result);
        }
    }
}
