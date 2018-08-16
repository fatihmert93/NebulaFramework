using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Nebula.CoreLibrary.Extensions;
using Nebula.CoreLibrary.IOC;
using Nebula.Security.Bearer.Helpers;

namespace Nebula.Membership
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class NebulaAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roleKeys;

        public NebulaAuthorizeAttribute(params string[] roleKeys)
        {
            this._roleKeys = roleKeys;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            context.Result = TokenVerify(context) ? new StatusCodeResult((int)HttpStatusCode.OK) : new StatusCodeResult((int)HttpStatusCode.Unauthorized);


        }

        public bool TokenVerify(AuthorizationFilterContext context)
        {
            try
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                    return false;
                var token = context.HttpContext.Request.Headers["Authorization"].ToString();
                token = token.Replace("Bearer ", "");
                SecurityToken validatedToken;
                TokenValidationParameters validationParameters = new TokenValidationParameters();
                validationParameters.IssuerSigningKey = JwtSecurityKey.Create("fiver-secret-key");
                validationParameters.ValidAudience = "fiver.Security.Bearer";
                validationParameters.ValidIssuer = "fiver.Security.Bearer";
                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);
                Guid userid = principal.FindFirst("UserId").Value.ConvertGuid();
                IUserService userService = DependencyService.GetService<IUserService>();
                IMembershipManager membershipManager = DependencyService.GetService<IMembershipManager>();
                var user = membershipManager.UserFind(userid);
                return _roleKeys.Select(roleKey => userService.IsUserAuthorized(user, roleKey)).Any(result => result);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
