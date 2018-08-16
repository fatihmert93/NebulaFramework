using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;

namespace Nebula.Membership.Repositories.Postgresql
{
    public class NpgRoleRepository : NpgGenericRepository<Role> , IRoleRepository
    {
        public NpgRoleRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
