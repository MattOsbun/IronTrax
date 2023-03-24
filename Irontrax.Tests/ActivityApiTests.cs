using Irontrax.Api.Activity;
using Irontrax.Api.Activity.Models;
using Irontrax.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Irontrax.Tests
{
    public class ActivityApiTests
    {
        [Fact]
        public async Task CreateActivity_WhenGivenActivity_ShouldReturnCompleteActivity()
        {
            //Arrange
            string testDescription = "Test Activity";
            string testUserId = "Test User";
            var mockCollecter = new Mock<IAsyncCollector<ActivityTableEntity>>();
            var mockHttpRequest = CreateMockRequest(new { Description = testDescription, UserId = testUserId });
            var mockLogger = new Mock<ILogger>();

            // Act
            var actualResponse = await ActivityApi.CreateActivity(
                mockHttpRequest.Object, mockCollecter.Object, 
                mockLogger.Object);

            //Assert
            Assert.IsType<OkObjectResult>(actualResponse);

            var actualActivity = ((OkObjectResult)actualResponse).Value as Activity;
            Assert.NotEmpty(actualActivity.id);
            Assert.NotEqual(DateTime.MinValue, actualActivity.TimeLogged);
            Assert.Equal(testDescription, actualActivity.Description);
            Assert.Equal(testUserId, actualActivity.UserId);
        }

        private static Mock<HttpRequest> CreateMockRequest(object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }

        private static bool AreActivitiesEqual(Activity activity, ActivityTableEntity entity)
        {
            return activity.id == entity.RowKey &&
                activity.TimeLogged == DateTime.Parse(entity.TimeLogged) &&
                activity.Description == entity.Description &&
                activity.UserId == entity.UserId;
        }
    }

}
