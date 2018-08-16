using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nebula.DataAccessLibrary
{
    public static class DataHelper
    {
        
        /// <summary>
        /// This method using for get DefaultValue attribute's value for property
        /// </summary>
        /// <param name="obj">Which type we trying to get default value from property</param>
        /// <param name="propertyName">which property we looking for to get default value</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception">When the property that we using has no DefaultValue attribte then throwing error.</exception>
        public static T GetDefaultValue<T>(Type obj, string propertyName)
        {
            if (obj == null) return default(T);
            var prop = obj.GetProperty(propertyName);
            if (prop == null) return default(T);
            var attr = prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if(attr == null) throw new Exception($"Please use DefaultValue attribute for {propertyName} property");
            if (attr.Length != 1) return default(T);
            return (T)((DefaultValueAttribute)attr[0]).Value;
        }

        /// <summary>
        /// the type is nullable type or not. it is return true or false
        /// </summary>
        /// <param name="type">Questionable type for learning nullable or not</param>
        /// <returns>True or False</returns>
        public static bool IsNullableType(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
