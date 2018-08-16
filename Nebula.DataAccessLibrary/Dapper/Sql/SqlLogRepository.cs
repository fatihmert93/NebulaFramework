using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.Dapper.Sql
{
    public class SqlLogRepository : SqlGenericRepository<LogEntity>, ILogRepository
    {
        public SqlLogRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
