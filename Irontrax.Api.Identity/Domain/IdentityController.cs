using Irontrax.Api.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Irontrax.Api.Identity.Controllers
{
    public class Identity
    {

        private readonly ILogger<Identity> _logger;

        public Identity(ILogger<Identity> logger)
        {
            _logger = logger;
        }

        public IrontraxUser? Validate(string userName, string passwordHash)
        {
            return new IrontraxUser {
                UserName = userName,
                NormalizedUserName = "Matt Osbun",
                Name = "Matt Osbun",
                EmailAddress = "noone@nowhere.com"
            };
        }
    }
}
