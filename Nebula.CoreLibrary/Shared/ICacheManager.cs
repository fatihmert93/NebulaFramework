using System;
using System.Collections.Generic;
using System.Text;

namespace Nebula.CoreLibrary.Shared
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Add(string key, object value, int duration);
        bool IsExists(string key);
        void Delete(string key);
        void Clear();
    }
}
