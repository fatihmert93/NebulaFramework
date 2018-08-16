using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nebula.CoreLibrary.Aspects
{
    public class AspectExecutingContext
    {
        public string MethodName { get; set; }
        public MethodBase Method { get; set; }
        public object InvokeObject { get; set; }
        public object[] args { get; set; }
    }

    public class AspectExecutedContext : AspectExecutingContext
    {
        public object ReturnValue { get; set; }

        public object InvokeObject { get; set; }
        public new MethodBase Method { get; set; }
        public object[] args { get; set; }
    }
}
