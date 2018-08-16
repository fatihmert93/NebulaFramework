using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Nebula.DataAccessLibrary.Dapper
{
    public static class DapperExtensions
    {
        public static void Insert<T>(this IDbConnection cnn, string tableName, dynamic param, IDbTransaction dbTransaction = null)
        {
            SqlMapper.Query<T>(cnn, DynamicQuery.GetInsertQuery(tableName, param), param, dbTransaction);
        }

        public static void Update(this IDbConnection cnn, string tableName, dynamic param, IDbTransaction dbTransaction = null)
        {
            SqlMapper.Execute(cnn, DynamicQuery.GetUpdateQuery(tableName, param), param, dbTransaction);
        }
    }
}
