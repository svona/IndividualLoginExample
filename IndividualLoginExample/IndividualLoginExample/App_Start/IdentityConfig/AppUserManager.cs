using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IndividualLoginExample.Crypto;
using IndividualLoginExample.Helpers;
using IndividualLoginExample.Models;
using IndividualLoginExample.Properties;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace IndividualLoginExample.App_Start.IdentityConfig
{
    public class AppUserManager : UserManager<User, int>
    {
        #region Constructors
        public AppUserManager(IUserStore<User, int> store) : base(store)
        {
        }
        #endregion

        #region Factory Methods
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new MyDBUserStore());
            manager.UserValidator = new UserValidator<User, int>(manager);

            manager.PasswordHasher = new MyCustomPasswordHasher();

            manager.PasswordValidator = new PasswordValidator
            {
                RequireDigit = Settings.Default.PasswordMustContainNumericCharacter,
                RequiredLength = Settings.Default.PasswordMinimumCharacterLength,
                RequireLowercase = Settings.Default.PasswordMustContainLowercaseLetter,
                RequireUppercase = Settings.Default.PasswordMustContainUppercaseLetter,
                RequireNonLetterOrDigit = Settings.Default.PasswordMustContainSpecialCharacter
            };

            manager.EmailService = new MyEmailService();

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(Settings.Default.AccountLockoutDurationInMinutes);
            manager.MaxFailedAccessAttemptsBeforeLockout = Settings.Default.NumberOfLoginAttempts;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                Microsoft.Owin.Security.DataProtection.IDataProtector p = dataProtectionProvider.Create("ASP.NET Identity");
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(p);
            }
            return manager;
        }
        #endregion

        #region Method Overrides
        public override Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            Task<IdentityResult> result = this.CheckOldPasswords(userId, newPassword);
            if (result != null)
            {
                return result;
            }

            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            Task<IdentityResult> result = this.CheckOldPasswords(userId, newPassword);
            if (result != null)
            {
                return result;
            }

            return base.ResetPasswordAsync(userId, token, newPassword);
        }
        #endregion

        private Task<IdentityResult> CheckOldPasswords(int userId, string newPassword)
        {
            var usedPasswordList = ((MyDBUserStore)this.Store).GetPasswords(userId).Take(Settings.Default.DontAllowLastNumberOfPasswords);

            if (usedPasswordList
                .Any(b => this.PasswordHasher.VerifyHashedPassword(b.PasswordHash, newPassword) == PasswordVerificationResult.Success))
            {
                // password was previously used and cannot be used again.
                return Task.FromResult(IdentityResult.Failed(String.Format("You cannot use any of your last {0} passwords.", Settings.Default.DontAllowLastNumberOfPasswords)));
            }

            return null;
        }
    }
}