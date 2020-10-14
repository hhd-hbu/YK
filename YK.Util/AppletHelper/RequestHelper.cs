using YK.Util.Extented;
using YK.Util.Helper;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace YK.Util.AppletHelper
{
    public class RequestHelper
    {


        /// <summary>
        /// code换取openid和session_key
        /// </summary>
        /// <param name="code"></param>
        /// <returns>openid</returns>
        public static v_petminiuserdetails NewCode2Session(string code)
        {
            string appid = ConfigHelper.Configuration["payConfig:appid"];
            string secret = ConfigHelper.Configuration["payConfig:secret"];
            string url = "https://api.weixin.qq.com/sns/jscode2session";
            string contentType = "application/x-www-form-urlencoded";
            string token = "appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
            var result = HttpHelper.PostData(url, token, contentType, null, null);
            var jObject = JObject.Parse(result);
            var openid = jObject["openid"].ToString();
            var session = jObject["session_key"].ToString();
            v_petminiuserdetails v_Petminiuserdetails = new v_petminiuserdetails
            {
                openid = openid,
                session_key = session
            };
            return v_Petminiuserdetails;
        }
        /// <summary>
        /// code换取openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns>openid</returns>
        public static string Code2Session(string code)
        {
            string appid = ConfigHelper.Configuration["payConfig:appid"];
            string secret = ConfigHelper.Configuration["payConfig:secret"];
            string url = "https://api.weixin.qq.com/sns/jscode2session";
            string contentType = "application/x-www-form-urlencoded";
            string token = "appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
            var result = HttpHelper.PostData(url, token, contentType, null, null);
            var jObject = JObject.Parse(result);
            var openid = jObject["openid"].ToString();
            return openid;
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="bookingNo"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public static string ToPay(string openid, string bookingNo, int total_fee)
        {
            Pay pay = new Pay();
            pay.nonce_str = PayHelper.GetRandomString(30);
            pay.spbill_create_ip = PayHelper.GetLocalIP();
            return Getprepay_id(pay, openid, bookingNo, total_fee);
        }

        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="pay"></param>
        /// <param name="openid"></param>
        /// <param name="bookingNo"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        private static string Getprepay_id(Pay pay, string openid, string bookingNo, int total_fee)
        {
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";//微信统一下单请求地址
            string contentType = "application/x-www-form-urlencoded";
            Dictionary<string, string> strParam = new Dictionary<string, string>();
            //小程序ID
            strParam.Add("appid", pay.appid);
            //附加数据
            strParam.Add("attach", pay.attach);
            //商品描述
            strParam.Add("body", pay.body);
            //商户号
            strParam.Add("mch_id", pay.mch_id);
            //随机字符串
            strParam.Add("nonce_str", pay.nonce_str);
            //通知地址 (异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。)
            strParam.Add("notify_url", pay.notify_url);
            //用户标识
            strParam.Add("openid", openid);
            //商户订单号
            strParam.Add("out_trade_no", bookingNo);
            //终端IP
            strParam.Add("spbill_create_ip", pay.spbill_create_ip);
            //标价金额
            strParam.Add("total_fee", total_fee.ToString());
            //交易类型
            strParam.Add("trade_type", pay.trade_type);
            strParam.Add("sign", XmlHelper.GetSignInfo(strParam, pay.key));
            //获取预支付ID
            string preInfo = HttpHelper.PostData(url, XmlHelper.CreateXmlParam(strParam), contentType, null, null);

            if (XmlHelper.GetXmlValue(preInfo, "return_code") == "SUCCESS")
            {
                if (XmlHelper.GetXmlValue(preInfo, "result_code") == "SUCCESS")
                {
                    string package = "prepay_id=" + XmlHelper.GetXmlValue(preInfo, "prepay_id");
                    //时间戳
                    string time = Extention.ToJsTimestamp(DateTime.Now).ToString();
                    //再次签名返回数据至小程序，调起支付
                    string strB = "appId=" + pay.appid + "&nonceStr=" + pay.nonce_str + "&package=" +
                        package + "&signType=MD5&timeStamp=" + time + "&key=" + pay.key;

                    ReSign resign = new ReSign();
                    resign.appId = pay.appid;
                    resign.timeStamp = time;
                    resign.nonceStr = pay.nonce_str;
                    resign.package = package;
                    resign.paySign = Extention.ToMD5String(strB).ToUpper();
                    resign.signType = "MD5";
                    resign.out_trade_no = bookingNo;
                    return JsonConvert.SerializeObject(resign);
                }
                else
                {
                    return XmlHelper.GetXmlValue(preInfo, "err_code_des");
                }
            }
            else
            {
                return XmlHelper.GetXmlValue(preInfo, "return_msg");
            }
        }

        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns>响应信息</returns>
        public static string GetQueryResult(string out_trade_no)
        {
            Pay pay = new Pay();
            pay.nonce_str = PayHelper.GetRandomString(30);
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            string contentType = "application/x-www-form-urlencoded";
            Dictionary<string, string> strParam = new Dictionary<string, string>();
            strParam.Add("appid", pay.appid);
            strParam.Add("mch_id", pay.mch_id);
            strParam.Add("nonce_str", pay.nonce_str);
            strParam.Add("out_trade_no", out_trade_no);
            strParam.Add("sign", XmlHelper.GetSignInfo(strParam, pay.key));
            string resultInfo = HttpHelper.PostData(url, XmlHelper.CreateXmlValue(strParam), contentType, null, null);
            return resultInfo;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="refund_fee"></param>
        /// <returns></returns>
        public static string PostRefund(string out_trade_no, int refund_fee)
        {
            Pay pay = new Pay();
            pay.nonce_str = PayHelper.GetRandomString(30);
            Dictionary<string, string> strParam = new Dictionary<string, string>();
            strParam.Add("appid", pay.appid);
            strParam.Add("mch_id", pay.mch_id);
            strParam.Add("nonce_str", pay.nonce_str);
            strParam.Add("out_refund_no", out_trade_no);
            strParam.Add("out_trade_no", out_trade_no);
            strParam.Add("refund_fee", refund_fee.ToString());
            strParam.Add("total_fee", refund_fee.ToString());
            strParam.Add("sign", XmlHelper.GetSignInfo(strParam, pay.key));

            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            string contentType = "application/x-www-form-urlencoded";
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2 cert = store.Certificates.Find(X509FindType.FindBySubjectName, "1592761851", false)[0];
            string refundinfo = HttpHelper.PostData(url, XmlHelper.CreateXmlValue(strParam), contentType, null, cert);
            return refundinfo;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="bookingNo"></param>
        /// <returns></returns>
        public static string CloseOrder(string bookingNo)
        {
            Pay pay = new Pay();
            pay.nonce_str = PayHelper.GetRandomString(30);
            Dictionary<string, string> strParam = new Dictionary<string, string>();
            strParam.Add("appid", pay.appid);
            strParam.Add("mch_id", pay.mch_id);
            strParam.Add("nonce_str", pay.nonce_str);
            strParam.Add("out_trade_no", bookingNo);
            strParam.Add("sign", XmlHelper.GetSignInfo(strParam, pay.key));

            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            string contentType = "application/x-www-form-urlencoded";
            string refundinfo = HttpHelper.PostData(url, XmlHelper.CreateXmlValue(strParam), contentType, null, null);
            return refundinfo;
        }
    }
    //实体Model

    //获取用户openid、session_key
    public class v_petminiuserdetails
    {
        /// <summary>
        /// 微信用户openId
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// session_key
        /// </summary>
        public string session_key { get; set; }
    }
    public class Pay
    {
        public string appid { get; set; } = ConfigHelper.Configuration["payConfig:appid"];
        public string attach { get; set; } = ConfigHelper.Configuration["payConfig:attach"];
        public string body { get; set; } = ConfigHelper.Configuration["payConfig:body"];
        public string mch_id { get; set; } = ConfigHelper.Configuration["payConfig:mch_id"];
        public string notify_url { get; set; } = ConfigHelper.Configuration["payConfig:notify_url"];
        public string nonce_str { get; set; }
        public string spbill_create_ip { get; set; }
        public string key { get; set; } = ConfigHelper.Configuration["payConfig:key"];
        public string trade_type { get; set; } = ConfigHelper.Configuration["payConfig:trade_type"];
    }

    /// <summary>
    /// 小程序调起支付数据签名
    /// 时间戳
    /// 随机字符串
    /// 统一下单接口返回的 prepay_id 
    /// 签名
    /// 签名类型，默认MD5
    /// </summary>
    public class ReSign
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }
        public string paySign { get; set; }
        public string signType { get; set; }
        public string out_trade_no { get; set; }
    }
}
