using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;
using Nebula.DataAccessLibrary.Dapper;
using Nebula.DataAccessLibrary.Dapper.PostgreSql;

namespace Nebula.Membership.Repositories.Postgresql
{
    public class NpgCompanyRepository : NpgGenericRepository<Company>, ICompanyRepository
    {
        public NpgCompanyRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
