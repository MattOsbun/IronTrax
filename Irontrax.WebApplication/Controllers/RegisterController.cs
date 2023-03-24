using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irontrax.Models;
using Irontrax.WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Irontrax.WebApplication.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<IrontraxUser> userManager;

        public RegisterController(UserManager<IrontraxUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modal()
        {
            return PartialView("_registrationModalPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Authenticate");
                }

                ModelState.AddModelError("", result.Errors.First().Description);
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAjax(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityResult result = await RegisterUserAsync(model);

                    if (result.Succeeded)
                    {
                        return Json(JsonConvert.SerializeObject(new { success = true }));
                    }

                    ModelState.AddModelError("", result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Site Exception", ex.Message);
                }
            }

            return PartialView("_registrationModalPartial", model);
        }

        private async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            IrontraxUser user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user = new IrontraxUser
                {
                    UserName = model.UserName,
                    EmailAddress = model.EmailAddress,
                    Name = model.Name
                };

                return await userManager.CreateAsync(user, model.Password);
            }
            else
            {
                IdentityError error = new IdentityError() { Description = "User already exists" };
                return await Task.FromResult(IdentityResult.Failed(new IdentityError[] { error }));
            }
        }
    }
}