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



        public void Create()
        {
            
            var log1 = new LogEntity();
            
            var context1 = _dbContextFactory.CreateInstance(typeof(NebulaNpgEntityContext));

            context1.Add(log1);
            
            var context2 = _dbContextFactory.CreateInstance(typeof(NebulaNpgEntityContext));
        }
    }
}
