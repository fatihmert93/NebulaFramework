using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.CoreLibrary.Shared
{
    public interface IMailService
    {
        List<string> To { get; set; }
        List<string> CC { get; set; }
        List<string> BCC { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string TemplateBody { get; set; }
        Task SendAsync();
        void Send();
        bool IsValidEmailString(string emailAddress);
    }
}
