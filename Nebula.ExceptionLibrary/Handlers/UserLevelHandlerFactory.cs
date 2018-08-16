using Nebula.ExceptionLibrary.Publishers;
using Nebula.ExceptionLibrary.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Handlers
{
    internal class UserLevelHandlerFactory : ExceptionHandlerFactory
    {
        internal override PublisherList GetPublishers()
        {
            PublisherList list = new PublisherList();
            return list;
        }

        internal override ExceptionRule GetRule()
        {
            return new NoneRule();
        }
    }
}
