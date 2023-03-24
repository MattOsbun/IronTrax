using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Api.Activity.Models
{
    internal static class RoleMapper
    {
        public const string PartitionKey = "Role";

        public static ApplicationRole ToRole(RoleTableEntity entity)
        {
            return new ApplicationRole
            {
                Id = entity.RowKey,
                Name = entity.Name,
                NormalizedName = entity.NormalizedName
            };
        }
    }
}
