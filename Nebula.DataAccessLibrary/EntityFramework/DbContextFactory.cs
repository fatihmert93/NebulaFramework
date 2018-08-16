using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Nebula.DataAccessLibrary.EntityFramework
{
    public interface IDbContextFactory
    {
        DbContext CreateInstance(Type instanceType);
    }
    
    
    
    public class DbContextFactory : IDbContextFactory, IDisposable
    {
        private readonly ConcurrentBag<DbContext> _contextList;

        public DbContextFactory()
        {
            this._contextList = new ConcurrentBag<DbContext>();
        }


        public DbContext CreateInstance(Type instanceType)
        {
            if(instanceType.BaseType != typeof(DbContext)) throw new Exception("Not supported db context type!");
            DbContext context = (DbContext)Activator.CreateInstance(instanceType);
            _contextList.Add(context);

            return context;
        }

        public void Dispose()
        {
            foreach (var dbContext in _contextList)
            {
                dbContext.Dispose();
            }
        }
    }
}
