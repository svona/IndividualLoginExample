using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Crypto
{
    public class MyCustomPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return PasswordHash.CreateHash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // PasswordVerificationResult.SuccessRehashNeeded
            bool result = PasswordHash.ValidatePassword(providedPassword, hashedPassword);
            return result ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}