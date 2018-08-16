using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Text;
using Nebula.CoreLibrary.Extensions;
using Nebula.Membership.Repositories;
using Nebula.Membership.Visitors;
using Nebula.Security.Bearer.Helpers;

[assembly: InternalsVisibleTo("Nebula.ClientLibrary")]
namespace Nebula.Membership
{
    public enum UserStatus
    {
        Deleted = 0,
        Active = 1,
        Deactive = 2
    }


    internal class MembershipManager : IMembershipManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserGroupService _userGroupService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserGroupRoleRepository _userGroupRoleRepository;
        private readonly ICompanyService _companyService;
        private readonly ICompanyRepository _companyRepository;

        public MembershipManager(IUserRepository userRepository, IUserGroupRepository userGroupRepository, IUserGroupService userGroupService, IRoleRepository roleRepository, IUserGroupRoleRepository userGroupRoleRepository, ICompanyService companyService, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userGroupService = userGroupService;
            _roleRepository = roleRepository;
            _userGroupRoleRepository = userGroupRoleRepository;
            _companyService = companyService;
            _companyRepository = companyRepository;
        }


        public Company CompanyFindAsNoTracking(Guid id)
        {
            return _companyRepository.FindAsNoTracking(id);
        }

        public IQueryable<Company> CompanyQuery()
        {
            return _companyRepository.Query();
        }

        public User UserCreate(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(UserExceptionTypes.EmailCannotBeNull);
            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException(UserExceptionTypes.PasswordCannotBeNull);

            if (_userRepository.Query(v => v.Email == user.Email).Any())
                throw new Exception("email already exists!");

            user.Id = Guid.NewGuid();
            user.IsEnable = true;
            user.Status = Convert.ToInt32(UserStatus.Active);
            user.Password = user.Password.HashString();
            _userRepository.Create(user);
            _userRepository.Commit();
            return user;

        }

        public User UserCreate(string email, string password)
        {
            User user = new User
            {
                Email = email,
                Password = password.HashString()
            };
            return UserCreate(user);
        }

        public void UserDelete(Guid id, bool softDelete = false)
        {
            User user = UserFind(id);
            if (!softDelete)
            {
                _userRepository.Delete(user);
                _userRepository.Commit();
            }
            else
            {
                user.Status = Convert.ToInt32(UserStatus.Deleted);
                UserUpdate(user);
            }
        }

        public User UserFindByEmail(string email)
        {
            if (!_userRepository.Query(v => v.Email == email).Any()) throw new Exception(UserExceptionTypes.EmailNotExists);
            var user = _userRepository.Query(v => v.Email == email).FirstOrDefault();
            user.Password = string.Empty;
            return user;
        }

        public string GeneratePassword(int length)
        {
            return RandomPassword.Generate(length);
        }

        public IQueryable<User> UserQuery(Guid companyId)
        {
            return _userRepository.Query(v => v.CompanyId == companyId);
        }

        public IQueryable<User> UserQuery()
        {
            return _userRepository.Query();
        }

        public User UserFind(Guid id)
        {
            var user = _userRepository.Find(id);
            return user;
        }

        public User UserFindAsNoTracking(Guid id)
        {
            var user = _userRepository.FindAsNoTracking(id);
            return user;
        }

        public Company CompanyCreate(Company company)
        {
            return _companyService.CreateCompany(company);
        }

        public Company CompanyCreate(string companyName)
        {
            return _companyService.CreateCompany(companyName);
        }

        public void CompanyDelete(Company company, bool softDelete = false)
        {
            _companyService.DeleteCompany(company, softDelete);
        }

        public Company CompanyUpdate(Company company)
        {
            return _companyService.UpdateCompany(company);
        }

        public Company CompanyFindByUser(User user)
        {
            return _companyService.GetCompany(user);
        }

        public Company CompanyFindByUserId(Guid userid)
        {
            var user = UserFind(userid);
            return _companyService.GetCompany(user);
        }

        public Company CompanyFind(Guid id)
        {
            return _companyRepository.Find(id);
        }

        public User UserUpdate(User user)
        {
            
            //user.Password = !string.IsNullOrEmpty(user.Password) ? user.Password.HashString() : model.Password;
            _userRepository.Update(user);
            _userRepository.Commit();
            return user;
        }

        private User UserFindByEmailWithPass(string email)
        {
            if(!_userRepository.Query(v => v.Email == email).Any()) throw new Exception(UserExceptionTypes.EmailNotExists);
            return _userRepository.Query(v => v.Email == email).FirstOrDefault();
        }

