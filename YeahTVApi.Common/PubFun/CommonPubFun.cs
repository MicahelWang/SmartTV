using YeahTVApi.DomainModel;
//处理Json时候需要
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.IO.Compression;
//处理 strust => bytes 时需要
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using YeahTVApi.DomainModel.Models.ViewModels;


namespace YeahTVApi.Common
{
    /// <summary>
    /// 公共方法库
    /// </summary>
    public static partial class PubFun
    {

        public static DateTime DBMinDateTime = new DateTime(1900, 1, 1);
        public static DateTime DBMaxDateTime = new DateTime(2079, 1, 1);


        /// <summary>
        /// 入住人保存sessionkey的前缀
        /// </summary>
        public static String RoomServiceSessionGuestPre = "_ROOM_GUEST_LIST";
        /// <summary>
        /// 接待单sessionkey的前缀
        /// </summary>
        public static String RoomServiceSessionOrderPre = "_ROOM_RECEIVE_ORDER";
        /// <summary>
        /// 接待单会员信息
        /// </summary>
        public static String RoomServiceSessionMemberPre = "_ROOM_MEMBER";
        /// <summary>
        /// 操作跳转提示字符
        /// </summary>
        public static String RedirectMessage = "redirectMessage";

        /// <summary>
        /// 将https的路径修改为http
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static String ChangetHttpsToHttps(String host)
        {
            if (host.ToLower().IndexOf("https") > -1)
            {
                if (host.Split(':').Length == 2)
                {
                    host += ":443";
                }
                host = host.Replace(host.Substring(0, 5), "http").Replace(Constant.HttpsPort.ToString(), Constant.HttpPort.ToString());

            }
            return host;
        }



        public static bool Check(String obj, out String Message)
        {

            var vkeyWords = new System.Text.RegularExpressions.Regex("[,\\`,\\~,\\!,\\@,\\#,\\$,\\%,\\^,\\+,\\*,\\&,\\\\,\\/,\\?,\\|,\\:,\\.,\\<,\\>,\\{,\\},\\(,\\),\\'',\\;,\\=,\"]");
            if (vkeyWords.IsMatch(obj))
            {
                Message = ("禁止输入特殊字符！");
                return false;
            }
            Message = null;
            return true;
        }

        /// <summary>
        /// 验证密码规则
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool VerifyPassword(String obj, out String Message)
        {

            var vkeyWords = new System.Text.RegularExpressions.Regex("(?!^[0-9]+$|^[a-zA-Z]+$|^[~`!@#$%^&*()_+-=|]+$)^[0-9a-zA-Z~`!@#$%^&*()_+-=|]{6,20}");
            if (!vkeyWords.IsMatch(obj))
            {
                Message = ("密码不符合规则,密码长度在6~20位，且必须包含字母和数字");
                return false;
            }
            Message = null;
            return true;
        }


