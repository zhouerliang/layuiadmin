using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace zhou.Services.Adapters
{
    public class MemoryCache : ICache
    {
        private IMemoryCache _cache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        #region 判断缓存是否存在

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="Key">缓存的键</param>
        /// <param name="Value">True：缓存的值</param>
        /// <returns></returns>
        private bool ContainsKey(string Key, out object Value)
        {
            return _cache.TryGetValue(Key, out Value);
        }

        public bool ContainsKey(string Key)
        {
            return ContainsKey(Key, out object Value);
        }

        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        /// <summary>
        /// 根据缓存的键获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return _cache.Get(key);
        }

        /// <summary>
        /// 根据缓存的键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key) where T : class
        {
            return _cache.Get<T>(key);
        }

        #endregion

        #region 设置缓存

        public void SetCache(string Key, object Value)
        {
            _cache.Set(Key, Value);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key">缓存的键</param>
        /// <param name="Value">缓存的值</param>
        /// <param name="TimeOut">过期时间</param>
        /// <param name="IsAbsolute">[True:绝对过期][False:相对过期]</param>
        public void SetCache(string Key, object Value, TimeSpan TimeOut, bool IsAbsolute = false)
        {
            var cacheEntryOptions = IsAbsolute
                ? new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeOut)//绝对过期
                : new MemoryCacheEntryOptions().SetSlidingExpiration(TimeOut);//相对过期

            _cache.Set(Key, Value, cacheEntryOptions);
        }

        #endregion

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="Key"></param>
        public void RemoveCache(string Key)
        {
            _cache.Remove(Key);
        }

        #endregion
    }
}
