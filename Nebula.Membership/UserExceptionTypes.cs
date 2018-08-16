using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.Membership
{
    public static class UserExceptionTypes
    {
        public static string UsernameCannotBeNull { get; set; } = "Username does not exists";
        public static string EmailNotExists { get; set; } = "Email does not exists";
        public static string EmailCannotBeNull { get; set; } = "Email can not be null";
        public static string PasswordCannotBeNull { get; set; } = "Password can not be null";


    }
}
