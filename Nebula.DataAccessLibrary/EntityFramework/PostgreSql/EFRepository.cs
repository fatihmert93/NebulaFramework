using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.EntityFramework.PostgreSql
{
    public class EFRepository<TEntity> : EFRepositoryBase<TEntity, NebulaNpgEntityContext>
        where TEntity : EntityBase, new()
    {
        public EFRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }

    public class EFRepository : IRepository
    {
        private Type _type;
        public bool AutoTableGenerate { get; set; } = false;
        static object SpinLock = new object();


        [ThreadStatic]
        private static DbContext _context;

        private static DbContext CurrentContext()
        {
            lock (SpinLock)
            {

                return _context ?? (_context = new NebulaNpgEntityContext());
            }

        }

        public DbContext Context => CurrentContext();


        public EFRepository()
        {

        }


        public IEnumerable<EntityBase> ExecuteQuery(string sql)
        {
            return Context.Set<EntityBase>().FromSql(sql);
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

        public virtual void Create(EntityBase entity)
        {
            Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(EntityBase entity)
        {
            Modify(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(EntityBase entity)
        {
            Remove(entity);
            Context.SaveChanges();
        }

        public void Add(EntityBase entity)
        {
            entity.CreateDate = DateTime.Now;
            Context.Add(entity);
        }

        public void Modify(EntityBase entity)
        {
            entity.UpdateDate = DateTime.Now;
            Context.Update(entity);
        }

        public void Remove(EntityBase entity)
        {
            entity.DeleteDate = DateTime.Now;
            Context.Remove(entity);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        {

        }

        public virtual EntityBase FindAsNoTracking(Guid id)
        {
            return Context.Set<EntityBase>().AsNoTracking().SingleOrDefault(v => v.Id == id);
        }

        public virtual EntityBase Find(Guid Id)
        {
            return Context.Set<EntityBase>().SingleOrDefault(v => v.Id == Id);
        }

        public virtual IQueryable<EntityBase> Query()
        {
            return Context.Set<EntityBase>().Where(v => v.IsDeleted == false);
        }

        public IQueryable<EntityBase> Query(Expression<Func<EntityBase, bool>> predict)
        {
            return Context.Set<EntityBase>().Where(predict).AsNoTracking();
        }

        public IEnumerable<EntityBase> ExecuteQuery(string sql, Dictionary<string, object> parameters = null)
        {
            return Context.Set<EntityBase>().FromSql(sql, parameters);
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