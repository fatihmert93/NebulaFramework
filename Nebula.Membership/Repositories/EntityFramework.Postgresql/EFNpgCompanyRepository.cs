using System;
using System.Collections.Generic;
using System.Text;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership.Repositories.Contexts;

namespace Nebula.Membership.Repositories.EntityFramework.Postgresql
{
    public class EFNpgCompanyRepository : EFRepositoryBase<Company, NpgMembershipContext>, ICompanyRepository
    {
        public EFNpgCompanyRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
