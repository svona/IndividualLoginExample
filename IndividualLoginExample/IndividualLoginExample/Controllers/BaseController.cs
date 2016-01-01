﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndividualLoginExample.App_Start.IdentityConfig;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace IndividualLoginExample.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        #region Private Variables
        private AppSignInManager signInManager;
        private AppUserManager userManager;
        private MyDBContext db;
        #endregion

        #region Constructors
        public BaseController()
        {
            this.db = new MyDBContext();
        }

        public BaseController(AppSignInManager signInManager, AppUserManager userManager)
            : base()
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        #endregion

        #region Public Properties
        public AppSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }
        #endregion

        #region Method Overrrides
        protected override void Dispose(bool disposing)
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}