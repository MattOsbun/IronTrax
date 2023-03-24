using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Api.Activity.Models
{
    public class UserTableEntity: TableEntity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string NormalizedUserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }

    public class UserForCreate
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string NormalizedUserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

    }


}
