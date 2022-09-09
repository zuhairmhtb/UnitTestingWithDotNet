using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace UnitTestImplementation.Code
{
    public class WebScrappingXunitTest
    {
        Mock<IHttpRequestHandler> requestHandler = new Mock<IHttpRequestHandler>();
        Mock<IHttpClientFactory> httpClientFactory = new Mock<IHttpClientFactory>();
        WebScrapper repository;
        public WebScrappingXunitTest()
        {
            repository = new WebScrapper(requestHandler.Object, httpClientFactory.Object);
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