        private static void SetObjectPropertyValue(object targetObj, string propertyName, object value)
        {
            System.Reflection.PropertyInfo propertyInfo = targetObj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (value != DBNull.Value)
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(targetObj, Enum.ToObject(propertyInfo.PropertyType, value), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(targetObj, value, null);
                    }
                }
            }
        }

        public static void ReaderToObject<T>(IDataReader reader, T targetObj)
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string strName = reader.GetName(i);
                object objValue = reader.GetValue(i);
                SetObjectPropertyValue(targetObj, strName, objValue);
            }
        }




        /// <summary>
        /// 将 一个 List 拼接成 KEY1=VAL1,KEY2=VAL2,KEY3=VAL3 的形式
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string DictionaryJoin<K, V>(this ICollection<KeyValuePair<K, V>> dict, string separator = ",")
        {
            StringBuilder bufStr = new StringBuilder();
            foreach (KeyValuePair<K, V> kv in dict)
            {
                if (bufStr.Length > 0) bufStr.Append(separator);
                bufStr.Append(kv.Key);
                bufStr.Append("=");
                bufStr.Append(kv.Value);
            }
            return bufStr.ToString();
        }

        /// <summary>
        /// 得到 一个 arrByte 的 一部分
        /// </summary>
        /// <param name="pm_Byte"></param>
        /// <param name="pm_Offset"></param>
        /// <param name="pm_Len"></param>
        /// <returns></returns>
        public static byte[] ByteArraySub(byte[] pm_Byte, int pm_Offset, int pm_Len)
        {
            if (pm_Byte == null) throw new ArgumentNullException("Byte[]");
            if ((pm_Offset < 0) || ((pm_Len + pm_Offset) > pm_Byte.Length)) throw new ArgumentOutOfRangeException("Offset Len");

            byte[] lc_Result = new byte[pm_Len];
            Array.Copy(pm_Byte, pm_Offset, lc_Result, 0, pm_Len);
            return lc_Result;
        }

        public static byte[] ByteArrayRight(byte[] pm_Byte, int pm_Len)
        {
            int lc_Offset = pm_Byte.Length - pm_Len;
            return ByteArraySub(pm_Byte, lc_Offset, pm_Len);
        }

        public static byte[] ByteArrayLeft(byte[] pm_Byte, int pm_Len)
        {
            return ByteArraySub(pm_Byte, 0, pm_Len);
        }

        public static Dictionary<string, string> SplitToKeyValueDictionary(string str, char c1 = ',', char c2 = '=')
        {
            Dictionary<string, string> rst = new Dictionary<string, string>();
            SplitToKeyValueDictionary(str, rst, c1, c2);
            return rst;
        }

        public static void SplitToKeyValueDictionary(string str, IDictionary<string, string> lstKV, char c1 = ',', char c2 = '=')
        {
            if (lstKV == null) throw new ArgumentNullException("Dictionary");
            if (lstKV.IsReadOnly) throw new ArgumentException("Dictionary is ReadOnly");
            if (string.IsNullOrEmpty(str)) return;

            string[] lst = str.Split(new char[] { c1 });
            for (int i = 0; i < lst.Length; i++)
            {
                string kv = lst[i].Trim();
                int p = kv.IndexOf(c2);
                if (p <= 0) throw new ArgumentException("没有找到 key=value 结构");
                lstKV[kv.Substring(0, p)] = kv.Substring(p + 1);
            }
        }

        public static void WriteToXmlWriteAttribute(XmlWriter xw, string key, object val)
        {
            if ((!DBNull.Value.Equals(val)) && (null != val))
            {
                bool isOk = false;
                string str = string.Empty;

                if (val is DateTime)
                {
                    DateTime dt = Convert.ToDateTime(val);
                    if (dt == DateTime.MinValue)
                    {
                        str = null;
                        isOk = true;
                    }
                    else if (dt.Year <= 1900)
                    {
                        str = string.Empty;
                        isOk = true;
                    }
                }

                if (!isOk) str = val.ToString();
                if (str != null)
                {
                    string p = str.ToString();
                    xw.WriteAttributeString(key, p);
                }
            }
        }


        public static void WriteIDListToXml(XmlWriter xw, string ids, string rowTag, string idTag)
        {
            string[] arrID = ids.Split(new char[] { ',' });

            WriteIDListToXml(xw, arrID, null, rowTag, idTag, false);
        }

        public static void WriteIDListToXml(XmlWriter xw, IList<string> lstID, string setTag, string rowTag, string idTag, bool writeSetTag)
        {
            if (writeSetTag)
            {
                if (string.IsNullOrEmpty(setTag))
                {
                    setTag = "rowSet";
                }
                xw.WriteStartElement(setTag);
            }

            if (string.IsNullOrEmpty(rowTag))
            {
                rowTag = "row";
            }

            if (string.IsNullOrEmpty(idTag))
            {
                rowTag = "id";
            }

            foreach (string idstr in lstID)
            {
                if (!string.IsNullOrEmpty(idstr))
                {
                    xw.WriteStartElement(rowTag);
                    xw.WriteAttributeString(idTag, idstr);
                    xw.WriteEndElement();
                }
            }

            if (writeSetTag)
            {
                xw.WriteEndElement();
            }
        }

        public static string XmlToJoinStr(string xml)
        {
            return XmlToJoinStr(xml, string.Empty, string.Empty, string.Empty);
        }

        public static string XmlToJoinStr(string xml, string tagRow, string tagField, string strSep)
        {
            if (string.IsNullOrEmpty(tagRow)) tagRow = "t";
            if (string.IsNullOrEmpty(tagField)) tagField = "f";
            if (string.IsNullOrEmpty(strSep)) strSep = ",";

            StringBuilder bufStr = new StringBuilder(64);
            using (XmlReader xr = XmlReader.Create(new StringReader(xml)))
            {

                while (xr.Read())
                {
                    if (xr.Name == tagRow)
                    {
                        xr.MoveToFirstAttribute();
                        if (xr.Name == tagField)
                        {
                            if (bufStr.Length > 0) bufStr.Append(strSep);
                            bufStr.Append(xr.Value);
                        }
                    }
                }
            }
            return bufStr.ToString();
        }

        public static void XmlNodeToDict(XPathNavigator nav, string path, StringDictionary dct)
        {
            if (nav == null) return;
            XmlNodeToDict(nav.SelectSingleNode(path), dct);
        }

        public static void XmlNodeToDict(XPathNavigator nav, StringDictionary dct)
        {
            if (nav == null) return;

            if (nav.HasChildren)
            {
                XPathNavigator navNew = nav.Clone();
                if (navNew.MoveToFirstChild())
                {
                    do
                    {
                        string key = navNew.Name;
                        string val = navNew.Value;
                        dct.Add(key, val);

                    } while (navNew.MoveToNext());
                }
            }
        }


        public static bool JoinStrMatch(string strJoin, string strSub)
        {
            return JoinStrMatch(strJoin, strSub, ",");
        }

        public static bool JoinStrMatch(string strJoin, string strSub, string strSep)
        {
            if (string.Equals(strJoin, strSub)) return true;
            if (string.IsNullOrEmpty(strJoin)) return false;
            if (string.IsNullOrEmpty(strSub)) return true;

            if (strJoin.IndexOf(strSep + strSub + strSep) >= 0) return true;

            if (strJoin.StartsWith(strSub + strSep)) return true;
            if (strJoin.EndsWith(strSep + strSub)) return true;
            return false;
        }

        public static XmlDocument UnCompressXML(byte[] buf)
        {
            using (MemoryStream ms = new MemoryStream(buf))
            {
                GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, false);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(zs);

                ms.Close();
                return xmldoc;
            }
        }

        public static string UnCompressToString(byte[] buf)
        {
            return Encoding.UTF8.GetString(UnCompressToBuffer(buf));
        }

        public static byte[] UnCompressToBuffer(byte[] buf)
        {
            using (MemoryStream ms = new MemoryStream(buf))
            {
                GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, false);

                byte[] bufTemp = new byte[10240];

                MemoryStream msDest = new MemoryStream();
                while (true)
                {
                    int r = zs.Read(bufTemp, 0, bufTemp.Length);
                    if (r <= 0) break;
                    msDest.Write(bufTemp, 0, r);
                }
                return msDest.ToArray();
            }
        }

        #region 读取文件的内容
        public static string GetFileContent(string filepath)
        {
            //string strfile = "~/html/disclaimer.txt";
            string strout;
            strout = "";

            string ss = System.AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + filepath))
            {
                StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + filepath);
                String input = sr.ReadToEnd();
                sr.Close();
                strout = input;
            }
            return strout;
        }

        #endregion

        /// <summary>
        /// 取整数的某个位上的数字 IntRightPos(12345, 1) = 4
        /// </summary>
        /// <param name="val"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static int IntRightPos(int val, int pos)
        {
            int rst = 0;
            switch (pos)
            {
                case 0: rst = val % 10; break;
                case 1: rst = (val / 10) % 10; break;
                case 2: rst = (val / 100) % 10; break;
                case 3: rst = (val / 1000) % 10; break;
                case 4: rst = (val / 10000) % 10; break;
                case 5: rst = (val / 100000) % 10; break;
                case 6: rst = (val / 1000000) % 10; break;
                case 7: rst = (val / 10000000) % 10; break;
                case 8: rst = (val / 100000000) % 10; break;
                case 9: rst = (val / 1000000000) % 10; break;
            }
            return rst;
        }

        /// <summary>
        /// 根据第一个字符拆分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] SplitByFirstChar(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return new string[0];

            return str.Substring(1).Split(str[0]);
        }


        public static string GetDictString(IDictionary<String, String> dict, string key)
        {
            return GetDictValue<String>(dict, key, string.Empty);
        }
        public static T GetDictValue<T>(IDictionary<String, T> dict, String key, T def) where T : class
        {
            T rst = null;
            if (dict != null)
            {
                if (!dict.TryGetValue(key, out rst))
                {
                    rst = def;
                }
            }

            if (rst == null) rst = def;
            return rst;
        }

        public static string GetAppSetting(string key)
        {
            string rst = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(rst)) rst = string.Empty;
            return rst;
        }

        public static string GetAppSetting(string key, string val)
        {
            string rst = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(rst)) rst = val;
            return rst;
        }

        public static int GetAppSetting(string key, int val)
        {
            int rst = val;
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], out rst))
            {
                rst = val;
            }
            return rst;
        }

        public enum FixUrlType { Image, Link };
        public static string FixUrl(FixUrlType typ, string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            if (url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase)) return url;

            StringBuilder buf = new StringBuilder();
            string fix = string.Empty;
            if (typ == FixUrlType.Image)
            {
                fix = GetAppSetting("UrlFix.Image");
            }
            else
            {
                fix = GetAppSetting("UrlFix.Link");
            }

            buf.Append(fix);
            if (!url.StartsWith(@"/")) buf.Append(@"/");
            buf.Append(url);
            return buf.ToString();
        }

        public static T JsonToObject<T>(string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }




        #region 计算距离
        private const double EARTH_RADIUS = 6378.137; //地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// xty 计算两点之间经纬度
        /// </summary>
        /// <param name="lat1">传入的纬度</param>
        /// <param name="lng1">传入的经度</param>
        /// <param name="lat2">某个元素的经度</param>
        /// <param name="lng2">某个元素的纬度</param>
        /// <returns>两点之间距离</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            try
            {
                double radLat1 = rad(lat1);
                double radLat2 = rad(lat2);
                double a = radLat1 - radLat2;
                double b = rad(lng1) - rad(lng2);
                double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                 Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
                s = s * EARTH_RADIUS;
                s = Math.Round(s * 10000) / 10000;
                return s;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        ///  拓展方法：根据两点距离判断是否在传入的范围内 
        /// </summary>
        /// <param name="hotelEntity">实体类型</param>
        /// <param name="lat1">纬度</param>
        /// <param name="lng1">经度</param>
        /// <param name="radius">范围</param>
        /// <returns>Bool</returns>
        public static bool GetDistance(this HotelEntity hotelEntity, double lat1, double lng1, double radius)
        {
            return GetDistance(lat1, lng1, (double)hotelEntity.Latitude, (double)hotelEntity.Longitude) <= radius;
        }

        #endregion

        public static Boolean CheckMobileFormat(string mobile)
        {
            String RegFormat = @"^1[0-9]{10}$";

            Regex re = new Regex(RegFormat);
            return re.IsMatch(mobile);
        }

        public static string SubDisplayString(this string stringToSub,int subCount)
        {
            return string.IsNullOrEmpty(stringToSub) ? string.Empty :
                stringToSub.Length > subCount ? stringToSub.Substring(0, subCount) + "......" : stringToSub;
        }
        public static string GetSortedObjectProperty(this object obj, string[] strList)
        {
            return string.Join("&", obj.GetType().GetProperties().
                Where(p => !strList.Contains(p.Name.ToLower())).
                Where(t=>t.GetValue(obj)!=null&&!string.IsNullOrWhiteSpace(t.GetValue(obj).ToString())).
                OrderBy(m => m.Name).Select(p => string.Format("{0}={1}", p.Name.ToLower(), p.GetValue(obj)
               )));
        }
    }

}
