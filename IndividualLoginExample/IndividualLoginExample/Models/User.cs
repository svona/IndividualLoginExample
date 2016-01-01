﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IndividualLoginExample.App_Start.IdentityConfig;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Models
{
    [Table("Users")]
    public class User : IUser<int>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            CreationDateUTC = DateTime.UtcNow;
        }
        #endregion

        #region IUser Implementation
        [Key]
        public int Id
        {
            get;
            protected set;
        }

        [StringLength(100)]
        public string UserName
        {
            get; set;
        }
        #endregion
        
        public DateTime CreationDateUTC { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}