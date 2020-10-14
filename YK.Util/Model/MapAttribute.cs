using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Model
{
    public class MapAttribute : Attribute
    {
        public MapAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }
        public Type[] TargetTypes { get; }
    }
}
