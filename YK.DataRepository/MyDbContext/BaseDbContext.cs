using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using YK.DataRepository.MyDbContext;
using YK.Util;
using YK.Util.DataAccess;
using YK.Util.Extented;
using YK.Util.Model;
using YK.Util.MtAttributes;

namespace YK.DataRepository.MyDbContext
{
    public class BaseDbContext:DbContext
    {
        public BaseDbContext(string nameOrConStr , DatabaseType dbType , string entityNamespace)
        {
            _nameOrConStr = nameOrConStr;
            _dbType = dbType;
            _entityNamespace = entityNamespace.IsNullOrEmpty() ? "YK.Entity" : entityNamespace;
        }
        private string _nameOrConStr { get; set; }
        private DatabaseType _dbType { get; set; }
        private string _entityNamespace { get; }
        private static ILoggerFactory _loger { get; } = new LoggerFactory(new ILoggerProvider[] { new EFCoreSqlLogeerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_nameOrConStr.IsNullOrEmpty())
                _nameOrConStr = GlobalSwitch.DefaultDbConName;

            string conStr = DbProviderFactoryHelper.GetConStr(_nameOrConStr);
            switch (_dbType)
            {
                case DatabaseType.SqlServer: optionsBuilder.UseSqlServer(_nameOrConStr).EnableSensitiveDataLogging(); break;
                case DatabaseType.MySql: optionsBuilder.UseMySQL(_nameOrConStr); break;
                default: throw new Exception("暂不支持该数据库！");
            }

            optionsBuilder.UseLoggerFactory(_loger);
        }

        /// <summary>
        /// 初始化DbContext
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TheEntity>();
            //以下代码最终目的就是将所有需要的实体类调用上面的方法加入到DbContext中，成为其中的一部分
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            MethodInfo entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
            var types = Assembly.Load("YK.Entity").GetTypes();

            List<Type> Entypes = types
                .Where(x => x.GetCustomAttribute(typeof(TableAttribute), false) != null && x.FullName.Contains(_entityNamespace))
                .ToList();
            foreach (var type in Entypes)
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null);
            }
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("YK.Entity"),
            //    x => x.GetCustomAttribute(typeof(EntityConfigAttribute), false) != null && x.FullName.Contains(_entityNamespace));
        }
    }
}
