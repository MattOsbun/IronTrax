using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.WebApplication.Models
{
    public class ActivitiesListViewModel
    {
        public string CurrentUserName { get; set; }
        public IEnumerable<ActivityViewModel> Activities { get; set; }
    }
}
