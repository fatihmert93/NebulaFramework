using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Nebula.CoreLibrary.IOC
{
    public interface IContainer
    {
        IServiceCollection ServiceCollection { get; set; }
        IServiceProvider BuildServiceProvider();
        

        void Register<TI, TT>()
            where TI : class
            where TT : class, TI;
        TI Resolve<TI>();

        void Register(Type serviceType, Type implementationType);
        void Register(Type serviceType, object implementationIntance);

        void RegisterTransient<TI, TT>() where TI : class where TT : class, TI;
        void RegisterTransient(Type serviceType, Type implementationType);
    }
}
