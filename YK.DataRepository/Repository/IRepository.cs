using YK.DataRepository.Transaction;
using YK.Util.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YK.DataRepository.Repository
{
    public interface IRepository : IBaseRepository,ITransaction
    {
        #region 数据库相关
        DbContext GetDbContext();
        /// <summary>
        /// SQL日志处理方法
        /// </summary>
        /// <value>
        /// The handle SQL log.
        /// </value>
        Action<string> HandleSqlLog { set; }
        #endregion

        #region 增加数据
        #endregion

        #region 删除数据

        #endregion

        #region 更新数据

        #endregion

        #region 查询数据
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        T GetEntity<T>(params object[] keyValue) where T : class, new();
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        Task<T> GetEntityAsync<T>(params object[] keyValue) where T : class, new();
        IQueryable<T> GetIQueryable<T>() where T : class, new();
        IQueryable GetIQueryable(Type type);
        IQueryable<T> ReadEntity<T>(Expression<Func<T, bool>> whereLambda = null) where T : class,new();

        #endregion

        #region 执行Sql语句
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        int ExecuteSql(string sql, params (string paramterName, object paramterValue)[] paramters);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        Task<int> ExecuteSqlAsync(string sql, params (string paramterName, object paramterValue)[] paramters);

        #endregion
    }
}
