using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nebula.CoreLibrary.Utilities
{
    public class ReflectionUtility
    {
        public static IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var type = typeof(TBaseType);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            return types;
        }

        public static IEnumerable<Type> FindSubClassesOf(Type type)
        {

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            return types;
        }

        public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType)
        {

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(v => v.GetTypes()).ToList();
            return from x in types
                   let y = x.BaseType
                   where
                       (y != null && y.IsGenericType &&
                        openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition()))
                   select x;
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            var props = type.GetProperties();
            return props;
        }
    }
}
