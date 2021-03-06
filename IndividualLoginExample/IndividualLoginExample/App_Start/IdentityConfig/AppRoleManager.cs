﻿using IndividualLoginExample.BizObjects;
using IndividualLoginExample.BizObjects.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace IndividualLoginExample.App_Start.IdentityConfig
{
    public class AppRoleManager : RoleManager<Role, int>
    {
        #region Constructors
        public AppRoleManager(IRoleStore<Role, int> store) : base(store)
        {
        }
        #endregion

        #region Factory Methods
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            var manager = new AppRoleManager(new MyDBRoleStore());
            manager.RoleValidator = new RoleValidator<Role, int>(manager);
            return manager;
        }
        #endregion
    }
}