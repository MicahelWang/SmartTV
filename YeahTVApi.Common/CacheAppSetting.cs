using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Common
{
    public class CacheAppSetting
    {
        private static Dictionary<String, String> cache = new Dictionary<string, string>();

        static CacheAppSetting()
        {

        }

        //public static string GetAppSetting(string key)
        //{
        //    if(cache!= null && cache.ContainsKey(key))
        //        return cache[key];
        //    string rst = System.Configuration.ConfigurationManager.AppSettings[key];
        //    if (string.IsNullOrWhiteSpace(rst)) rst = string.Empty;
        //    try
        //    {
        //        cache.Add(key, rst);
        //    }
        //    catch(Exception err) {
        //        HTOutputLog.SaveError("CacheAppSetting-GetAppSettingGetAppSetting(string key)", err);
        //    }
        //    return rst;
        //}

        //public static string GetAppSetting(string key, string val)
        //{
        //    if(cache!= null && cache.ContainsKey(key))
        //        return cache[key];
        //    string rst = System.Configuration.ConfigurationManager.AppSettings[key];
        //    if (string.IsNullOrWhiteSpace(rst)) rst = val;
        //     try
        //    {
        //        cache.Add(key, rst);
        //    }
        //     catch (Exception err)
        //     {
        //         HTOutputLog.SaveError("CacheAppSetting-GetAppSettingGetAppSetting(string key, string val)", err);
        //     }
        //    return rst;
        //}

        //public static int GetAppSetting(string key, int val)
        //{
        //    if(cache!= null && cache.ContainsKey(key))
        //        return int.Parse(cache[key]);
        //    int rst = val;
        //    if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], out rst))
        //    {
        //        rst = val;
        //    }
        //    try
        //    {
        //        cache.Add(key, rst.ToString());
        //    }
        //    catch (Exception err)
        //    {
        //        HTOutputLog.SaveError("CacheAppSetting-GetAppSettingGetAppSetting(string key, int val)", err);
        //    }
        //    return rst;
        //}
    }
}
