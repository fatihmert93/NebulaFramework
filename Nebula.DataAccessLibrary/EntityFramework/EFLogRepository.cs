using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.DataAccessLibrary.EntityFramework
{
    public class EFLogRepository : EFRepositoryBase<LogEntity, NebulaEntityContext>, ILogRepository
    {
        public EFLogRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
