using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nebula.CoreLibrary.Aspects.DynamicProxy
{
    public class CacheAspect : NebulaAspect
    {
        private readonly int _cacheDuration;
        private readonly ICacheManager _cacheManager;

        public CacheAspect(int cacheDuration = 60)
        {
            _cacheDuration = cacheDuration;

            _cacheManager = DependencyService.GetService<ICacheManager>();

        }

        public override void After(AspectExecutedContext context)
        {
            Console.WriteLine("cache aspect çalıştı");

            string methodName = string.Format("{0}.{1}",
                context.Method.ReflectedType.Namespace,
                context.InvokeObject.GetType().Name
            );
            var key = string.Format("{0}", methodName);

            if (_cacheManager.IsExists(key))
            {
                context.ReturnValue = _cacheManager.Get<object>(key);
            }
            else
            {
                object result = context.Method.Invoke(context.InvokeObject, context.args);
                context.ReturnValue = result;
                _cacheManager.Add(key, context.ReturnValue, _cacheDuration);
            }

        }
    }
}
