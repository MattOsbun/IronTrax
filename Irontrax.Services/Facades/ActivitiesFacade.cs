using Irontrax.Services.Interfaces;
using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Irontrax.Services.Facades
{
    public class ActivitiesFacade: IActivityManageable
    {
        private HttpClient _activityApiHttpClient;
        private string _baseAddress;
        public ActivitiesFacade(HttpClient activityApiHttpClient, string baseAddress)
        {
            _activityApiHttpClient = activityApiHttpClient;
            _baseAddress = baseAddress;
        }

        public async Task<IEnumerable<Activity>> GetActivities()
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "Daxy8wrKUfOg6vU1KEyyKHa6DseljaYve1azaKMkzJrSfULGPUwDog==");
            return await _activityApiHttpClient.ApiGet<IEnumerable<Activity>>(@$"{_baseAddress}");
        }

        public async Task<IEnumerable<Activity>> GetActivitiesForUser(string userId)
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "EzXkFbdn4i6vnah5vDoG7SmDeGKaNCA6yJ2EjWfYPyELKZHRBMgZEA==");
            return await _activityApiHttpClient.ApiGet<IEnumerable<Activity>>(@$"{_baseAddress}/user/{userId}");
        }

        public async Task<Activity> GetActivityById(string activityId)
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "AeBizIb2dQIxeBBBqtF125WcK0MGeV99UwH8aW9DcYUPKbauXY4szQ==");
            return await _activityApiHttpClient.ApiGet<Activity>(@$"{_baseAddress}/{activityId}");
        }

        public async Task<Activity> GetActivitiesByChannel(string channelName)
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "AzsrdHdeVbzuXgZv6gbITo1RDCblGbKfiZX0zcQBmVdXLQTHo4mH/Q==");
            return await _activityApiHttpClient.ApiGet<Activity>(@$"{_baseAddress}/channel/{channelName}");
        }

        public async Task<Activity> CreateActivity(string description, string userId)
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "56aizgPaW49vDGJbvDFQ24/x8oV0R02OL3wftKclh8RjWa/hMTaAEw==");
            return await _activityApiHttpClient.ApiPost<ActivityForCreate,Activity>(
                @$"{_baseAddress}",
                new ActivityForCreate { 
                    Description = description, 
                    UserId = userId }
            );
        }

        public async Task<Activity> UpdateActivity(string description, string activityId)
        {
            _activityApiHttpClient.CleanAdd("x-functions-key", "AXm8nlpdmlyAdXiGOx3hENV/ORDASXQjne7cz63A2LAqYouiohrmBA==");
            return await _activityApiHttpClient.ApiPut<ActivityForUpdate, Activity>(
                @$"{_baseAddress}/{activityId}",
                new ActivityForUpdate { Description = description }
            );

        }

        private class ActivityForCreate
        {
            public string Description { get; set; }
            public string UserId { get; set; }
        }

        private class ActivityForUpdate
        {
            public string Description { get; set; }
        }
    }
}
