using System;
using System.Collections.Generic;
using System.Text;

namespace zhou.Services.Adapters
{
    public interface ICache
    {
        #region 判断缓存是否存在

        /// <summary>
        /// 是否存在键值
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        //bool ContainsKey(string Key, out object Value);

        bool ContainsKey(string Key);

        #endregion

        #region 设置缓存   
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        void SetCache(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="timeout">过期时间</param>
        /// <param name="IsAbsolute">[True:绝对过期][False:相对过期]</param>
        void SetCache(string key, object value, TimeSpan timeout, bool IsAbsolute = false);

        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取所有key
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllKeys();

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        object GetCache(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <typeparam name="T">数据类型</typeparam>
        T GetCache<T>(string key) where T : class;

        #endregion

        #region 删除缓存

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">主键</param>
        void RemoveCache(string key);

        #endregion
    }
}
