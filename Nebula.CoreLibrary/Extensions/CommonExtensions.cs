using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Nebula.CoreLibrary.Extensions
{
    public static class CommonExtensions
    {

        private const string _pepper = "P&0myWHq";

        public static string HashString(this string value)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(value + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }

        public static Guid ConvertGuid(this string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);
            return result;
        }

        public static string ConvertJsonFromObject(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static object ConvertObjectFromString(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }
    }
}
