using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}