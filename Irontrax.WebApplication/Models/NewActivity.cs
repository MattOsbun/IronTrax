using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.WebApplication.Models
{
    public class NewActivity
    {
        [Required]
        [MinLength(5)]
        [MaxLength(1024)]
        [Display(Name = "What did you do today?")]
        public string Activity { get; set; }
    }
}
