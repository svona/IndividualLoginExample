﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndividualLoginExample.Models
{
    [Table("BizObjects")]
    public class BizObject
    {
        [Key]
        public int Id { get; protected set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}