using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.Membership
{
    public class UserGroup : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
