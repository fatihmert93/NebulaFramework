using Microsoft.EntityFrameworkCore;
using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nebula.CoreLibrary;
using Nebula.DataAccessLibrary.ScriptGenerators;

namespace Nebula.DataAccessLibrary.EntityFramework
{
    public abstract class EFRepositoryBase<TEntity, TContext> : IRepository<TEntity>, IDisposable
        where TEntity : EntityBase
        where TContext : DbContext, new()
    {


        private Type _type;
        public bool AutoTableGenerate { get; set; } = false;
        static object SpinLock = new object();
        

        
        private DbContext _context;

        

        protected IDbContextFactory _dbContextFactory;

        protected DbContext Context => _context;


        protected EFRepositoryBase(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            if (_context == null)
                _context = _dbContextFactory.CreateInstance(typeof(NebulaNpgEntityContext));

            _type = typeof(TEntity);

        }


        public IEnumerable<TEntity> ExecuteQuery(string sql)
        {
            return Context.Set<TEntity>().FromSql(sql);
        }

        public IEnumerable<dynamic> Query(string sql)
        {
            return Context.Set<dynamic>().FromSql(sql);
        }

        public int ExecuteSql(string sql)
        {
            return Context.Database.ExecuteSqlCommand(sql);
        }

        public IEnumerable<dynamic> Query(string sql, Dictionary<string, object> parameters = null)
        {
            return Context.Set<dynamic>().FromSql(sql, parameters);
        }

        public virtual void Create(TEntity entity)
        {
            Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Modify(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            Remove(entity);
            Context.SaveChanges();
        }

        public void Add(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            Context.Add(entity);
        }

        public void Modify(TEntity entity)
        {
            entity.UpdateDate = DateTime.Now;
            Context.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            entity.DeleteDate = DateTime.Now;
            entity.IsDeleted = true;
            Context.Update(entity);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        {
            
        }

        public virtual TEntity FindAsNoTracking(Guid id)
        {
            return Context.Set<TEntity>().AsNoTracking().SingleOrDefault(v => v.Id == id);
        }

        public virtual TEntity Find(Guid Id)
        {
            return Context.Set<TEntity>().SingleOrDefault(v => v.Id == Id);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().Where(v => v.IsDeleted == false);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predict)
        {
            return Context.Set<TEntity>().Where(predict).AsNoTracking();
        }

        public IEnumerable<TEntity> ExecuteQuery(string sql, Dictionary<string, object> parameters = null)
        {
            return Context.Set<TEntity>().FromSql(sql, parameters);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        
    }
}
