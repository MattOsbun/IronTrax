using Irontrax.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services.Identity
{
    public class IrontraxUserclaimsPrincipalFactory : UserClaimsPrincipalFactory<IrontraxUser>
    {
        public IrontraxUserclaimsPrincipalFactory(UserManager<IrontraxUser> userManager, IOptions<IdentityOptions> optionsAccessor) : 
            base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IrontraxUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(ClaimTypes.Role, "General"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Walking"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Lifting"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Running"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Aerobics"));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.EmailAddress));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));

            return identity;
        }
    }
}
