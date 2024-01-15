using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Api.Activity.Models
{
    public class Exercise
    {
        public string id { get; } = Guid.NewGuid().ToString("n");
        public DateTime TimeLogged { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Channel { get; set; }
    }
}
