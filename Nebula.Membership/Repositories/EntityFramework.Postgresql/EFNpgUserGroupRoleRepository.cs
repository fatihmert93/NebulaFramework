using System;
using System.Collections.Generic;
using System.Text;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership.Repositories.Contexts;

namespace Nebula.Membership.Repositories.EntityFramework.Postgresql
{
    public class EFNpgUserGroupRoleRepository : EFRepositoryBase<UserGroupRole, NpgMembershipContext>, IUserGroupRoleRepository
    {
        public EFNpgUserGroupRoleRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
