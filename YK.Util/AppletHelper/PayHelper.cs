using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace YK.Util.AppletHelper
{
    public class PayHelper
    {
        /// <summary>
        /// 生成随机串    
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        private static Random _random { get; } = new Random();
        public static string GetRandomString(int length)
        {
            const string key = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length < 1)
                return string.Empty;
            byte[] buffer = new byte[8];
            ulong bit = 31;
            StringBuilder sb = new StringBuilder((length / 5 + 1) * 5);
            while (sb.Length < length)
            {
                _random.NextBytes(buffer);
                buffer[5] = buffer[6] = buffer[7] = 0x00;
                UInt64 result = BitConverter.ToUInt64(buffer, 0);
                while (result > 0 && sb.Length < length)
                {
                    Int32 index = (int)(bit & result);
                    sb.Append(key[index]);
                    result = result >> 5;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到执行统一下单接口的服务器的ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            string strLocalIP = null;
            //得到计算机名 
            //string strPcName = Dns.GetYKstName();
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var IPadd in ipEntry.AddressList)
            {
                //得到本地IP地址 
                strLocalIP = IPadd.ToString();
            }
            return strLocalIP;
        }
    }
}
