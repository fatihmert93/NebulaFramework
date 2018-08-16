using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Nebula.CoreLibrary.Shared;

namespace Nebula.CacheLibrary.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        protected MemoryCache Cache;

        public MemoryCacheManager()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        public void Add(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            Cache.Set(key, data, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheTime)));

        }


        public void Clear()
        {
            Cache.Dispose();
        }

        public bool IsExists(string key)
        {
            bool result = Cache.TryGetValue(key, out _);
            return result;
        }

        public void Delete(string key)
        {
            Cache.Remove(key);
        }
    }
}
