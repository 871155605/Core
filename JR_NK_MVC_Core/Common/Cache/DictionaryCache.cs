using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.Cache
{
    public class DictionaryCache : ICache
    {
        public static readonly ConcurrentDictionary<string, Object> cacheDictionary = new ConcurrentDictionary<string, Object>();
        public long Del(params string[] key)
        {
            long i = 0;
            foreach (var item in key)
            {
                bool flag = cacheDictionary.TryRemove(item, out _);
                if(flag)i++;
            }
            return i;
        }

        public Task<long> DelAsync(params string[] key)
        {
            throw new NotImplementedException();
        }

        public Task<long> DelByPatternAsync(string pattern)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            return cacheDictionary.ContainsKey(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string Get(string key)
        {
            return (string)cacheDictionary.GetValueOrDefault(key);
        }

        public T Get<T>(string key)
        {
            return (T)cacheDictionary.GetValueOrDefault(key);
        }

        public Task<string> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Set(string key, object value)
        {
            if (this.Exists(key)) this.Del(key);
            return cacheDictionary.TryAdd(key,value);
        }

        public bool Set(string key, object value, TimeSpan expire)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
            throw new NotImplementedException();
        }
    }
}
