using Irontrax.Api.Activity.Models;
using Irontrax.Models;
using Irontrax.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services.Facades
{
    public class IdentityFacade : IUserManageable
    {
        private HttpClient _userApiHttpClient;
        private string _baseAddress;

        public IdentityFacade(HttpClient activityApiHttpClient, string baseAddress)
        {
            _userApiHttpClient = activityApiHttpClient;
            _baseAddress = baseAddress;
        }

        public async Task<IrontraxUser> CreateUser(IrontraxUser user)
        {
            _userApiHttpClient.CleanAdd("x-functions-key", "pQ8tNFRkNX8bAlbaqT4OxzGeAU7L9r7gH0mkVJKr5ZNwCBwjUHQwCg==");
            return await _userApiHttpClient.ApiPost<IrontraxUser, IrontraxUser>(
                @$"{_baseAddress}",
                user
            );
        }

        public async Task<IrontraxUser> FindById(string id)
        {
            _userApiHttpClient.CleanAdd("x-functions-key", "l5DViy1XrGBozlX2ZS5vUkIcbmr57cQ5XQGGkrlwgjlecQEnKuhaWg==");
            IrontraxUser user = await _userApiHttpClient.ApiGet<IrontraxUser>(@$"{_baseAddress}/{id}");

            if(user == null) { return null; }

            return user;
        }

        public async Task<IrontraxUser> FindByUsername(string userName)
        {
            _userApiHttpClient.CleanAdd("x-functions-key", "Cklot1KuNWvRXHTh3w4MrvYf3BP3gjs4tsQSw8jWgUJWW3mgv031/A==");
            IrontraxUser user = await _userApiHttpClient.ApiGet<IrontraxUser>(@$"{_baseAddress}/findbyname/{userName}");

            if (user == null) { return null; }

            return user;
        }

        public Task<IrontraxUser> UpdateUser(IrontraxUser user)
        {
            throw new NotImplementedException();
        }
    }
}
