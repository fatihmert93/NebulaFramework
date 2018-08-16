using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Nebula.CoreLibrary.Shared;
using Microsoft.Extensions.Caching.Redis;
using Nebula.CoreLibrary;

namespace Nebula.CacheLibrary.Redis
{
    public class RedisCacheManager : IDistributedCacheManager
    {
        private readonly IDistributedCache _distributedCache;

        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }

        public RedisCacheManager()
        {
            string connectionString = ApplicationSettings.TryGetValueFromAppSettings("REDIS_CONNECTIONSTRING");

            RedisCacheOptions options = new RedisCacheOptions()
            {
                Configuration = connectionString,
                InstanceName = "master"
            };
            _distributedCache = new RedisCache(options);
            
        }

        public T Get<T>(string key)
        {
            var dataString = _distributedCache.GetString(key);
            var data = JsonConvert.DeserializeObject<T>(dataString);
            return data;
        }

        public void Add(string key, object value, int duration)
        {
            var data = JsonConvert.SerializeObject(value);
            var dataByte = Encoding.UTF8.GetBytes(data);
            //_distributedCache.Set(key,dataByte);
            _distributedCache.SetString(key, data);
        }

        public bool IsExists(string key)
        {
            var dataString = _distributedCache.GetString(key);
            return string.IsNullOrEmpty(dataString);
        }

        public void Delete(string key)
        {
            _distributedCache.Remove(key);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        
    }
}
