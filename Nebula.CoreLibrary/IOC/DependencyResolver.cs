using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.IOC
{
    internal class DependencyResolver
    {
        private DependencyResolver()
        {

        }

        static readonly Lazy<DependencyResolver> _resolver = new Lazy<DependencyResolver>(() => new DependencyResolver());

        public static DependencyResolver Instance = _resolver.Value;

        private IContainer _currentResolver;

        public virtual IContainer CurrentResolver => _currentResolver ?? (_currentResolver = new CoreResolver());
    }
}
