using System;
using System.Collections.Generic;
using System.Text;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.SystemSettings.Repositories.Contexts;

namespace Nebula.SystemSettings.Repositories.EntityFramework.Postgresql
{
    public class EFNpgSystemSettingsRepository : EFRepositoryBase<SettingsEntity,NpgSettingsContext>, ISystemSettingsRepository
    {
        public EFNpgSystemSettingsRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
