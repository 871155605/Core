using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.Cache
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public string RedisConnectionString { get; set; }
    }

    public enum CacheType
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        MemoryCache,

        /// <summary>
        /// Redis缓存
        /// </summary>
        RedisCache
    }
}
