using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace YeahTVApi.Common
{
    public static class ObjectConvert
    {
        /// <summary>
        /// 返回一个'a','b','c'的格式
        /// </summary>
        /// <param name="source">一个字符串</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string FormatAsIn(this string source, char splitChar = ',')
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                var arry = source.Split(splitChar);

                var sb = new StringBuilder();

                foreach (var str in arry)
                {
                    sb.AppendFormat("'{0}',", str);
                }
                sb = sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return string.Empty;
        }

        public static T? GetValueOrNull<T>(this object columnValue) where T : struct
        {
            if (!(columnValue is DBNull || columnValue == null))
            {
                return (T)Convert.ChangeType(columnValue, typeof(T));
            }

            return null;
        }

        public static long ToLongInt(this object columnValue)
        {
            if (!(columnValue is DBNull || columnValue == null))
            {
                return Convert.ToInt64(columnValue);
            }
            return 0;
        }

        public static int ToInt(this object columnValue)
        {
            if (!(columnValue is DBNull || columnValue == null))
            {
                return Convert.ToInt32(columnValue);
            }
            return 0;
        }

        public static int ToInt(this string value)
        {
            var i = 0;
            if (value == null) return i;
            int.TryParse(value, out i);
            return i;
        }

        public static decimal ToDecimal(this object columnValue, int decimals = 0)
        {
            if (!(columnValue is DBNull || columnValue == null))
            {
                var d = Convert.ToDecimal(columnValue);
                return decimals > 0 ? decimal.Round(d, decimals) : d;
            }
            return 0;
        }

        /// <summary>
        /// 返回时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this object obj, bool defaultTime)
        {
            return ToDateTimeStr(obj, "", defaultTime);
        }

        /// <summary>
        /// 返回时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this object obj, string format = DateFormate.yyyyMMddHHmmss,
            bool defaultTime = false)
        {
            var time = new DateTime(1900, 1, 1);
            if (obj == null && defaultTime)
            {
                return time.ToString(format);
            }
            if (obj == null)
                return string.Empty;
            //
            //            if (defaultTime)
            //            {
            //                return format == "" ? DateTime.Now.ToString() : DateTime.Now.ToString(format);
            //            }
            if (DateTime.TryParse(obj.ToString(), out time))
            {
                return time.ToString(format);
            }
            return "";
        }

        /// <summary>
        /// 返回时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj)
        {
            var dt = new DateTime(1900, 1, 1);
            if (obj is DBNull)
            {
                return dt;
            }
            DateTime.TryParse(obj.ToString(), out dt);
            return dt;
        }

        //        public static DateTime? ToDateTime(this object obj)
        //        {
        //
        //            if (obj is DBNull || obj==null)
        //            {
        //                return null;
        //            }
        //            DateTime dt;
        //            if (DateTime.TryParse(obj.ToString(), out dt))
        //            {
        //                return dt;
        //            }
        //            return null;
        //        }
        /// <summary>
        /// 重写toString
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值为空</param>
        /// <returns></returns>
        public static string CastToString(this object obj, string defaultValue = "")
        {
            if (obj is DBNull || obj == null)
            {
                return defaultValue;
            }
            return obj.ToString().Trim();
        }

        //        /// <summary>
        //        /// 返回Deciaml
        //        /// </summary>
        //        /// <param name="obj"></param>
        //        /// <returns></returns>
        //        public decimal ConvetToDecimal(object obj)
        //        {
        //            if (obj == null || obj.ToString().Trim() == "") return 0;
        //            decimal num = 0;
        //            if (decimal.TryParse(obj.ToString(), out num))
        //                return num;
        //            return 0;
        //        }

        //        /// <summary>
        //        /// 保留几位
        //        /// </summary>
        //        /// <param name="obj"></param>
        //        /// <param name="num"></param>
        //        /// <returns></returns>
        //        public decimal ConvetToDecimal(object obj, int count)
        //        {
        //            if (obj == null || obj.ToString().Trim() == "") return 0;
        //            decimal num = 0;
        //            if (decimal.TryParse(obj.ToString(), out num))
        //                return decimal.Round(num, count);
        //            return 0;
        //        }
        //
        //        public string ConvetToDecimalStr(object obj, string format, string defaultValue)
        //        {
        //            if (obj != null && obj.ToString().Trim() == "-1") return defaultValue;
        //            string value = "0";
        //            if (defaultValue != null && defaultValue != "")
        //                value = defaultValue;
        //            if (obj == null || obj.ToString().Trim() == "") return value;
        //            if (obj.ToString() == "0") return value;
        //            decimal num = 0;
        //            decimal.TryParse(obj.ToString(), out num);
        //            if (num == 0) return value;
        //            else return num.ToString(fomat);
        //        }


        //        public int ConvetToInt(object obj)
        //        {
        //            if (obj == null || obj.ToString().Trim() == "") return 0;
        //            int num = 0;
        //            if (int.TryParse(obj.ToString(), out num))
        //                return num;
        //            return 0;
        //        }

        public static bool ToBool(this object obj)
        {
            if (obj == null || obj.ToString().Trim() == "") return false;
            return Convert.ToBoolean(obj);
        }

        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null) return true;
            return string.IsNullOrEmpty(obj.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }


        /// <summary>
        /// Array 数组字符串从 a,b,c 转为 'a','b','c'
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToArrayFormart(this string source)
        {
            if (source != null)
            {
                string[] array = source.Split(',');
                source = string.Empty;
                foreach (var s in array)
                {
                    if (!string.IsNullOrEmpty(source))
                    {
                        source += ",";
                    }
                    source += "'" + s + "'";
                }
            }
            return source;
        }



        /// <summary>
        /// 将当前对象的属性值复制到目标对象，使用浅表
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象，如果为空，将生成一个</param>
        /// <param name="filter">字段过滤器</param>
        /// <returns>复制过后的目标对象</returns>
        public static T CopyTo<T>(this object source, T target = null, string[] filter = null) where T : class,new()
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                target = new T();
            ModuleCast.GetCast(source.GetType(), typeof(T)).Cast(source, target, filter);
            return target;
        }


        

    }
}