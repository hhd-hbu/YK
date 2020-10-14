using YK.Util.Extented;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Helper
{
    /// <summary>
    /// GUID帮助类
    /// </summary>
    public static class GuidHelper
    {
        /// <summary>
        /// 生成主键
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            return Guid.NewGuid().ToSequentialGuid().ToUpper();
        }
    }
}
