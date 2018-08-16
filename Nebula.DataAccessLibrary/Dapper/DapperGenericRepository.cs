using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.ExpressionBuilders;
using Nebula.DataAccessLibrary.ScriptGenerators;

namespace Nebula.DataAccessLibrary.Dapper
{
    public abstract class DapperGenericRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {

        protected readonly IConnectionFactory _connectionFactory;
        protected readonly IDbConnection _connection;
        protected readonly Type type;
        protected readonly string _tableName;
        protected IDbTransaction _dbTransaction;


        protected DapperGenericRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.Connection;
            type = typeof(TEntity);
            // ReSharper disable once VirtualMemberCallInConstructor
            _tableName = CreateTableName(type);

            if (!ApplicationSettings.AutoCreateTable) return;
            // ReSharper disable once VirtualMemberCallInConstructor
            if (TableExistsCheck()) return;
            // ReSharper disable once VirtualMemberCallInConstructor
            ExecuteCreateTableScript();
        }

        protected abstract string CreateTableName(Type type);

        protected abstract void ExecuteCreateTableScript();

        protected abstract bool TableExistsCheck();

        protected virtual dynamic InsertMapping(TEntity item)
        {
            return item;
        }

        protected virtual dynamic UpdateMapping(TEntity item)
        {
            return item;
        }

        public virtual IQueryable<TEntity> Query()
        {
            string query = "SELECT * FROM " + _tableName;
            return _connection.Query<TEntity>(query).AsQueryable();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predict)
        {

            WhereBuilder whereBuilder = new WhereBuilder();
            WherePart wherePart = whereBuilder.ToSql(predict);
            string whereQuery = wherePart.Sql;
            string query = $"SELECT * FROM  {_tableName} WHERE {whereQuery}";
            return _connection.Query<TEntity>(query, wherePart.Parameters).AsQueryable();
        }

        public IEnumerable<TEntity> ExecuteQuery(string sql, Dictionary<string,object> parameters = null)
        {
            return _connection.Query<TEntity>(sql, parameters);
        }

        public int ExecuteSql(string sql)
        {
            return 0;
        }

        public IEnumerable<dynamic> Query(string sql, Dictionary<string, object> parameters = null)
        {
            IEnumerable<dynamic> query = _connection.Query(sql,parameters);
            return query;
        }

        public void Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            _dbTransaction = _connection.BeginTransaction();
            entity.Id = Guid.NewGuid();
            entity.CreateDate = DateTime.Now;
            var parameters = (object)InsertMapping(entity);
            _connection.Insert<Guid>(_tableName, parameters, _dbTransaction);
        }

        public void Modify(TEntity entity)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            _dbTransaction = _connection.BeginTransaction();
            entity.UpdateDate = DateTime.Now;
            var parameters = (object)UpdateMapping(entity);
            _connection.Update(_tableName, parameters, _dbTransaction);
        }

        public void Remove(TEntity entity)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            entity.DeleteDate = DateTime.Now;
            _dbTransaction = _connection.BeginTransaction();
            _connection.Execute("DELETE FROM " + _tableName + " WHERE Id=@Id", new { ID = entity.Id }, _dbTransaction);
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            _connection.Close();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
            _connection.Close();
        }

        public TEntity FindAsNoTracking(Guid id)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(Guid Id)
        {
            TEntity item = default(TEntity);
            item = _connection.Query<TEntity>("SELECT * FROM " + _tableName + " WHERE Id=@Id", new { Id = Id }).SingleOrDefault();
            return item;
        }


        public void Dispose()
        {
            _connectionFactory?.Dispose();
            _connection?.Dispose();
            _dbTransaction?.Dispose();
        }
    }
}
