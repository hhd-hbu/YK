using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.MtAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityConfigAttribute : Attribute
    {
    }
}
