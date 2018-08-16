using System;
using System.Collections.Generic;
using System.Text;
using Nebula.ExceptionLibrary.Publishers;

namespace Nebula.ExceptionLibrary.Rules
{
    internal class AllRule : ExceptionRule
    {
        internal override void Run(Exception e, PublisherList list)
        {
            foreach (ExceptionPublisher publisher in list)
            {
                try
                {
                    publisher.Publish(e);
                }
                catch
                {

                }
            }
        }
    }
}
