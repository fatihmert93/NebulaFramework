using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nebula.CoreLibrary.Aspects.DynamicProxy
{
    public class ProxyGenerator<T> : DispatchProxy
    {
        private T _decorated;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                AspectExecutingContext beforecontext = new AspectExecutingContext();
                AspectExecutedContext aftercontext = new AspectExecutedContext();
                beforecontext.Method = targetMethod;
                beforecontext.MethodName = targetMethod.Name;
                beforecontext.args = args;
                beforecontext.InvokeObject = _decorated;

                aftercontext.MethodName = targetMethod.Name;
                aftercontext.args = args;
                aftercontext.Method = targetMethod;
                aftercontext.InvokeObject = _decorated;

                Type instanceType = _decorated.GetType();

                NebulaAspect[] typeAspects = (NebulaAspect[])(instanceType.GetCustomAttributes(typeof(NebulaAspect), true));

                foreach (var aspect in typeAspects)
                {
                    aspect.Before(beforecontext);
                }

                Type[] paramTypes = targetMethod.GetParameters().Select(v => v.ParameterType).ToArray();

                NebulaAspect[] aspects = (NebulaAspect[])(instanceType.GetMethod(targetMethod.Name,paramTypes).GetCustomAttributes(typeof(NebulaAspect), true));

                beforecontext.MethodName = targetMethod.Name;
                aftercontext.MethodName = targetMethod.Name;
                aftercontext.args = args;

                foreach (NebulaAspect aspect in aspects)
                    aspect.Before(beforecontext);

                

                if (aftercontext.ReturnValue != null)
                    return aftercontext.ReturnValue;

                var result = targetMethod.Invoke(_decorated, args);

                foreach (var aspect in typeAspects)
                {
                    aspect.After(aftercontext);
                }

                foreach (NebulaAspect aspect in aspects)
                {
                    aspect.After(aftercontext);

                }

                return result;
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, ProxyGenerator<T>>();
            ((ProxyGenerator<T>)proxy).SetParameters(decorated);

            return (T)proxy;
        }

        private void SetParameters(T decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }
            _decorated = decorated;
        }


    }
}
