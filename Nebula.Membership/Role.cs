using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.Membership
{
    public class Role : EntityBase
    {
        public string Key { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
