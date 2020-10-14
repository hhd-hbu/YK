using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Cache
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public interface IRedisCache : ICache
    {
        CSRedisClient Db { get; }
    }
}
