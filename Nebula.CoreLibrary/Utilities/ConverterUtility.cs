using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Nebula.CoreLibrary.Utilities
{
    public class ConverterUtility
    {
        public static string WriteFromObject(object obj)
        {
            //Create User object.  

            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();
            Type type = obj.GetType();
            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            ser.WriteObject(ms, obj);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        // Deserialize a JSON stream to a User object.  
        public static T ReadToObject<T>(string json)
        {
            T deserializedUser = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = (T)ser.ReadObject(ms);
            ms.Close();
            return deserializedUser;
        }

        public static string ObjectPropertySerializer(object obj)
        {
            string serialize = "";
            var props = obj.GetType().GetProperties();
            serialize = string.Join(",", props.Select(v => v.Name));
            return serialize;
        }
    }
}
