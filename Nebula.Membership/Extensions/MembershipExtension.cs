using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.CoreLibrary.IOC;
using Nebula.Membership.Repositories;

namespace Nebula.Membership.Extensions
{
    public static class MembershipExtension
    {

        private static readonly Lazy<IUserRepository> _userRepository = new Lazy<IUserRepository>(DependencyService.GetService<IUserRepository>);

        public static IUserRepository UserRepository = _userRepository.Value;


        public static IEnumerable<User> GetUsersChild(this User parantUser)
        {
            return GetChildrenUsers(parantUser);
        }

        private static IEnumerable<User> GetChildrenUsers(User parantUser)
        {
            var users = UserRepository.Query(v => v.ManagerId == parantUser.ManagerId).ToList();
            foreach (var user in users)
            {
                yield return user;
                foreach (var childrenUser in GetChildrenUsers(parantUser))
                {
                    var childrenUser1 = childrenUser;
                    yield return childrenUser1;
                }
            }
        }

    }
}
