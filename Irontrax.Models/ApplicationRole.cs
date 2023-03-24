using System;
using System.Collections.Generic;
using System.Text;

namespace Irontrax.Models
{
    public class ApplicationRole
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
