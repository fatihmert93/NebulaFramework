using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nebula.DataAccessLibrary.EntityFramework;
using Nebula.Membership.Repositories.Contexts;

namespace Nebula.Membership.Repositories.EntityFramework.Postgresql
{
    public class EFNpgUserGroupRepository : EFRepositoryBase<UserGroup, NpgMembershipContext>, IUserGroupRepository
    {
        public override void Update(UserGroup entity)
        {
            //Context.Entry(entity).State = EntityState.Detached;
            var oldEntity = Context.Set<UserGroup>().Include(v => v.UserGroupRoles)
                .First(v => v.Id == entity.Id);

            oldEntity.Name = entity.Name;

            Context.RemoveRange(oldEntity.UserGroupRoles);

            foreach (var groupRole in entity.UserGroupRoles)
            {
                oldEntity.UserGroupRoles.Add(groupRole);
            }

            base.Update(oldEntity);
        }


        public EFNpgUserGroupRepository(IDbContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
