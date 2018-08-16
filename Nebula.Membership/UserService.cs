using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.Membership.Repositories;

namespace Nebula.Membership
{
    public class UserService : IUserService
    {
        private readonly IUserGroupService _userGroupService;

        public UserService(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        public bool IsUserAuthorized(User user, string roleKey)
        {
            UserGroup userGroup = _userGroupService.GetUserGroup(user);
            return _userGroupService.IsUserGroupAuthorize(userGroup, roleKey);
        }
    }

    public interface IUserService
    {
        bool IsUserAuthorized(User user, string roleKey);

    }
}
