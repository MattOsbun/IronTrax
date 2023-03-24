using Irontrax.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Irontrax.WebApplication.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

    }
}
