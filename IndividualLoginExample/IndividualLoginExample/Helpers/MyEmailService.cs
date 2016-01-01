using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Helpers
{
    internal class MyEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}