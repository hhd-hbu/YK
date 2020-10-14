using YK.Util.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.DataRepository.Repository
{
    public class PostgreSqlRepository : DbRepository, IRepository
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PostgreSqlRepository()
            : base(null, DatabaseType.PostgreSql, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        public PostgreSqlRepository(string conStr)
            : base(conStr, DatabaseType.PostgreSql, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        /// <param name="entityNamespace">实体命名空间</param>
        public PostgreSqlRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.PostgreSql, entityNamespace)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库连接上下文</param>
        public PostgreSqlRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.PostgreSql, null)
        {
        }

        #endregion

    }
}
