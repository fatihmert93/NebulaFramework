using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.ScriptGenerators;

namespace Nebula.DataAccessLibrary.Dapper.Sql
{
    public class SqlGenericRepository<TEntity> : DapperGenericRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected override string CreateTableName(Type type)
        {
            return type.Name;
        }

        protected override void ExecuteCreateTableScript()
        {
            ScriptGeneratorBase scriptGenerator = ScriptGeneratorFactory.CreateInstance(type, typeof(MsSqlScriptGenerator));
            var script = scriptGenerator.CreateTableScript();
            _connection.Execute(script);
        }

        protected override bool TableExistsCheck()
        {
            Type tip = typeof(TEntity);
            string typename = tip.Name;
            bool exists;
            try
            {
                string query = $@"
                         SELECT 1 FROM sys.tables AS T
                         INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
                         WHERE S.Name = 'dbo' AND T.Name = '{typename}'";

                int result = _connection.Query<int>(query).FirstOrDefault();

                if (result == 1)
                    exists = true;
                else
                {
                    exists = false;
                }
            }
            catch (Exception)
            {
                exists = false;
            }
            finally
            {
                _connection.Close();
            }

            return exists;
        }


        public SqlGenericRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
