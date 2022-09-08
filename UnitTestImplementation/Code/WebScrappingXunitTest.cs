using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestImplementation.Code
{
    public class WebScrappingXunitTest
    {
        WebScrapper repository;
        public WebScrappingXunitTest()
        {
            repository = new WebScrapper();
        }
        [Fact]
        public void Scrape_ShouldReturnThree()
        {
            ///Act
            var result = repository.Scrape();

            ///Assert
            Assert.Equal(3, result);
        } 
    }
}
