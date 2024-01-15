using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Api.Identity.Models
{
    internal static class UserMapper
    {
        public const string PartitionKey = "User";

        public static IrontraxUser ToUser(UserForCreate user)
        {
            return new IrontraxUser
            {
                UserName = user.UserName,
                Name = user.Name,
                NormalizedUserName = user.NormalizedUserName,
                EmailAddress = user.EmailAddress,
                PasswordHash = user.PasswordHash,
            };
        }
        public static UserTableEntity ToTableEntity(IrontraxUser user)
        {
            return new UserTableEntity
            {
                RowKey = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PartitionKey = PartitionKey,
                EmailAddress = user.EmailAddress
            };
        }
        public static UserTableEntity ToTableEntity(UserForCreate user)
        {
            return new UserTableEntity
            {
                RowKey = Guid.NewGuid().ToString("n"),
                UserName = user.UserName,
                Name = user.Name,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PartitionKey = PartitionKey,
                EmailAddress = user.EmailAddress
            };
        }

        public static IrontraxUser ToUser(UserTableEntity entity)
        {
            return new IrontraxUser
            {
                Id = entity.RowKey,
                UserName = entity.UserName,
                Name = entity.Name,
                NormalizedUserName = entity.NormalizedUserName,
                PasswordHash = entity.PasswordHash,
                EmailAddress = entity.EmailAddress
            };
        }
    }
}
