﻿using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.DI
{
    public class AutofacHelper
    {
        public static ILifetimeScope Container { get; set; }

        /// <summary>
        /// 获取全局服务
        /// 警告：此方法使用不当会造成内存溢出,一般开发请勿使用此方法,请使用GetScopeService
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// 获取当前请求为生命周期的服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetScopeService<T>() where T : class
        {
            return (T)GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService(typeof(T));
        }
    }
}
