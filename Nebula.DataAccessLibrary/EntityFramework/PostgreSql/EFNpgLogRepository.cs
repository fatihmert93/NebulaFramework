using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.DataAccessLibrary.EntityFramework.PostgreSql
{
    public class EFNpgLogRepository : EFRepositoryBase<LogEntity, NebulaNpgEntityContext>, ILogRepository
    {
        public EFNpgLogRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
