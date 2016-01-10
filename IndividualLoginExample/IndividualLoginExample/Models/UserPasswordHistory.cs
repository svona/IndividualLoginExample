using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndividualLoginExample.Models
{
    [Table("UserPasswordHistory")]
    public class UserPasswordHistory
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordHistory"/> class.
        /// </summary>
        public UserPasswordHistory()
        {
            CreationDateUTC = DateTime.UtcNow;
        }
        #endregion

        #region Public Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(68)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreationDateUTC { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        #endregion
    }
}