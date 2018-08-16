using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;

namespace Nebula.CoreLibrary.Aspects.DynamicProxy
{
    public class PerformanceCounterAspect : NebulaAspect
    {
        readonly Stopwatch _stopwatch;
        private readonly ILogManager _logManager;
        private readonly IExceptionManager _exceptionManager;
        private readonly int _limitTime;
        public PerformanceCounterAspect(int limitTime = 2000)
        {
            _limitTime = limitTime;
            _stopwatch = Stopwatch.StartNew();
            _logManager = DependencyService.GetService<ILogManager>();
            _exceptionManager = DependencyService.GetService<IExceptionManager>();
        }

        public override void After(AspectExecutedContext context)
        {
            base.After(context);
            _stopwatch.Stop();

            string methodName = string.Format("{0}.{1}.{2}",
                context.Method.ReflectedType.Namespace,
                context.InvokeObject.GetType().Name, context.Method.Name
            );

            double time = _stopwatch.Elapsed.TotalMilliseconds;
            if (time > _limitTime)
            {
                try
                {
                    string message = $"{methodName} worked {time} ms (time)";
                    _logManager.Write(message, "performance info");
                }
                catch (Exception e)
                {
                    _exceptionManager.Handle(e);
                }
            }
           
        }
    }
}
