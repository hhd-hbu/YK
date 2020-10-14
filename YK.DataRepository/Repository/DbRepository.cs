using YK.DataRepository.MyDbContext;
using YK.DataRepository.Transaction;
using YK.Util.DataAccess;
using YK.Util.Extented;
using YK.Util.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YK.DataRepository.Repository
{
    public class DbRepository : IRepository, IInternalTransaction
    {
        #region 数据库连接相关方法

        protected DbContext Db
        {
            get
            {
                if (_disposed)
                {
                    _db = Handle_BuildDbContext?.Invoke();
                    _disposed = false;
                }

                return _db;
            }
            set
            {
                _db = value;
            }
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            return Db;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="param">构造参数，可以为数据库连接字符串或者DbContext</param>
        /// <param name="dbType">数据库类型</param>
        public DbRepository(object param, DatabaseType dbType, string entityNamespace)
        {
            BuildParam = param;
            _dbType = dbType;
            _entityNamespace = entityNamespace;
            Handle_BuildDbContext = new Func<DbContext>(() =>
            {
                return DbFactory.GetDbContext(BuildParam, _dbType, _entityNamespace);
            });
            _db = Handle_BuildDbContext?.Invoke();
            _connectionString = _db.Database.GetDbConnection().ConnectionString;
        }

        #endregion

        #region 事件处理
        Func<DbContext> Handle_BuildDbContext { get; set; }

        #endregion

        #region 数据库相关
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string _connectionString { get; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        private DatabaseType _dbType { get; set; }
        /// <summary>
        /// 连接上下文DbContext
        /// </summary>
        private DbContext _db { get; set; }
        /// <summary>
        /// 建造DbConText所需参数
        /// </summary>
        private Object BuildParam { get; set; }
        /// <summary>
        /// 实体命名空间
        /// </summary>
        private string _entityNamespace { get; set; }

        public void CommitTransaction()
        {
            _transaction?.Commit();
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }
        public void DisposeTransaction()
        {
            _db.ChangeTracker.Entries().ToList().ForEach(aEntry=> { if (aEntry.State != EntityState.Detached) aEntry.State = EntityState.Detached; });
            _transaction?.Dispose();
            _openedTransaction = false;
        }
        public Action<string> HandleSqlLog { set => EFCoreSqlLogeerProvider.HandleSqlLog = value; }

        #endregion

        #region 私有成员
        protected IDbContextTransaction _transaction { get; set; }
        protected static PropertyInfo GetKeyProperty(Type type)
        {
            return GetKeyPropertys(type).FirstOrDefault();
        }
        protected static List<PropertyInfo> GetKeyPropertys(Type type)
        {
            var properties = type
                .GetProperties()
                .Where(x => x.GetCustomAttributes(true).Select(o => o.GetType().FullName).Contains(typeof(KeyAttribute).FullName))
                .ToList();

            return properties;
        }
        protected string GetDbTableName(Type type)
        {
            string tableName = string.Empty;
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;
            else
                tableName = type.Name;

            return tableName;
        }
        protected bool _openedTransaction { get; set; } = false;

        private List<DbParameter> CreateDbParamters(List<(string paramterName, object paramterValue)> paramters)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            List<DbParameter> dbParamters = new List<DbParameter>();
            paramters.ForEach(aParamter =>
            {
                var newParamter = dbProviderFactory.CreateParameter();
                newParamter.ParameterName = aParamter.paramterName;
                newParamter.Value = aParamter.paramterValue;
                dbParamters.Add(newParamter);
            });

            return dbParamters;
        }

        #endregion

        #region 事物相关

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _openedTransaction = true;
            _transaction = _db.Database.BeginTransaction(isolationLevel);
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            _openedTransaction = true;
            _transaction = await _db.Database.BeginTransactionAsync(isolationLevel);
        }

        public (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool success = true;
            Exception resEx = null;
            try
            {
                BeginTransaction(isolationLevel);

                action();

                CommitTransaction();
            }
            catch (Exception ex)
            {
                success = false;
                resEx = ex;
                RollbackTransaction();
            }
            finally
            {
                DisposeTransaction();
            }

            return (success, resEx);
        }

        public async Task<(bool Success, Exception ex)> RunTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool success = true;
            Exception resEx = null;
            try
            {
                await BeginTransactionAsync(isolationLevel);

                await action();

                CommitTransaction();
            }
            catch (Exception ex)
            {
                success = false;
                resEx = ex;
                RollbackTransaction();
            }
            finally
            {
                DisposeTransaction();
            }

            return (success, resEx);
        }

        #endregion

        #region Dispose

        private bool _disposed = false;
        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            DisposeTransaction();
            _db.Dispose();
        }

        #endregion

        #region 增加数据
        public int Insert<T>(T entity) where T : class, new()
        {
            return Insert(new List<T> { entity });
        }
        public async Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            return await InsertAsync(new List<T> { entity });
        }
        public int Insert<T>(List<T> entities) where T : class, new()
        {
            _db.AddRange(entities);
            return _db.SaveChanges();
        }
        public async Task<int> InsertAsync<T>(List<T> entities) where T : class, new()
        {
            await _db.AddRangeAsync(entities);

            return await _db.SaveChangesAsync();
        }

        #endregion

        #region 删除数据

        public int Delete<T>(T entity) where T : class, new()
        {
            return Delete(new List<T> { entity });
        }
        public async Task<int> DeleteAsync<T>(T entity) where T : class, new()
        {
            return await DeleteAsync(new List<T> { entity });
        }
        public int Delete<T>(List<T> entities) where T : class, new()
        {
            _db.RemoveRange(entities);
            return _db.SaveChanges();
        }
        public async Task<int> DeleteAsync<T>(List<T> entities) where T : class, new()
        {
            _db.RemoveRange(entities);

            return await _db.SaveChangesAsync();
        }
        public int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var deleteList = GetIQueryable<T>().Where(condition).ToList();
            return Delete(deleteList);
        }
        public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var deleteList = await GetIQueryable<T>().Where(condition).ToListAsync();
            return await DeleteAsync(deleteList);
        }

        public int DeleteAll<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 更新数据

        public int Update<T>(T entity) where T : class, new()
        {
            return Update(new List<T> { entity });
        }
        public async Task<int> UpdateAsync<T>(T entity) where T : class, new()
        {
            return await UpdateAsync(new List<T> { entity });
        }
        public int Update<T>(List<T> entities) where T : class, new()
        {
            _db.UpdateRange(entities);
            return _db.SaveChanges();
        }
        public async Task<int> UpdateAsync<T>(List<T> entities) where T : class, new()
        {
            _db.UpdateRange(entities);

            return await _db.SaveChangesAsync();
        }
        public int UpdateAny<T>(T entity, List<string> properties) where T : class, new()
        {
            return UpdateAny(new List<T> { entity }, properties);
        }
        public async Task<int> UpdateAnyAsync<T>(T entity, List<string> properties) where T : class, new()
        {
            return await UpdateAnyAsync(new List<T> { entity }, properties);
        }
        public int UpdateAny<T>(List<T> entities, List<string> properties) where T : class, new()
        {
            entities.ForEach(aEntity =>
            {
                properties.ForEach(aProperty =>
                {
                    _db.Entry(aEntity).Property(aProperty).IsModified = true;
                });
            });

            return _db.SaveChanges();
        }
        public async Task<int> UpdateAnyAsync<T>(List<T> entities, List<string> properties) where T : class, new()
        {
            return await Task.Run(()=> UpdateAny(entities, properties));
        }
        public int UpdateWhere<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new()
        {
            var list = GetIQueryable<T>().Where(whereExpre).ToList();
            list.ForEach(aData => set(aData));
            return Update(list);
        }
        public async Task<int> UpdateWhereAsync<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new()
        {
            var list = GetIQueryable<T>().Where(whereExpre).ToList();
            list.ForEach(aData => set(aData));
            return await UpdateAsync(list);
        }
        
        #endregion

        #region 查询数据

        public T GetEntity<T>(params object[] keyValue) where T : class, new()
        {
            var obj = _db.Set<T>().Find(keyValue);
            if (!obj.IsNullOrEmpty())
                _db.Entry(obj).State = EntityState.Detached;

            return obj;
        }
        public async Task<T> GetEntityAsync<T>(params object[] keyValue) where T : class, new()
        {
            var obj = await _db.Set<T>().FindAsync(keyValue);
            if (!obj.IsNullOrEmpty())
                _db.Entry(obj).State = EntityState.Detached;

            return obj;
        }
        public List<T> GetList<T>() where T : class, new()
        {
            return GetIQueryable<T>().ToList();
        }
        public async Task<List<T>> GetListAsync<T>() where T : class, new()
        {
            return await GetIQueryable<T>().ToListAsync();
        }
        public IQueryable<T> GetIQueryable<T>() where T : class, new()
        {
            return GetIQueryable(typeof(T)) as IQueryable<T>;
        }
        public IQueryable GetIQueryable(Type type)
        {
            var dbSet = _db.GetType().GetMethod("Set").MakeGenericMethod(type).Invoke(_db, null);
            var resQ = typeof(EntityFrameworkQueryableExtensions).GetMethod("AsNoTracking").MakeGenericMethod(type).Invoke(null, new object[] { dbSet });
            return resQ as IQueryable;
        }

        public IQueryable<T> ReadEntity<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            return _db.Set<T>().Where(whereLambda);
        }

        #endregion

        #region 执行Sql语句

        public int ExecuteSql(string sql, params (string paramterName, object paramterValue)[] paramters)
        {
            return _db.Database.ExecuteSqlRaw(sql, CreateDbParamters(paramters.ToList()).ToArray());
        }
        public async Task<int> ExecuteSqlAsync(string sql, params (string paramterName, object paramterValue)[] paramters)
        {
            return await _db.Database.ExecuteSqlRawAsync(sql, CreateDbParamters(paramters.ToList()).ToArray());
        }

        #endregion

    }
}
