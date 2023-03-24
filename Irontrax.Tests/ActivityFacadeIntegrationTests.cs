using Irontrax.Services;
using Irontrax.Services.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Irontrax.Tests
{
    public class ActivityFacadeIntegrationTests
    {
        [Fact]
        public async Task FindUserById_WhenCalledWithInvalidId_ShouldReturnNull()
        {
            //Arrange
            IdentityFacade sut = BuildIdentityFacade(BuildHttpClient());

            //Act
            var actualResponse = await sut.FindById("1");

            //Assert
            Assert.Null(actualResponse);
        }

        [Fact]
        public async Task FindUserById_WhenCalledWithValidId_ShouldNotReturnNull()
        {
            //Arrange
            IdentityFacade sut = BuildIdentityFacade(BuildHttpClient());

            //Act
            var actualResponse = await sut.FindById("3bfad6f104d045d99e5ed1b00c9b98d1");

            //Assert
            Assert.NotNull(actualResponse);
        }

        [Fact]
        public async Task GetActivityById_WhenValidIdPassed_ShouldReturnActivity()
        {
            //Arrange
            ActivitiesFacade sut = BuildActivitiesFacade(BuildHttpClient());

            //Act
            var actualActivity = await sut.GetActivityById("29a446b1a6464c8395ab0484ec88d71a");

            //Assert
            Assert.NotNull(actualActivity);
            Assert.Equal("29a446b1a6464c8395ab0484ec88d71a", actualActivity.id);
            Assert.Equal("Tons of pushups", actualActivity.Description);
            Assert.Equal("Test User", actualActivity.UserId);
            Assert.Equal("General", actualActivity.Channel);
        }

        [Fact]
        public async Task GetActivities_WhenCalled_ShouldReturnSuccessfulGetResponse()
        {
            //Arrange
            ActivitiesFacade sut = BuildActivitiesFacade(BuildHttpClient());

            //Act
            _ = await sut.GetActivities();

            //Act

            // Lack of exception is a successful test. Actual content is irrelevant
        }
        private static HttpClient BuildHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));
            return httpClient;
        }

        private static IdentityFacade BuildIdentityFacade(HttpClient httpClient)
        {
            return new IdentityFacade(httpClient, @"https://irontrax-api.azurewebsites.net/api/user");
        }

        private static ActivitiesFacade BuildActivitiesFacade(HttpClient httpClient)
        {
            return new ActivitiesFacade(httpClient, @"https://irontrax-api.azurewebsites.net/api/activity");
        }
    }
}
