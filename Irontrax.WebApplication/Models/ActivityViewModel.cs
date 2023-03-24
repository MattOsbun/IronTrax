using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.WebApplication.Models
{
    public class ActivityViewModel
    {
        public string id { get; set; }
        private DateTime TimeLogged { get; set; }
        public string TimeLoggedDisplay => $"{TimeLogged.ToShortDateString()} {TimeLogged.ToShortTimeString()}";
        public string Description { get; set; }
        public string UserId { get; set; }

        public ActivityViewModel()
        {

        }

        public ActivityViewModel(Activity activity)
        {
            id = activity.id;
            TimeLogged = activity.TimeLogged;
            Description = activity.Description;
            UserId = activity.UserId;
        }
    }
}
