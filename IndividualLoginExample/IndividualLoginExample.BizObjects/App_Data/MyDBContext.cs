using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using CodeFirstStoreFunctions;
using IndividualLoginExample.BizObjects.Models;
using IndividualLoginExample.BizObjects.Helpers;

namespace IndividualLoginExample.BizObjects
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

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

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

            modelBuilder.Entity<UserRole>().MapToStoredProcedures(b => 
                b.Insert(c => c.HasName("UserRoleInsert"))
                .Update(c => c.HasName("UserRoleUpdate"))
                .Delete(c => c.HasName("UserRoleDelete")));

            modelBuilder.Entity<BizObject>().MapToStoredProcedures(b =>
                b.Insert(c => c.HasName("BizObjectInsert"))
                .Update(c => c.HasName("BizObjectUpdate"))
                .Delete(c => c.HasName("BizObjectDelete")));

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

        public ObjectResult<Role> GetRoles(int? id = null, string roleName = null, string nameContains = null, int? rowsToSkip = 0, int? rowsToTake = 5)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Role>(
                "GetRoles",
                GetObjParameter("Id", typeof(int), id),
                GetObjParameter("RoleName", typeof(string), roleName),
                GetObjParameter("NameContains", typeof(string), nameContains),
                GetObjParameter("RowsToSkip", typeof(int), rowsToSkip),
                GetObjParameter("RowsToTake", typeof(int), rowsToTake));
        }

        public List<Role> GetRoles(ref int totalRecords, int? id = null, string roleName = null, string nameContains = null, int? rowsToSkip = 0, int? rowsToTake = 5)
        {
            totalRecords = 0;
            var result = new List<Role>();

            using (var connection = new SqlConnection(this.Database.Connection.ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("GetRoles", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id.HasValue ? id.Value : (object)DBNull.Value;
                    cmd.Parameters.Add("RoleName", SqlDbType.NVarChar, 100).Value = !String.IsNullOrEmpty(roleName) ? roleName : (object)DBNull.Value;
                    cmd.Parameters.Add("NameContains", SqlDbType.NVarChar, 100).Value = !String.IsNullOrEmpty(nameContains) ? nameContains : (object)DBNull.Value;
                    cmd.Parameters.Add("RowsToSkip", SqlDbType.Int).Value = rowsToSkip;
                    cmd.Parameters.Add("RowsToTake", SqlDbType.Int).Value = rowsToTake;

                    using (var sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            totalRecords = sdr.GetValue<int>("TotalRecords");
                            result.Add(new Role
                            {
                                Id = sdr.GetValue<int>("Id"),
                                Name = sdr.GetValue<string>("Name"),
                                CreationDateUTC = sdr.GetValue<DateTime>("CreationDateUTC", DateTime.MinValue)
                            });
                        }
                    }
                }
            }

            return result;
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