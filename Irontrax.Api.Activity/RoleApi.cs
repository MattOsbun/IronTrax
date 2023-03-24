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

namespace Irontrax.Api.Activity
{
    public static class RoleApi
    {
        private const string _tableName = "irontraxapplicationrole";

        [FunctionName("GetRoleById")]
        public static async Task<IActionResult> GetRoleById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "role/{id}")] HttpRequest req,
            [Table(_tableName, RoleMapper.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")]RoleTableEntity roleEntity,
            ILogger log,
            string id)
        {
            log.LogInformation($"Getting Role {id}");

            if (roleEntity == null)
            {
                log.LogInformation($"Role {id} not found");
                return new NotFoundResult();
            }

            return await Task.FromResult(new OkObjectResult(RoleMapper.ToRole(roleEntity)));
        }
    }
}
