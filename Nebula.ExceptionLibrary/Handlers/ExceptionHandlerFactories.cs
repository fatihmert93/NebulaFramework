using Nebula.ExceptionCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.ExceptionLibrary.Handlers
{
    internal class ExceptionHandlerFactories
    {
        internal static ExceptionHandlerFactory GetFactory(Exception e)
        {
            if (e is CoreLevelException)
                return new CoreLevelHandlerFactory();
            else if (e is CriticalLevelException)
                return new CriticalLevelHandlerFactory();
            else if (e is UserLevelException)
                return new UserLevelHandlerFactory();
            else
                return new NetLevelHandlerFactory();
        }
    }
}
