using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using DapperExtensions.Sql;
using DapperExtensions.Mapper;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.ExpressionBuilders;
using Nebula.DataAccessLibrary.ScriptGenerators;

namespace Nebula.DataAccessLibrary.Dapper.PostgreSql
{
    public class NpgGenericRepository<TEntity> : DapperGenericRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected override string CreateTableName(Type type)
        {
            return $"\"{type.Name}\"";
        }


        
        protected override void ExecuteCreateTableScript()
        {
            ScriptGeneratorBase scriptGenerator = ScriptGeneratorFactory.CreateInstance(type, typeof(PostgreScriptGenerator));
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
                        SELECT EXISTS (
                           SELECT 1
                           FROM   information_schema.tables 
                           WHERE  table_schema = 'public'
                           AND    table_name = '{typename}'
                           );";

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

        public NpgGenericRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
            
            

        }
    }
}
