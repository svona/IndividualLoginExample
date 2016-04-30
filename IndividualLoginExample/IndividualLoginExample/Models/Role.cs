using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Models
{
    [Table("Roles")]
    public class Role : IRole<int>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            this.UserRoles = new List<UserRole>();
        }
        #endregion

        #region Public Properties
        #region IRole Implementation
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        #endregion

        public DateTime CreationDateUTC { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("RoleId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}