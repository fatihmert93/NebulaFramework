using System;
using System.Collections.Generic;
using System.Text;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership.Repositories.Contexts;

namespace Nebula.Membership.Repositories.EntityFramework.Postgresql
{
    public class EFNpgRoleRepository : EFRepositoryBase<Role, NpgMembershipContext>, IRoleRepository
    {
        public EFNpgRoleRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
