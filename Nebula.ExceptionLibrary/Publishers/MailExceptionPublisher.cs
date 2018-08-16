using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;

namespace Nebula.ExceptionLibrary.Publishers
{

    internal class MailExceptionPublisher : ExceptionPublisher
    {
        private readonly IMailService mailSender;
        public MailExceptionPublisher()
        {
            mailSender = DependencyService.GetService<IMailService>();
        }

        internal override void Publish(Exception e)
        {
            mailSender.To.Add(ApplicationSettings.ErrorMailAddress);
            mailSender.Subject = $"{ApplicationSettings.ApplicationName} Error";
            mailSender.TemplateBody = $"Error Message: {e.Message.ToString()} \n Inner Message: {e.InnerException ?? e.InnerException} ";
        }
    }
}
