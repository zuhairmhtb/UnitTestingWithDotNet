using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnitTestImplementation.Code;
using Xunit;

namespace UnitTestImplementation.XUnit
{
    public class WebScrapper
    {
        Mock<IHttpRequestHandler> requestHandler = new Mock<IHttpRequestHandler>();
        Mock<IHttpClientFactory> httpClientFactory = new Mock<IHttpClientFactory>();
        Mock<IHttpClientWrapper> httpClient = new Mock<IHttpClientWrapper>();
        Code.WebScrapper repository;

        class InvalidResponseData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new HttpResponse() { Content = "", StatusCode = System.Net.HttpStatusCode.NoContent } };
                yield return new object[] { new HttpResponse() { Content = "", StatusCode = System.Net.HttpStatusCode.NotFound } };
                yield return new object[] { new HttpResponse() { Content = "", StatusCode = System.Net.HttpStatusCode.BadRequest } };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public WebScrapper()
        {
            repository = new Code.WebScrapper(requestHandler.Object, httpClientFactory.Object);
            /// Shared context
            requestHandler.Setup(x => x.CreateClient(It.IsAny<IHttpClientFactory>()))
                .Returns(httpClient.Object);
        }
        [Fact]
        public void WhenScrappingWeb_ShouldReturnValidResponse()
        {
            ///Arrange
            var response = new HttpResponse() {
                Content = "<!DOCTYPE html><html><head><title>Sample HTML Content</title></head><body>This is a demo HTML response</body></html>",
                StatusCode = System.Net.HttpStatusCode.OK
            };
            httpClient.Setup(x => x.Get(It.IsAny<string>())).Returns(response);
            ///Act
            var result = repository.Scrape();

            ///Assert
            Assert.Equal(response.Content, result);
            requestHandler.Verify(x => x.CreateClient(httpClientFactory.Object), Times.Once);
            httpClient.Verify(x => x.Get(It.Is<string>(val => val == Code.WebScrapper.ScrappingUrl)), Times.Once);
            httpClient.Verify(x => x.Dispose(), Times.Once);
            
        }
        
        [Theory]
        [ClassData(typeof(InvalidResponseData))]
        public void WhenScrappingWebAndUnsuccessfulResponseIsReturned_ShouldReturnEmptyResponse(HttpResponse response)
        {
            ///Arrange
            httpClient.Setup(x => x.Get(It.IsAny<string>())).Returns(response);
            
            //Act
            var result = repository.Scrape();

            ///Assert
            Assert.True(result == null);
            requestHandler.Verify(x => x.CreateClient(httpClientFactory.Object), Times.Once);
            httpClient.Verify(x => x.Get(It.Is<string>(val => val == Code.WebScrapper.ScrappingUrl)), Times.Once);
            httpClient.Verify(x => x.Dispose(), Times.Once);
        }

        [Theory]
        [InlineData(typeof(ArgumentException))]
        [InlineData(typeof(HttpRequestException))]
        public void WhenScrappingWebAndExceptionIsReturned_ShouldReturnEmptyResponse(Type exceptionType) {
            ///Arrange
            if(exceptionType == typeof(ArgumentException))
            {
                requestHandler.Setup(x => x.CreateClient(It.IsAny<IHttpClientFactory>()))
                .Throws(new ArgumentException());
            }
            httpClient.Setup(x => x.Get(It.IsAny<string>()))
                .Throws(new HttpRequestException());
            
            ///Act
            var result = repository.Scrape();

            ///Assert
            Assert.True(result == null);
            if(exceptionType == typeof(ArgumentException))
            {
                httpClient.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
            } else
            {
                requestHandler.Verify(x => x.CreateClient(httpClientFactory.Object), Times.Once);
                httpClient.Verify(x => x.Get(It.Is<string>(val => val == Code.WebScrapper.ScrappingUrl)), Times.Once);
                httpClient.Verify(x => x.Dispose(), Times.Once);
            }
        }
    }
}
