using Nebula.CoreLibrary.IOC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Nebula.CoreLibrary
{
    public class Bootstrapper
    {
        public IContainer Container { get; set; }
        public void Start()
        {
            StartLogging();
            ExceptionHangling();
            DependencyResolving(DependencyResolver.Instance.CurrentResolver);
            Container = DependencyResolver.Instance.CurrentResolver;
        }

        public virtual void DependencyResolving(IContainer instanceCurrentResolver)
        {

        }

        public virtual void ExceptionHangling()
        {

        }

        public virtual void StartLogging()
        {
            Debug.WriteLine("Application started");
        }

        public void Stop()
        {
            Debug.WriteLine("Application stoped");
        }
    }
}
