using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using IndividualLoginExample.Models;

namespace IndividualLoginExample
{
    public class MyDBContext : DbContext
    {
        #region Constructors
        public MyDBContext()
            : base(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString)
        {
            this.Database.Log += (s) => { System.Diagnostics.Debug.WriteLine(s.ToString()); };
        }
        #endregion
        #region Encapsulation of DbSets
        public T Add<T>(T entity) where T : class
        {
            return Set<T>().Add(entity);
        }

        public T Attach<T>(T entity) where T : class
        {
            return Set<T>().Attach(entity);
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
            modelBuilder.Entity<User>();
            //.MapToStoredProcedures(b =>
            //{
            //});

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}