using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CodeFirstStoreFunctions;
using IndividualLoginExample.Models;

namespace IndividualLoginExample
{
    public class MyDBContext : DbContext
    {
        #region Constructors
        public MyDBContext()
            : base(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString)
        {
#if DEBUG
            this.Database.Log += (s) => { System.Diagnostics.Debug.WriteLine(s.ToString()); };
#endif
        }
        #endregion

        #region Factory Methods
        public static MyDBContext Create()
        {
            return new MyDBContext();
        }
        #endregion

        #region Encapsulation of DbSets
        public T Add<T>(T entity) where T : class
        {
            return Set<T>().Add(entity);
        }

        public T Attach<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry<T>(entity).State = EntityState.Modified;
            return entity;
        }

        public T Detach<T>(T entity) where T : class
        {
            Entry<T>(entity).State = EntityState.Detached;
            return entity;
        }

        public T Remove<T>(T entity) where T : class
        {
            if (Entry<T>(entity).State == EntityState.Detached)
            {
                Entry<T>(entity).State = EntityState.Deleted;
                return Entry<T>(entity).Entity;
            }
            else
                return Set<T>().Remove(entity);
        }
        #endregion

        #region Method Overrides
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FunctionsConvention<MyDBContext>("dbo"));

            var userConfig = modelBuilder.Entity<User>();
            userConfig.MapToStoredProcedures(b =>
            {
                b.Insert(c => c.HasName("UserInsert"));
                b.Update(c => c.HasName("UserUpdate"));
                b.Delete(c => c.HasName("UserDelete"));
            }).HasMany(b => b.UserPasswordHistoryList).WithRequired(b => b.User).WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>().MapToStoredProcedures(b =>
            b.Insert(c => c.HasName("RoleInsert"))
            .Update(c => c.HasName("RoleUpdate"))
            .Delete(c => c.HasName("RoleDelete")));

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Stored Procedures
        // [DbFunction("dbo", "GetUsers")]
        public ObjectResult<User> GetUsers(int? id = null, string userName = null, string email = null)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<User>(
                "GetUsers",
                GetObjParameter("Id", typeof(int), id),
                GetObjParameter("UserName", typeof(string), userName),
                GetObjParameter("Email", typeof(string), email));
        }

        public ObjectResult<UserPasswordHistory> GetUserPasswordHistory(int? id = null, int? userId = null)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<UserPasswordHistory>(
                "GetUserPasswordHistory",
                GetObjParameter("Id", typeof(int), id),
                GetObjParameter("Userid", typeof(int), userId));
        }

        public ObjectResult<Role> GetRoles(int? id = null, string roleName = null)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Role>(
                "GetRoles",
                GetObjParameter("Id", typeof(int), id),
                GetObjParameter("RoleName", typeof(string), roleName));
        }
        #endregion

        #region Private Methods
        private ObjectParameter GetObjParameter(string name, Type type, object value)
        {
            var result = new ObjectParameter(name, type);
            if (value != null)
                result.Value = value;

            return result;
        }
        #endregion
    }
}