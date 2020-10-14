using YK.Util.Extented;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace YK.Util.Helper
{
    /// <summary>
    /// XML文档操作帮助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 序列化为XML字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            Type type = obj.GetType();
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>  
        public static object Deserialize(Type type, string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }

        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type"></param>  
        /// <param name="xml"></param>  
        /// <returns></returns>  

        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }


        /// <summary>
        /// 获取签名数据
        ///</summary>
        /// <param name="strParam"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSignInfo(Dictionary<string, string> strParam, string key)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in strParam)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string sign = Extention.ToMD5String(sb.ToString()).ToUpper();
            return sign;
        }
        /// <summary>
        /// 集合转换XML数据
        /// </summary>
        /// <param name="strParam"></param>
        /// <returns></returns>
        public static string CreateXmlValue(Dictionary<string, string> strParam)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("<xml>");
            foreach (KeyValuePair<string, string> k in strParam)
            {
                strb.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
            }
            strb.Append("</xml>");
            return strb.ToString();
        }
        /// <summary>
        /// 集合转换XML数据 (签名)
        /// </summary>
        /// <param name="strParam">参数</param>
        /// <returns></returns>
        public static string CreateXmlParam(Dictionary<string, string> strParam)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, string> k in strParam)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sb.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sb.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
        /// <summary>
        /// 获取XML值
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="strData">字段值</param>
        /// <returns></returns>
        public static string GetXmlValue(string strXml, string strData)
        {
            string xmlValue = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strXml);
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
        }
        /// <summary>
        /// 返回通知 XML
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXml(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }
        /// <summary>
        /// 退款签名
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string CreateSign(string xml)
        {
            string key = "appid=" + GetXmlValue(xml, "appid") + "&attach=" + GetXmlValue(xml, "attach") + "&bank_type=" + GetXmlValue(xml, "bank_type") + "&cash_fee=" + GetXmlValue(xml, "cash_fee") +
                "&fee_type=" + GetXmlValue(xml, "fee_type") + "&is_subscribe=" + GetXmlValue(xml, "is_subscribe") + "&mch_id=" + GetXmlValue(xml, "mch_id") + "&nonce_str=" + GetXmlValue(xml, "nonce_str") +
                "&openid=" + GetXmlValue(xml, "openid") + "&out_trade_no=" + GetXmlValue(xml, "out_trade_no") + "&result_code=" + GetXmlValue(xml, "result_code") +
                "&return_code=" + GetXmlValue(xml, "return_code") + "&time_end=" + GetXmlValue(xml, "time_end") + "&total_fee=" + GetXmlValue(xml, "total_fee") +
                "&trade_type=" + GetXmlValue(xml, "trade_type") + "&transaction_id=" + GetXmlValue(xml, "transaction_id") + "&key=" + ConfigHelper.Configuration["payConfig:key"];
            string sign = Extention.ToMD5String(key.ToString()).ToUpper();
            return sign;
        }
    }
}
