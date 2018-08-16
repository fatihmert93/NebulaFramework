using Microsoft.Extensions.DependencyInjection;
using Nebula.CoreLibrary.Aspects.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nebula.CoreLibrary.IOC
{
    public class CoreResolver : IContainer
    {
        private IServiceProvider serviceProvider
        {
            get => BuildServiceProvider();
            set { }
        }

        public IServiceCollection ServiceCollection { get; set; } = new ServiceCollection();

        public IServiceProvider BuildServiceProvider()
        {
            return ServiceCollection.BuildServiceProvider();
            
        }

        public void Register<TI, TT>() where TI : class where TT : class, TI
        {
            ServiceCollection.AddSingleton<TI, TT>();
        }

        public TI Resolve<TI>()
        {
            if (serviceProvider == null)
                serviceProvider = BuildServiceProvider();
            object obj = serviceProvider.GetService<TI>();
            return ProxyGenerator<TI>.Create((TI)obj);


            //return obj;
        }

        public void Register(Type serviceType, Type implementationType)
        {
            ServiceCollection.AddSingleton(serviceType, implementationType);
        }

        public void Register(Type serviceType, object implementationIntance)
        {
            ServiceCollection.AddSingleton(serviceType, implementationIntance);
        }

        public void RegisterTransient<TI, TT>() where TI : class where TT : class, TI
        {
            ServiceCollection.AddTransient<TI, TT>();
        }

        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            ServiceCollection.AddTransient(serviceType, implementationType);
        }
    }
}
