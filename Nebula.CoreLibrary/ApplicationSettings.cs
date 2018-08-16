using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary
{
    public class ApplicationSettings
    {
        public static string ApplicationName { get; set; }
        public static string ConnectionString { get; set; }
        public static string ErrorMailAddress { get; set; }
        public static string SmtpMailAddress { get; set; }
        public static string SmtpMailPassword { get; set; }
        public static int SmtpPort { get; set; }
        public static string SmtpServer { get; set; }
        public static bool SmtpEnableSsl { get; set; }
        public static bool AutoCreateTable { get; set; } = true;
        public static Dictionary<string,object> AppSettings { get; set; } = new Dictionary<string, object>();

        public static string TryGetValueFromAppSettings(string key)
        {
            try
            {
                string value = AppSettings[key].ToString();
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
        }
    }
}
