using System;
using System.Collections.Generic;
using System.Text;
using Nebula.ExceptionLibrary.Publishers;

namespace Nebula.ExceptionLibrary.Rules
{
    internal class NoneRule : ExceptionRule
    {
        internal override void Run(Exception e, PublisherList list)
        {
            foreach (ExceptionPublisher publisher in list)
            {

            }
        }
    }
}
