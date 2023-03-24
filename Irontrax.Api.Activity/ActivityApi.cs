using Irontrax.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Irontrax.Api.Activity.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace Irontrax.Api.Activity
{
    public static class ActivityApi
    {
        private const string _tableName = "activities";

        [FunctionName("CreateActivity")]
        public static async Task<IActionResult> CreateActivity(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "activity")] HttpRequest req,
            [Table(_tableName,Connection = "AzureWebJobsStorage")]IAsyncCollector<ActivityTableEntity> activityTable,
            ILogger log)
        {
            log.LogInformation("Creating an activity");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ActivityForCreate activityToCreate = JsonConvert.DeserializeObject<ActivityForCreate>(requestBody);
            var activity = ActivityMapper.ToActivity(activityToCreate);

            await activityTable.AddAsync(ActivityMapper.ToActivityTableEntity(activity));

            return new OkObjectResult(activity);
        }

        [FunctionName("GetActivities")]
        public static async Task<IActionResult> GetActivities(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity")] HttpRequest req,
            [Table(_tableName, Connection = "AzureWebJobsStorage")]CloudTable activityTable,
            ILogger log)
        {
            log.LogInformation("Getting all activities");

            TableQuery<ActivityTableEntity> query = new TableQuery<ActivityTableEntity>();
            IList<ActivityTableEntity> activities = await activityTable.ExecuteQueryAsync(query);
            return new OkObjectResult(activities.Select(entity=>ActivityMapper.ToActivity(entity)));
        }

        [FunctionName("GetActivitiesByUser")]
        public static async Task<IActionResult> GetActivitiesByUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity/user/{userId}")] HttpRequest req,
            [Table(_tableName, Connection = "AzureWebJobsStorage")]CloudTable activityTable,
            ILogger log,
            string userId
        )
        {
            log.LogInformation($"Getting all activities for user {userId}");

            TableQuery<ActivityTableEntity> query = new TableQuery<ActivityTableEntity>()
            .Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ActivityMapper.PartitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId)
                )
            );
            IList<ActivityTableEntity> activities = await activityTable.ExecuteQueryAsync<ActivityTableEntity>(query);
            return new OkObjectResult(activities.Select(entity => ActivityMapper.ToActivity(entity)));
        }

        [FunctionName("GetActivitiesByChannel")]
        public static async Task<IActionResult> GetActivitiesByChannel(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity/channel/{channelName}")] HttpRequest req,
            [Table(_tableName, Connection = "AzureWebJobsStorage")]CloudTable activityTable,
            ILogger log,
            string channelName
        )
        {
            log.LogInformation($"Getting all activities for channel {channelName}");

            TableQuery<ActivityTableEntity> query = new TableQuery<ActivityTableEntity>()
            .Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ActivityMapper.PartitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Channel", QueryComparisons.Equal, channelName)
                )
            );
            IList<ActivityTableEntity> activities = await activityTable.ExecuteQueryAsync<ActivityTableEntity>(query);
            return new OkObjectResult(activities.Select(entity => ActivityMapper.ToActivity(entity)));
        }

        [FunctionName("GetActivityById")]
        public static IActionResult GetActivityById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity/{id}")] HttpRequest req,
            [Table(_tableName, ActivityMapper.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")]ActivityTableEntity activityEntity,
            ILogger log,
            string id
        )
        {
            log.LogInformation($"Getting Activity {id}");

            if (activityEntity == null)
            {
                log.LogInformation($"Item {id} not found");
                return new NotFoundResult();
            }

            return new OkObjectResult(ActivityMapper.ToActivity(activityEntity));
        }

        [FunctionName("UpdateActivity")]
        public static async Task<IActionResult> UpdateActivity(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "activity/{id}")] HttpRequest req,
            [Table(_tableName, Connection = "AzureWebJobsStorage")]CloudTable activityTable,
            ILogger log,
            string id
        )
        {
            log.LogInformation($"Updating Activity {id}");

            TableOperation findOperation = TableOperation.Retrieve<ActivityTableEntity>(ActivityMapper.PartitionKey, id);
            TableResult findResult = await activityTable.ExecuteAsync(findOperation);

            if(findResult.Result == null)
            {
                return new NotFoundResult();
            }

            var activityToUpdate = findResult.Result as ActivityTableEntity;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newActivity = JsonConvert.DeserializeObject<ActivityForUpdate>(requestBody);
            activityToUpdate.Description = newActivity.Description;

            TableOperation replaceOperation = TableOperation.Replace(activityToUpdate);
            await activityTable.ExecuteAsync(replaceOperation);

            return new OkObjectResult(ActivityMapper.ToActivity(activityToUpdate));
        }

        [FunctionName("DeleteActivity")]
        public static async Task<IActionResult> DeleteActivity(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "activity/{id}")] HttpRequest req,
            [Table(_tableName, Connection = "AzureWebJobsStorage")]CloudTable activityTable,
            ILogger log,
            string id
        )
        {
            log.LogInformation($"Deleting Activity {id}");

            TableOperation deleteOperation = TableOperation.Delete(
                new TableEntity
                {
                    PartitionKey = ActivityMapper.PartitionKey,
                    RowKey = id,
                    ETag = "*"
                }
            );

            try
            {
                TableResult deleteREsult = await activityTable.ExecuteAsync(deleteOperation);
                return new OkResult();
            }
            catch(StorageException se) when (se.RequestInformation.HttpStatusCode == 404)
            {
                return new NotFoundResult();
            }
        }
    }
}
