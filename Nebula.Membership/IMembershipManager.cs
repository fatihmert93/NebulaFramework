using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.Membership.Visitors;

namespace Nebula.Membership
{
    public interface IMembershipManager
    {
        // companies
        Company CompanyCreate(Company company);
        Company CompanyCreate(string companyName);
        void CompanyDelete(Company company, bool softDelete = false);
        Company CompanyUpdate(Company company);
        Company CompanyFindByUser(User user);
        Company CompanyFindByUserId(Guid userid);
        Company CompanyFind(Guid id);
        Company CompanyFindAsNoTracking(Guid id);
        IQueryable<Company> CompanyQuery();
        // users
        User UserCreate(User user);
        User UserCreate(string email, string password);
        void UserDelete(Guid id,bool softDelete = false);
        User UserFindByEmail(string email);
        string GeneratePassword(int length);
        IQueryable<User> UserQuery(Guid companyId);
        IQueryable<User> UserQuery();
        User UserFind(Guid id);
        User UserFindAsNoTracking(Guid id);
        User UserUpdate(User user);
        bool UserValidate(string email, string password);
        string SignIn(string email, string password);
        void Accept(IMembershipVisitor membershipVisitor);
        IEnumerable<Role> GetUserRoles(User user);
        bool IsUserAuthorized(User user, string roleKey);
        // roles
        void RoleCreate(Role role);
        void RoleDelete(Role role);
        void RoleUpdate(Role role);
        IQueryable<Role> RoleQuery();
        Role RoleFind(Guid id);
        Role RoleFindAsNoTracking(Guid id);
        // user groups
        void UserGroupCreate(UserGroup userGroup);
        void UserGroupDelete(UserGroup userGroup);
        void UserGroupDelete(Guid id);
        void UserGroupUpdate(UserGroup userGroup);
        IQueryable<UserGroup> UserGroupQuery();
        UserGroup UserGroupFind(Guid id);
        UserGroup UserGroupFindAsNoTracking(Guid id);
        UserGroup UserGroupFindByUser(User user);
        void UserGroupRegisterToRole(UserGroup userGroup, Role role);
        void UserRoleDelete(Guid userGroupId, Guid roleId);



    }
}
