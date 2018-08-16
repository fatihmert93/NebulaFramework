using Nebula.ExceptionLibrary.Publishers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Rules
{
    internal abstract class ExceptionRule
    {
        internal abstract void Run(Exception e, PublisherList list);
    }
}
