using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace YK.IBusiness
{
    public interface IBaseBusiness
    {
        #region 事物提交

        /// <summary>
        /// 开始事物提交
        /// </summary>
        (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// 结束事物提交
        /// </summary>
        Task<(bool Success, Exception ex)> RunTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        #endregion

        #region 增加数据

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        int Insert<T>(T entity) where T : class, new();
        /// <summary>
        /// 插入数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(T entity) where T : class, new();

        /// <summary>
        /// 插入数据列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        int Insert<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 插入数据列表(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertAsync<T>(List<T> entities) where T : class, new();
        #endregion

        #region 删除数据


        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        int Delete<T>(T entity) where T : class, new();
        /// <summary>
        /// 删除一条数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(T entity) where T : class, new();

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">数据列表</param>
        int Delete<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 删除多条数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 通过条件删除数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件</param>
        int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();
        /// <summary>
        /// 通过条件删除数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        int Update<T>(T entity) where T : class, new();
        /// <summary>
        /// 更新一条数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(T entity) where T : class, new();
        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象</param>
        /// <returns></returns>
        int Update<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 更新多条数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象</param>
        /// <returns></returns>
        Task<int> UpdateAsync<T>(List<T> entities) where T : class, new();
        /// <summary>
        /// 更新一条数据,某些属性
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">需要更新的字段</param>
        int UpdateAny<T>(T entity, List<string> properties) where T : class, new();
        /// <summary>
        /// 更新一条数据,某些属性(异步)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">需要更新的字段</param>
        Task<int> UpdateAnyAsync<T>(T entity, List<string> properties) where T : class, new();
        /// <summary>
        /// 更新多条数据,某些属性
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">需要更新的字段</param>
        /// <returns></returns>
        int UpdateAny<T>(List<T> entities, List<string> properties) where T : class, new();
        /// <summary>
        /// 更新多条数据,某些属性(异步)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">需要更新的字段</param>
        /// <returns></returns>
        Task<int> UpdateAnyAsync<T>(List<T> entities, List<string> properties) where T : class, new();
        /// <summary>
        /// 根据表达式修改数据
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="whereExpre">表达式</param>
        /// <param name="set"></param>
        /// <returns></returns>
        int UpdateWhere<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new();
        /// <summary>
        /// 根据表达式修改数据(异步)
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="whereExpre">表达式</param>
        /// <param name="set"></param>
        /// <returns></returns>
        Task<int> UpdateWhereAsync<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new();
        #endregion

        #region 查询数据

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        T GetEntity<T>(params object[] keyValue) where T : class, new();
        /// <summary>
        /// 获取实体(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        Task<T> GetEntityAsync<T>(params object[] keyValue) where T : class, new();

        /// <summary>
        /// 获取表的所有数据，当数据量很大时不要使用！
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        List<T> GetList<T>() where T : class, new();
        /// <summary>
        /// 获取表的所有数据(异步)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returnsaq>
        Task<List<T>> GetListAsync<T>() where T : class, new();

        /// <summary>
        /// 获取实体对应的表，延迟加载，主要用于支持Linq查询操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        IQueryable<T> GetIQueryable<T>() where T : class, new();
        IQueryable GetIQueryable(Type type);

        /// <summary>
        /// lambda  查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> ReadEntity<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new();

        #endregion

        #region 执行Sql语句
        /// <summary>
        /// 通过参数执行Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        int ExecuteSql(string sql, params (string paramterName, object paramterValue)[] paramters);

        /// <summary>
        /// 通过参数异步执行Sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, params (string paramterName, object paramterValue)[] paramters);


        #endregion
    }
}
