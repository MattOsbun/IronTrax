using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Models
{
    public class IrontraxUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}
