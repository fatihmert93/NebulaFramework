using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;

namespace Nebula.Membership
{
    public class Company : EntityBase
    {
        public string Name { get; set; }
        public string CommercialName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }

    }
}
