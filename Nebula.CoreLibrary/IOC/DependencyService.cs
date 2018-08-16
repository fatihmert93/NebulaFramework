using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Nebula.CoreLibrary.IOC
{
    public class DependencyService
    {
        public static T GetService<T>()
        {
            return DependencyResolver.Instance.CurrentResolver.Resolve<T>();
        }

        public static object GetService(Type type)
        {
            return DependencyResolver.Instance.CurrentResolver.BuildServiceProvider().GetService(type);
        }
    }
}
