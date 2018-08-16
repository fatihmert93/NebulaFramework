using Nebula.ExceptionLibrary.Publishers;
using Nebula.ExceptionLibrary.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Handlers
{
    internal abstract class ExceptionHandlerFactory
    {
        internal abstract PublisherList GetPublishers();
        internal abstract ExceptionRule GetRule();
    }
}
