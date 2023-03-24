using Irontrax.Api.Activity.Models;
using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services.Facades
{
    public class RoleFacade
    {
        private HttpClient _roleApiHttpClient;
        private string _baseAddress;

        public RoleFacade(HttpClient activityApiHttpClient, string baseAddress)
        {
            _roleApiHttpClient = activityApiHttpClient;
            _baseAddress = baseAddress;
        }

        public async Task<ApplicationRole> FindById(string id)
        {
            _roleApiHttpClient.CleanAdd("x-functions-key", "l5DViy1XrGBozlX2ZS5vUkIcbmr57cQ5XQGGkrlwgjlecQEnKuhaWg==");
            ApplicationRole applicationRole = await _roleApiHttpClient.ApiGet<ApplicationRole>(@$"{_baseAddress}/{id}");

            if (applicationRole == null) { return null; }

            return new ApplicationRole
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name,
                NormalizedName = applicationRole.NormalizedName
            };
        }
    }
}
