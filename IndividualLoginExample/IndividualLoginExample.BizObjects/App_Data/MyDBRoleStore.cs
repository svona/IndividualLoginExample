using System;
using System.Threading.Tasks;
using IndividualLoginExample.BizObjects.Models;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace IndividualLoginExample.BizObjects
{
    public class MyDBRoleStore : IRoleStore<Role, int>, 
        IQueryableRoleStore<Role, int>
    {
        #region Private Members
        private MyDBContext db;
        private bool disposed = false;
        #endregion

        #region Contructors     
        public MyDBRoleStore()
        {
            this.db = new MyDBContext();
        }
        #endregion

        #region IRoleStore Implementation

        #region CRUD
        public Task CreateAsync(Role role)
        {
            this.ThrowIfDisposed();
            this.db.Add(role);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        #region Read
        public Task<Role> FindByIdAsync(int roleId)
        {
            this.ThrowIfDisposed();
            var res = db.GetRoles(id: roleId).SingleOrDefault();
            return Task.FromResult<Role>(res);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            this.ThrowIfDisposed();
            var res = db.GetRoles(roleName: roleName).SingleOrDefault();
            return Task.FromResult<Role>(res);
        }
        #endregion
        
        public Task UpdateAsync(Role role)
        {
            this.ThrowIfDisposed();
            this.db.Attach(role);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }

        public Task DeleteAsync(Role role)
        {
            this.ThrowIfDisposed();
            this.db.Remove(role);
            var res = this.db.SaveChanges();
            return Task.FromResult(res);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }
        #endregion

        #endregion

        #region IQueryableRoleStore Implementation
        public IQueryable<Role> Roles
        {
            get
            {
                return this.db.GetRoles().ToList().AsQueryable();
            }
        }
        #endregion

        #region Private Methods
        private void ThrowIfDisposed()
        {
            if (this.disposed)
                throw new ObjectDisposedException(nameof(db));
        }
        #endregion
    }
}