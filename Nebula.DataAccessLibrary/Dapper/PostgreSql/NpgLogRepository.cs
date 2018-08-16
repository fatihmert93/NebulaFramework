using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.Dapper.PostgreSql
{
    public class NpgLogRepository : NpgGenericRepository<LogEntity>, ILogRepository
    {
        public NpgLogRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public object GetDtoDatas()
        {
            return null;
        }

        //protected override dynamic Mapping(LogEntity item)
        //{
        //    return new
        //    {
        //        Id = item.Id.ToString(),
        //        Message = item.Message,
        //        LogType = item.LogType,
        //        UpdateDate = item.UpdateDate

        //    };
        //}
    }
}
