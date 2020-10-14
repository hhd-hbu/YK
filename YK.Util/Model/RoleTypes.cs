using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Model
{
    /// <summary>
    /// 系统角色类型
    /// </summary>
    [Flags]
    public enum RoleTypes
    {
        /// <summary>
        /// 一般用户
        /// </summary>
        General = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,

        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 2,
    }
}
