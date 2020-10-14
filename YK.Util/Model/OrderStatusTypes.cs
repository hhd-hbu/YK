using System;
using System.Collections.Generic;
using System.Text;

namespace YK.Util.Model
{
    public enum OrderStatusTypes
    {
        待支付 = 0,
        已接单 = 1,
        申请退款 = 2,
        已完成 = 3,
        完成退款=4,
        退款失败 =  5
    }
}
