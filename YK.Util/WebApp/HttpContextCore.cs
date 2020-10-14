using Microsoft.AspNetCore.Http;
using System;
using YK.Util.DI;

namespace YK.Util.WebApp
{
    public static class HttpContextCore
    {
        public static HttpContext Current { get => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext; }
    }
}
