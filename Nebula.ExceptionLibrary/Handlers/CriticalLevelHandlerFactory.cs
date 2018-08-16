using Nebula.ExceptionLibrary.Publishers;
using Nebula.ExceptionLibrary.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Handlers
{
    internal class CriticalLevelHandlerFactory : ExceptionHandlerFactory
    {
        internal override PublisherList GetPublishers()
        {
            PublisherList list = new PublisherList();
            list.Add(new MailExceptionPublisher());
            list.Add(new SqlExceptionPublisher());
            return list;
        }

        internal override ExceptionRule GetRule()
        {
            return new AllRule();
        }
    }
}
