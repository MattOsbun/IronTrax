using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services.Interfaces
{
    public interface IActivityManageable
    {
        Task<IEnumerable<Activity>> GetActivitiesForUser(string userId);
        Task<IEnumerable<Activity>> GetActivities();
        Task<Activity> GetActivityById(string activityId);
        Task<Activity> CreateActivity(string description, string userId);
        Task<Activity> UpdateActivity(string description, string activityId);
    }
}
