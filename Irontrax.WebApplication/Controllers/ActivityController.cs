using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irontrax.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Irontrax.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Irontrax.Models;
using Irontrax.WebApplication.Wrappers;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Irontrax.WebApplication.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityManageable _activityManager;

        public ActivityController(IActivityManageable activityManager)
        {
            _activityManager = activityManager;
        }

        public async Task<ViewResult> Index()
        {
            var activities = await _activityManager.GetActivities();
            ActivitiesListViewModel model = new ActivitiesListViewModel
            {
                Activities = activities.Select(activity => new ActivityViewModel(activity)),
                CurrentUserName = "Matt"
            };
            return View(model);
        }

        public IActionResult LogAjax()
        {
            return PartialView("_logActivityModalPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogAjax(NewActivity model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await LogActivity(model);
                    return Json(JsonConvert.SerializeObject(new { success = true }));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Site Exception", ex.Message);
                }
            }

            return PartialView("_logActivityModalPartial", model);

        }

        private async Task<Activity> LogActivity(NewActivity activity)
        {
            return await _activityManager.CreateActivity(activity.Activity, HttpContext.User.Identity.Name);
        }
    }
}
