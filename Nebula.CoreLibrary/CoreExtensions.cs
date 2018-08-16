using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Nebula.CoreLibrary
{
    public static class CoreExtensions
    {
        public static dynamic AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);

            return expandoDict;
        }

        public static bool GuidEmpty(this string value)
        {
            try
            {
                Guid guid = Guid.Parse(value);
                if (guid == Guid.Empty)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
            
        }
    }
}
