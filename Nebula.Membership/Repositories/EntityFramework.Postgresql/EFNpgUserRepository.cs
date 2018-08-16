using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership.Repositories.Contexts;

namespace Nebula.Membership.Repositories.EntityFramework.Postgresql
{
    public class EFNpgUserRepository : EFRepositoryBase<User,NpgMembershipContext>, IUserRepository
    {
        public EFNpgUserRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
            
        }


        public override void Create(User entity)
        {
            base.Create(entity);
            
            
        }
    }
}
