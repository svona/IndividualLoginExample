using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.BizObjects.Models
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
            this.CreationDateUTC = DateTime.UtcNow;
            this.UserPasswordHistoryList = new List<UserPasswordHistory>();
            this.UserRoles = new List<UserRole>();
        }
        #endregion

        #region Public Properties

        #region IUser Implementation
        [Key]
        public int Id
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string UserName
        {
            get; set;
        }
        #endregion

        [Required]
        [StringLength(68)]
        public string PasswordHash { get; set; }

        public DateTime CreationDateUTC { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool TwoFactorEnabled { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required]
        public string SecurityStamp { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("UserId")]
        public virtual ICollection<UserPasswordHistory> UserPasswordHistoryList { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}