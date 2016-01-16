using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndividualLoginExample.Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Controllers
{
    public class ManageController : BaseController
    {
        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : String.Empty;

            var userId = User.Identity.GetUserId<int>();

            var model = new ManageIndexViewModel
            {
                HasPassword = HasPassword(),
                //PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                //TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                //Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId.ToString())
            };

            return View(model);
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }
    }
}