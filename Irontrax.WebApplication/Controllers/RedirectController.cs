using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Irontrax.WebApplication.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult RedirectToLocal(string localUrl)
        {
            if (Url.IsLocalUrl(localUrl))
            {
                return Redirect(localUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}