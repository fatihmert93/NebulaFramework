using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.Membership.Visitors
{
    public interface IMembershipVisitor
    {
        void Visit(IMembershipManager membershipManager);
    }
}
