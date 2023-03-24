using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.Models
{
    public class Activity
    {
        public string id { get; set; } = Guid.NewGuid().ToString("n");
        public DateTime TimeLogged { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Channel { get; set; }
    }
}
