using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Model
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 总数据量（仅分页时有效）
        /// </summary>
        public int Total { get; set; }
    }
}
