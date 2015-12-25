using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Models
{
    [Table("Users")]
    public class User : IUser<int>
    {
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
    }
}