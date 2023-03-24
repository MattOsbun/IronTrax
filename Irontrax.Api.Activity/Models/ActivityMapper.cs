using System;
using System.Collections.Generic;
using System.Text;
using Irontrax.Models;

namespace Irontrax.Api.Activity.Models
{
    internal static class ActivityMapper
    {
        public const string PartitionKey = "Activity";

        public static Irontrax.Models.Activity ToActivity(ActivityForCreate activity)
        {
            return new Irontrax.Models.Activity
            {
                TimeLogged = DateTime.Now,
                Description = activity.Description,
                UserId = activity.UserId,
                Channel = activity.Channel
            };
    }

        public static Irontrax.Models.Activity ToActivity(ActivityTableEntity entity)
        {
            return new Irontrax.Models.Activity
            {
                id = entity.RowKey,
                TimeLogged = DateTime.Parse(entity.TimeLogged),
                Description = entity.Description,
                UserId = entity.UserId,
                Channel = entity.Channel
            };
        }

        public static ActivityTableEntity ToActivityTableEntity(Irontrax.Models.Activity activity)
        {
            return new ActivityTableEntity
            {
                RowKey = activity.id,
                TimeLogged = activity.TimeLogged.ToString(),
                Description = activity.Description,
                UserId = activity.UserId,
                PartitionKey = PartitionKey,
                Channel = activity.Channel
            };
        }
    }
}
