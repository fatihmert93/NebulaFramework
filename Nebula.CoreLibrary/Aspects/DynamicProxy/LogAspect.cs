using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Aspects.DynamicProxy
{
    public class LogAspect : NebulaAspect
    {
        private readonly ILogManager _logManager;

        public LogAspect()
        {
            _logManager = DependencyService.GetService<ILogManager>();
        }


        public override void Before(AspectExecutingContext context)
        {

            string methodName = string.Format("{0}.{1}.{2}",
                context.Method.ReflectedType.Namespace,
                context.InvokeObject.GetType().Name,context.Method.Name
            );

            _logManager.Write($"{methodName} worked");

            base.Before(context);
        }
    }
}
