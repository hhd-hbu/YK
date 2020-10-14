using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.DI
{
    public abstract class BaseFilterAttribute : Attribute, IFilter
    {
        public abstract void OnActionExecuted(IInvocation invocation);
        public abstract void OnActionExecuting(IInvocation invocation);
    }
}
