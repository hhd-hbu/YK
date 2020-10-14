using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YK.Util
{
    public static class GlobalData
    {
        static readonly List<string> _fxAssemblies =
            new List<string> {
                "YK.Util",
                "YK.Entity",
                "YK.DataRepository",
                "YK.Business" ,
                "YK.IBusiness"};
        static GlobalData()
        {
            var assemblys = _fxAssemblies.Select(x => Assembly.Load(x)).ToList();
            List<Type> allTypes = new List<Type>();
            assemblys.ForEach(aAssembly =>
            {
                allTypes.AddRange(aAssembly.GetTypes());
            });

            FxAllTypes = allTypes;
        }

        /// <summary>
        /// 框架所有自定义类
        /// </summary>
        public static readonly List<Type> FxAllTypes;
    }
}
