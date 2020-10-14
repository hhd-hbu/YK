using YK.Util.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.DataRepository.Repository
{
    public class SqlServerRepository : DbRepository, IRepository
    {

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlServerRepository()
            : base(null, DatabaseType.SqlServer, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        public SqlServerRepository(string conStr)
            : base(conStr, DatabaseType.SqlServer, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conStr">数据库连接名</param>
        /// <param name="entityNamespace">实体命名空间</param>
        public SqlServerRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.SqlServer, entityNamespace)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库连接上下文</param>
        public SqlServerRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.SqlServer, null)
        {
        }

        #endregion
    }
}
