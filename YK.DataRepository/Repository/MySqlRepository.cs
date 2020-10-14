using YK.Util.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.DataRepository.Repository
{
    public class MySqlRepository : DbRepository, IRepository
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MySqlRepository()
            : base(null, DatabaseType.MySql, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        public MySqlRepository(string conStr)
            : base(conStr, DatabaseType.MySql, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        /// <param name="entityNamespace">实体命名空间</param>
        public MySqlRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.MySql, entityNamespace)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库连接上下文</param>
        public MySqlRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.MySql, null)
        {
        }

        #endregion
    }
}
