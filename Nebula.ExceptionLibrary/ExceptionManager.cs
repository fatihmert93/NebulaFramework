using Nebula.CoreLibrary.Shared;
using Nebula.ExceptionLibrary.Handlers;
using Nebula.ExceptionLibrary.Publishers;
using Nebula.ExceptionLibrary.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary
{
    public class ExceptionManager : IExceptionManager
    {
        public void Handle(Exception e)
        {
            ExceptionHandlerFactory factory = ExceptionHandlerFactories.GetFactory(e);
            PublisherList list = factory.GetPublishers();
            ExceptionRule rule = factory.GetRule();
            rule.Run(e, list);
        }

        public void Wrap()
        {
            throw new NotImplementedException();
        }
    }
}
