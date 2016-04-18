using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Common
{
    public static partial class PubFun
    {

        public static T IsNull<T>(T val, T def) where T : class
        {
            if (Convert.IsDBNull(val)) return def;
            if (val == null) return def;

            return val;
        }

        public static int BoolToInt(this bool flag)
        {
            return flag ? 1 : 0;
        }

        public static bool IntToBool(this int flag)
        {
            if (flag == 0) return false;
            return true;
        }

        public static T ConvertDef<T>(object obj, T defValue)
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return defValue;
            }
        }

        public static string ConvertToString(this object obj)
        {
            return ConvertToString(obj, string.Empty);
        }

        public static string ConvertToString(this object obj, string def)
        {
            if (Convert.IsDBNull(obj)) return def;
            if (obj == null) return def;

            if (obj is DateTime)
            {
                DateTime dt = Convert.ToDateTime(obj);
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return obj.ToString();
            }
        }

        public static int ConvertToInt(this string str, int def = 0)
        {
            int rst;
            if (!Int32.TryParse(str, out rst)) rst = def;
            return rst;
        }

        public static int ConvertToInt(this object obj, int def = 0)
        {
            if (Convert.IsDBNull(obj)) return def;
            if (obj == null) return def;

            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return def;
            }
        }

        public static double ConvertToDouble(this string str, double def = 0)
        {
            double rst;
            if (!double.TryParse(str, out rst)) rst = def;
            return rst;
        }

        public static double ConvertToDouble(this object obj, double def = 0)
        {
            if (Convert.IsDBNull(obj)) return def;
            if (obj == null) return def;

            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return def;
            }
        }

        public static decimal ConvertToDecimal(this string str, decimal def = 0)
        {
            decimal rst;
            if (!decimal.TryParse(str, out rst)) rst = def;
            return rst;
        }

        public static decimal ConvertToDecimal(this object obj, decimal def = 0)
        {
            if (Convert.IsDBNull(obj)) return def;
            if (obj == null) return def;

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return def;
            }
        }

        public static DateTime ConvertToDateTime(this object obj)
        {
            return ConvertToDateTime(obj, DateTime.MinValue);
        }

        public static DateTime ConvertToDateTime(this object obj, DateTime def)
        {
            DateTime rst;
            try
            {
                rst = Convert.ToDateTime(obj);
            }
            catch
            {
                rst = def;
            }
            return rst;
        }


        public static string GetEnumName<T>(this string enumType) where T : new()
        {
            var name = "";
            var type = typeof(T);
            foreach (var info in type.GetMembers())
            {
                if (info.Name == enumType)
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(info))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            name = ((DescriptionAttribute)attr).Description;
                            break;
                        }
                    }
                }
            }

            return name;
        }
    }
}
