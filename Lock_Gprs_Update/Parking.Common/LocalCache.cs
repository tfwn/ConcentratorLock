using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Parking.Common
{
    public class LocalCache
    {
        public static object Get(string key)
        {
            var cache = MemoryCache.Default;
            return cache[key];
        }

        public static void Set(string key, object obj)
        {
            var cache = MemoryCache.Default;
            cache.Set(key, obj, null);
        }

        public static void Set(string key, object obj, DateTimeOffset absoluteExpiration)
        {
            var cache = MemoryCache.Default;
            cache.Set(key, obj, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
        }

        public static void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

    }
}
