using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Irontrax.WebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Irontrax.WebApplication.Wrappers;
using Newtonsoft.Json;
using Irontrax.Models;

namespace Irontrax.WebApplication.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly UserManager<IrontraxUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<IrontraxUser> _claimsPrincipalFactory;
        private readonly SignInManager<IrontraxUser> _signinManager;

        public AuthenticateController(UserManager<IrontraxUser> userManager,
            IUserClaimsPrincipalFactory<IrontraxUser> claimsPrincipalFactory,
            SignInManager<IrontraxUser> signinManager)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _signinManager = signinManager;
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login(string ReturnUrl)
        {
            return View();
        }

        public IActionResult LoginAjax()
        {
            return PartialView("_loginModalPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (await SignIn(model))
                {
                    return RedirectToAction("Index", "Activity");
                }

                ModelState.AddModelError("","Invalid login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAjax(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await SignIn(model))
                    {
                        return Json(JsonConvert.SerializeObject(new { success = true }));
                    }

                    ModelState.AddModelError("", "Invalid login");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Site Exception", ex.Message);
                }
            }

            return PartialView("_loginModalPartial", model);
        }

        private async Task<bool> SignIn(LoginModel model)
        {
            IrontraxUser user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ClaimsPrincipal principal = await _claimsPrincipalFactory.CreateAsync(user);

                await HttpContext.SignInAsync("Identity.Application", principal);

                return true;
            }

            return false;
        }
    }
}