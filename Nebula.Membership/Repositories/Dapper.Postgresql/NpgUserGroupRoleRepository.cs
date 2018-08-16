using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;

namespace Nebula.Membership.Repositories.Postgresql
{
    public class NpgUserGroupRoleRepository : NpgGenericRepository<UserGroupRole>, IUserGroupRoleRepository
    {
        public NpgUserGroupRoleRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
