using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Aspects.DynamicProxy
{
    public class CacheRemoveAspect : NebulaAspect
    {
        private readonly ICacheManager _cacheManager;

        public CacheRemoveAspect()
        {
            _cacheManager = DependencyService.GetService<ICacheManager>();
        }

        public override void After(AspectExecutedContext context)
        {
            base.After(context);

            string key = string.Format("{0}.{1}",
                context.Method.ReflectedType.Namespace,
                context.InvokeObject.GetType().Name
            );

            _cacheManager.Delete(key);

        }
    }
}
