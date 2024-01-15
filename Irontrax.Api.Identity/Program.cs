using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Irontrax.Api.Activity.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Irontrax.Api.Activity
{
    public static class IdentityApi
    {
        private const string _tableName = "irontraxuser";

        public static async Task<IActionResult> CreateUser(
            [Table(_tableName, Connection = "AzureWebJobsStorage")] IAsyncCollector<UserTableEntity> userTable,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserForCreate userToCreate = JsonConvert.DeserializeObject<UserForCreate>(requestBody);
            UserTableEntity userEntity = UserMapper.ToTableEntity(userToCreate);
            await userTable.AddAsync(userEntity);

            return new OkObjectResult(UserMapper.ToUser(userEntity));
        }

        [FunctionName("FindUserById")]
        public static IActionResult FindUserById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
            [Table(_tableName, UserMapper.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")] UserTableEntity userEntity,
            ILogger log,
            string id
        )
        {
            log.LogInformation($"Getting User {id}");

            if (userEntity == null)
            {
                log.LogInformation($"User {id} not found");
                return new NotFoundResult();
            }

            return new OkObjectResult(UserMapper.ToUser(userEntity));
        }

        [FunctionName("FindUserByName")]
        public static async Task<IActionResult> FindUserByName(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/findbyname/{userName}")] HttpRequest req,
            [Table(_tableName, UserMapper.PartitionKey, Connection = "AzureWebJobsStorage")] CloudTable userTable,
            ILogger log,
            string userName
        )
        {
            log.LogInformation($"Getting User {userName}");

            TableQuery<UserTableEntity> query = new TableQuery<UserTableEntity>()
            .Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, UserMapper.PartitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("NormalizedUserName", QueryComparisons.Equal, userName)
                )
            );
            IList<UserTableEntity> users = await userTable.ExecuteQueryAsync<UserTableEntity>(query);

            if (users.Count > 1)
            {
                throw new Exception($"Multiple users with username {userName}");
            }

            if (!users.Any())
            {
                log.LogInformation($"User {userName} not found");
                return new NotFoundResult();
            }

            return new OkObjectResult(UserMapper.ToUser(users[0]));
        }
    }
}
