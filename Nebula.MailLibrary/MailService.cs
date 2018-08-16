using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;

namespace Nebula.MailLibrary
{
    public class MailService : IMailService
    {

        private const string HtmlEmailHeader = "<html><head><title></title></head><body style='font-family:arial; font-size:14px;'>";
        private const string HtmlEmailFooter = "<br/><p></p>  </body></html>";
        public string TemplateBody { get; set; }

        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        private IExceptionManager _exceptionManager;

        public MailService(IExceptionManager exceptionManager)
        {
            _exceptionManager = exceptionManager;

            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
        }



        private static string GetAdminEmail()
        {
            return ApplicationSettings.SmtpMailAddress;
        }

        private static string GetAdminEmailPassword()
        {
            return ApplicationSettings.SmtpMailPassword;
        }

        public string GetSmtpServer()
        {

            return ApplicationSettings.SmtpServer;
        }

        private static int GetSmtpPort()
        {
            return ApplicationSettings.SmtpPort;
        }

        public bool EnableSsl()
        {
            return ApplicationSettings.SmtpEnableSsl;
        }

        public async Task SendAsync()
        {
            MailMessage message = new MailMessage();

            foreach (var x in To)
            {
                if (IsValidEmailString(x) && !string.IsNullOrEmpty(x))
                    message.To.Add(x);
            }
            foreach (var x in CC)
            {
                message.CC.Add(x);
            }
            foreach (var x in BCC)
            {
                message.Bcc.Add(x);
            }

            message.Subject = Subject;
            message.Body = string.Concat(HtmlEmailHeader, Body, HtmlEmailFooter);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.From = new MailAddress(GetAdminEmail());
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                SmtpClient smtpClient = new SmtpClient(GetSmtpServer(), GetSmtpPort());
                NetworkCredential basicCredential = new NetworkCredential(GetAdminEmail(), GetAdminEmailPassword());
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = EnableSsl();

                await smtpClient.SendMailAsync(message);


            }
            catch (Exception ex)
            {
                string msg = ex.Message;

                throw ex;
            }
        }

        public void Send()
        {
            MailMessage message = new MailMessage();

            foreach (var x in To)
            {
                if (IsValidEmailString(x))
                    message.To.Add(x);

            }
            foreach (var x in CC)
            {
                message.CC.Add(x);
            }
            foreach (var x in BCC)
            {
                message.Bcc.Add(x);
            }

            message.Subject = Subject;
            message.Body = string.Concat(HtmlEmailHeader, Body, HtmlEmailFooter);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.From = new MailAddress(GetAdminEmail());
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                SmtpClient smtpClient = new SmtpClient(GetSmtpServer(), GetSmtpPort());
                NetworkCredential basicCredential = new NetworkCredential(GetAdminEmail(), GetAdminEmailPassword());
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = EnableSsl();

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public bool IsValidEmailString(string emailAddresses)
        {
            try
            {
                var addresses = emailAddresses.Split(',', ';')
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .ToArray();

                var reformattedAddresses = string.Join(",", addresses);

                var dummyMessage = new System.Net.Mail.MailMessage();
                dummyMessage.To.Add(reformattedAddresses);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
