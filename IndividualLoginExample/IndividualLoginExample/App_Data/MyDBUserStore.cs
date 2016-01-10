using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IndividualLoginExample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IndividualLoginExample
{
    public class MyDBUserStore : IUserStore<User, int>, IUserPasswordStore<User, int>
    {
        #region Private Methods
        private MyDBContext db;
        #endregion

        #region Constructors
        public MyDBUserStore() 
            : base()
        {
            this.db = new MyDBContext();
        }
        #endregion

        #region IUserStore Implementation

        #region CRUD Methods
        public Task CreateAsync(User user)
        {
            this.db.Add(user);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        #region Read
        public Task<User> FindByIdAsync(int userId)
        {
            var res = db.GetUsers(id: userId).SingleOrDefault();
            return Task.FromResult<User>(res);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var res = db.GetUsers(userName: userName).SingleOrDefault();
            return Task.FromResult<User>(res);
        }
        #endregion

        public Task UpdateAsync(User user)
        {
            this.db.Attach(user);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        public Task DeleteAsync(User user)
        {
            this.db.Remove(user);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }
        #endregion

        public void Dispose()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }
        #endregion

        #region IUserPasswordStore Implementation
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task<string>.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task<bool>.FromResult(!String.IsNullOrWhiteSpace(user.PasswordHash));
        }
        #endregion
    }
}