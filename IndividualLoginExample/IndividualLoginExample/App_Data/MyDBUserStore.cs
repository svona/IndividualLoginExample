using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IndividualLoginExample.Models;
using IndividualLoginExample.Properties;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IndividualLoginExample
{
    public class MyDBUserStore : IUserStore<User, int>, 
        IUserPasswordStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>,
        IUserEmailStore<User, int>,
        IUserSecurityStampStore<User, int>
    {
        #region Private Members
        private MyDBContext db;
        private bool disposed = false;
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
            this.ThrowIfDisposed();
            this.db.Add(user);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        #region Read
        public Task<User> FindByIdAsync(int userId)
        {
            this.ThrowIfDisposed();
            var res = db.GetUsers(id: userId).SingleOrDefault();
            return Task.FromResult<User>(res);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            this.ThrowIfDisposed();
            var res = db.GetUsers(userName: userName).SingleOrDefault();
            return Task.FromResult<User>(res);
        }
        #endregion

        public Task UpdateAsync(User user)
        {
            this.ThrowIfDisposed();
            this.db.Attach(user);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        public Task DeleteAsync(User user)
        {
            this.ThrowIfDisposed();
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
                this.disposed = true;
            }
        }
        #endregion

        #region IUserPasswordStore Implementation
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<string>.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<bool>.FromResult(!String.IsNullOrWhiteSpace(user.PasswordHash));
        }
        #endregion

        #region IUserLockoutStore Implementation
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<DateTimeOffset>.FromResult(
                user.LockoutEndDateUtc.HasValue 
                ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) 
                : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? null : new DateTime?(lockoutEnd.DateTime);
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.AccessFailedCount++;
            return Task<int>.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<int>.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<bool>.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserTwoFactorStore Implementation
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<bool>.FromResult(user.TwoFactorEnabled);
        }
        #endregion

        #region IUserEmailStore Implementation
        public Task SetEmailAsync(User user, string email)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task<string>.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<bool>.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            this.ThrowIfDisposed();

            var res = db.GetUsers(email: email).SingleOrDefault();
            return Task.FromResult<User>(res);
        }
        #endregion

        #region IUserSecurityStampStore Implementation
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task<string>.FromResult(user.SecurityStamp);
        }
        #endregion

        #region Private Methods
        private void ThrowIfDisposed()
        {
            if (this.disposed)
                throw new ObjectDisposedException(nameof(db));
        }
        #endregion

        #region Internal Methods
        public IEnumerable<UserPasswordHistory> GetPasswords(int userId)
        {
            return this.db.GetUserPasswordHistory(userId: userId)
                .ToList();
        }
        #endregion
    }
}