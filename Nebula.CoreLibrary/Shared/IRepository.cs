using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        T FindAsNoTracking(Guid id);
        T Find(Guid id);
        IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> predict);
        IEnumerable<T> ExecuteQuery(string sql, Dictionary<string,object> parameters = null);
        int ExecuteSql(string sql);
        IEnumerable<dynamic> Query(string sql, Dictionary<string,object> parameters = null);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Add(T entity);
        void Modify(T entity);
        void Remove(T entity);

        void Commit();
        void Rollback();
    }

    public interface IRepository : IRepository<EntityBase>
    {

    }
}
