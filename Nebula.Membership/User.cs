using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Nebula.CoreLibrary.Attributes;
using Nebula.CoreLibrary.Shared;
using Nebula.Membership.Extensions;

namespace Nebula.Membership
{
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellPhone { get; set; }

        [DefaultValue(true)]
        public bool IsEnable { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid UserGroupId { get; set; }
        public int Status { get; set; }
        public Guid CompanyId { get; set; }
        public UserGroup UserGroup { get; set; }
        public Company Company { get; set; }
        public User Manager { get; set; }
        public virtual ICollection<User> Employees { get; set; }

    }
}
