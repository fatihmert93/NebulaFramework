using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.Membership.Repositories;

namespace Nebula.Membership
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserGroupRoleRepository _userGroupRoleRepository;


        public UserGroupService(IUserGroupRepository userGroupRepository, IRoleRepository roleRepository, IUserGroupRoleRepository userGroupRoleRepository)
        {
            _userGroupRepository = userGroupRepository;
            _roleRepository = roleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
        }

        public UserGroup GetUserGroup(User user)
        {
            var userGroupId = user.UserGroupId;
            var userGroup = _userGroupRepository.Find(userGroupId);
            return userGroup;
        }

        public IEnumerable<Role> GetAuthorizedRoles(UserGroup userGroup)
        {
            var usergroupid = userGroup.Id;
            var userGroupRoles = _userGroupRoleRepository.Query(v => v.UserGroupId == usergroupid).ToList();
            var roleids = userGroupRoles.Select(v => v.RoleId).ToList();
            var roles = _roleRepository.Query(v => roleids.Contains(v.Id)).ToList();
            return roles;
        }

        public bool IsUserGroupAuthorize(UserGroup userGroup, string roleKey)
        {
            IEnumerable<Role> authorizedRoles = GetAuthorizedRoles(userGroup);
            var authorizedRoleKeys = authorizedRoles.Select(v => v.Key).ToList();
            return authorizedRoleKeys.Contains(roleKey);
        }
    }


    public interface IUserGroupService
    {
        UserGroup GetUserGroup(User user);
        IEnumerable<Role> GetAuthorizedRoles(UserGroup userGroup);
        bool IsUserGroupAuthorize(UserGroup userGroup, string roleKey);
    }
}