        public bool UserValidate(string email, string password)
        {
            User user = UserFindByEmailWithPass(email);
            if (user == null)
                throw new Exception(UserExceptionTypes.EmailNotExists);
            if(!user.IsEnable)
                throw new Exception("User is not enable!");
            string hashPass = password.HashString();
            return (user.Password == hashPass);
        }

        public string SignIn(string email, string password)
        {
            bool validate = UserValidate(email, password);
            if (!validate)
                throw new UnauthorizedAccessException("Email or password is incorrect");

            User user = UserFindByEmail(email);

            UserGroup userGroup = _userGroupRepository.Find(user.UserGroupId);

            var roles = _userGroupService.GetAuthorizedRoles(userGroup);


            string userGroupName = "";
            if (userGroup != null)
                userGroupName = userGroup.Name;

            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("fiver-secret-key"))
                .AddSubject(email)
                .AddIssuer("fiver.Security.Bearer")
                .AddAudience("fiver.Security.Bearer")
                .AddClaim("UserId", user.Id.ToString())
                .AddClaim("User", user.ConvertJsonFromObject())
                .AddClaim("Roles", roles.ConvertJsonFromObject())
                .AddClaim("UserGroup", userGroupName)
                .AddExpiry(10)
                .Build();

            return token.Value;
        }

        public void Accept(IMembershipVisitor membershipVisitor)
        {
            membershipVisitor.Visit(this);
        }

        public IEnumerable<Role> GetUserRoles(User user)
        {
            var userGroup = UserGroupFindByUser(user);
            return _userGroupService.GetAuthorizedRoles(userGroup);
        }

        public bool IsUserAuthorized(User user, string roleKey)
        {
            var userGroup = UserGroupFindByUser(user);
            return _userGroupService.IsUserGroupAuthorize(userGroup, roleKey);
        }

        public UserGroup UserGroupFind(Guid id)
        {
            return _userGroupRepository.Find(id);
        }

        public UserGroup UserGroupFindAsNoTracking(Guid id)
        {
            return _userGroupRepository.FindAsNoTracking(id);
        }

        public UserGroup UserGroupFindByUser(User user)
        {
            return _userGroupService.GetUserGroup(user);
        }

        public void RoleCreate(Role role)
        {
            _roleRepository.Create(role);
            _roleRepository.Commit();
        }

        public void RoleDelete(Role role)
        {
            _roleRepository.Delete(role);
            _roleRepository.Commit();
        }

        public void RoleUpdate(Role role)
        {
            _roleRepository.Update(role);
            _roleRepository.Commit();
        }

        public IQueryable<Role> RoleQuery()
        {
            return _roleRepository.Query();
        }

        public Role RoleFind(Guid id)
        {
            return _roleRepository.Find(id);
        }

        public Role RoleFindAsNoTracking(Guid id)
        {
            return _roleRepository.FindAsNoTracking(id);
        }

        public void UserGroupCreate(UserGroup userGroup)
        {
            _userGroupRepository.Create(userGroup);
            _userGroupRepository.Commit();
        }

        public void UserGroupDelete(UserGroup userGroup)
        {
            _userGroupRepository.Delete(userGroup);
            _userGroupRepository.Commit();
        }

        public void UserGroupDelete(Guid id)
        {
            var userGroup = _userGroupRepository.FindAsNoTracking(id);
            _userGroupRepository.Delete(userGroup);
        }

        public void UserGroupUpdate(UserGroup userGroup)
        {
            _userGroupRepository.Update(userGroup);
            _userGroupRepository.Commit();
        }

        public IQueryable<UserGroup> UserGroupQuery()
        {
            return _userGroupRepository.Query();
        }

        public void UserGroupRegisterToRole(UserGroup userGroup, Role role)
        {
            var userGroupRole = new UserGroupRole
            {
                UserGroupId = userGroup.Id,
                RoleId = role.Id
            };
            _userGroupRoleRepository.Create(userGroupRole);
            _userGroupRoleRepository.Commit();
        }

        public void UserRoleDelete(Guid userGroupId, Guid roleId)
        {
            var userGroupRole = _userGroupRoleRepository.Query(v => v.UserGroupId == userGroupId && v.RoleId == roleId)
                .FirstOrDefault();
            _userGroupRoleRepository.Delete(userGroupRole);
            _userGroupRoleRepository.Commit();

        }
    }
}
