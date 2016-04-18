using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

using HZTVApi.Common;
using System.Net;


namespace HZTVApi.Manager
{
    public class BusinessFun : PubFun
    {

        #region 模版
        /// <summary>
        /// 加载邮件模板，并替换相关变量，如果邮件遇到乱码，请确保两点：1.邮件模板的以ANSI文件格式保存，2.邮件模板的html编码格式为gb2312
        /// 说明：如果是非www.htinns.com下运行，则邮件中的域名将批量替换为AppSettings["Mail_replaceDomain"]
        /// </summary>
        /// <param name="templatefile">模板文件名称</param>
        /// <param name="aParams">替换值hash表</param>
        /// <returns></returns>
        public static string LoadSMSMailContent(string templatefile, IDictionary<string, string> aParams)
        {
            string mailcont = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/template/" + templatefile), Encoding.Default);
            foreach (KeyValuePair<string, string> kv in aParams)
            {
                mailcont = Regex.Replace(mailcont, "{" + kv.Key + "}", kv.Value, RegexOptions.IgnoreCase);
            }
            
            if (HttpContext.Current != null)
            {
                string Mail_replaceDomain =  GetAppSetting("Mail_replaceDomain");
                if (Mail_replaceDomain.Length > 0)
                {
                    mailcont = Regex.Replace(mailcont, "www\\.htinns\\.com", Mail_replaceDomain, RegexOptions.IgnoreCase);
                }
            }
            return mailcont;
        }

        public static string LoadTemplate(string templatefile, IDictionary<string, string> aParams)
        {
            string str = File.ReadAllText(HttpContext.Current.Server.MapPath("/config/template/" + templatefile), Encoding.Default);
            StringBuilder buf = new StringBuilder(str);
            foreach (KeyValuePair<string, string> kv in aParams)
            {
                buf.Replace("{" + kv.Key + "}", kv.Value);
            }
            return buf.ToString();
        }

        public static string LoadSMSContent(string key, IDictionary<string, string> aParams)
        {
            string str = GetAppSetting("sms.template." + key);
            if (string.IsNullOrEmpty(str)) return str;

            StringBuilder buf = new StringBuilder(str);
            foreach (KeyValuePair<string, string> kv in aParams)
            {
                buf.Replace("{" + kv.Key + "}", kv.Value);
            }
            return buf.ToString();
        }
        #endregion

        //
    }
}
