using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IndividualLoginExample.Properties;

namespace IndividualLoginExample.DataAnnotations
{
    public class PasswordStringLength : StringLengthAttribute
    {
        public PasswordStringLength(int maximumLength) 
            : base(maximumLength)
        {
            this.MinimumLength = Settings.Default.PasswordMinimumCharacterLength;
        }
    }
}