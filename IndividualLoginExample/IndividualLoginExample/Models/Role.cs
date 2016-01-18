using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IndividualLoginExample.Models
{
    [Table("Roles")]
    public class Role : IRole<int>
    {
        #region IRole Implementation
        [Key]
        public int Id { get; protected set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        #endregion
    }
}