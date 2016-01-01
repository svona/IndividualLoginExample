using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndividualLoginExample.Models;
using IndividualLoginExample.Properties;

namespace IndividualLoginExample.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var status = await this.SignInManager.PasswordSignInAsync(
                model.UserName, 
                model.Password, 
                model.RememberMe, 
                Settings.Default.AccountLockoutDurationInMinutes > 0);

            ActionResult result = View();

            switch (status)
            {
                case Microsoft.AspNet.Identity.Owin.SignInStatus.Success:
                    result = RedirectToLocal(returnUrl);
                    break;
                case Microsoft.AspNet.Identity.Owin.SignInStatus.LockedOut:
                    result = View("Lockout");
                    break;
                case Microsoft.AspNet.Identity.Owin.SignInStatus.RequiresVerification:
                    result = RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    break;
                case Microsoft.AspNet.Identity.Owin.SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    result = View(model);
                    break;
            }

            return result;
        }
    }
}