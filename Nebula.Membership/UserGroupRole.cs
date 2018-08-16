using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.Membership
{
    public class UserGroupRole : EntityBase
    {
        public Guid UserGroupId { get; set; }
        public Guid RoleId { get; set; }
        public UserGroup UserGroup { get; set; }
        public Role Role { get; set; }
    }
}
