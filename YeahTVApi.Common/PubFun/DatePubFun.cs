using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Common
{
    public static partial class PubFun
    {
        /// <summary>
        /// 将时间转化为特殊格式：yyyy.MM.dd 上午 HH:mm
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetFormatDate(DateTime dateTime)
        {
            var date = dateTime.Hour;
            var str = "";
            if (date > 12) { str = " PM "; }
            else if (date < 12) { str = " AM "; }
            return dateTime.ToString("yyyy.MM.dd") + str + dateTime.ToString("HH:mm");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns>返回yyyyMMddHHmmss格式的字符串</returns>
        public static String GetDateTimeString(this DateTime time)
        {
            return time.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns>返回yyyyMMddHHmmss格式的字符串</returns>
        public static String GetShortDateTimeString(this DateTime time)
        {
            return time.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns>返回yyyyMMddHHmmss格式的长整型</returns>
        public static long GetDateTimeLong(this DateTime time)
        {
            return long.Parse(GetDateTimeString(time));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns>返回yyyyMMddHHmmss格式的精度型</returns>
        public static decimal GetDateTimeDecimal(this DateTime time)
        {
            return decimal.Parse(GetDateTimeString(time));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime">需要转换的日期时间字符串</param>
        /// <returns>返回去除/、-、：分隔符的精度型</returns>
        public static decimal GetStringDateTimeDecimal(this string datetime)
        {
            datetime = datetime.Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "");
            return decimal.Parse(datetime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <returns>返回制定fromat格式的字符串</returns>
        public static String GetDateTimeString(this DateTime time, string fromat)
        {
            return time.ToString(fromat);
        }
        /// <summary>
        /// 将日期字符串转换成指定格式日期字符串
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sourceformat">源格式</param>
        /// <param name="targetformat">目标格式</param>
        /// <returns>返回指定格式字符串</returns>
        public static DateTime? GetDateTime(this long time, string sourceformat = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceformat))
                    sourceformat = "yyyyMMddHHmmss";

                return DateTime.ParseExact(time.ToString(), sourceformat, global::System.Globalization.CultureInfo.CurrentCulture
                    , global::System.Globalization.DateTimeStyles.None);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据时间参数的长度来判断，容易出错，格式要规范
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetDateTime(string time)
        {
            string sourceformat = "";
            string targetformat = "";
            if (time.Length == 8)
            {
                sourceformat = "yyyyMMdd";
                targetformat = "yyyy-MM-dd";
            }
            else if (time.Length == 12)
            {
                sourceformat = "yyyyMMddHHmm";
                targetformat = "yyyy-MM-dd HH:mm";
            }
            else if (time.Length == 14)
            {
                sourceformat = "yyyyMMddHHmmss";
                targetformat = "yyyy-MM-dd HH:mm:ss";
            }

            try
            {
                if (sourceformat == "")
                    sourceformat = "yyyyMMddHHmmss";
                if (targetformat == "")
                    targetformat = "yyyy-MM-dd";
                time = DateTime.ParseExact(time, sourceformat, global::System.Globalization.CultureInfo.CurrentCulture
                    , global::System.Globalization.DateTimeStyles.None).ToString(targetformat);
            }
            catch (Exception ex)
            {
                time = "";
            }
            return time;
        }
        //计算两个日期日期的时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = "";
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();


                if (ts.Days != 0) dateDiff += ts.Days.ToString() + "天";
                if (ts.Hours != 0) dateDiff += ts.Hours.ToString() + "小时";
                if (ts.Minutes != 0) dateDiff += ts.Minutes.ToString() + "分钟";
                dateDiff += ts.Seconds.ToString() + "秒";

            }
            catch
            {

            }
            return dateDiff;
        }

        public static int WeekOfMonth(this DateTime dtSel, bool sundayStart)
        {

            //如果要判断的日期为1号，则肯定是第一周了
            if (dtSel.Day == 1)
                return 1;

            else
            {

                //得到本月第一天
                DateTime dtStart = new DateTime(dtSel.Year, dtSel.Month, 1);
                //得到本月第一天是周几
                int dayofweek = (int)dtStart.DayOfWeek;
                //如果不是以周日开始，需要重新计算一下dayofweek，详细风DayOfWeek枚举的定义
                if (!sundayStart)
                {

                    dayofweek = dayofweek - 1;
                    if (dayofweek < 0)

                        dayofweek = 7;
                }
                //得到本月的第一周一共有几天
                int startWeekDays = 7 - dayofweek;
                //如果要判断的日期在第一周范围内，返回1

                if (dtSel.Day <= startWeekDays)

                    return 1;

                else
                {

                    int aday = dtSel.Day + 7 - startWeekDays;

                    return aday / 7 + (aday % 7 > 0 ? 1 : 0);

                }

            }

        }
        public static List<DateTime> GetTimeRange(DateTime beginTime, DateTime endTime)
        {
            var listDays = new List<DateTime>();
            var dtDay = new DateTime();
            for (dtDay = endTime; dtDay.CompareTo(beginTime) >= 0; dtDay = dtDay.AddDays(-1))
            {
                listDays.Add(dtDay);
            }
            return listDays;
        }

        public static string GetLogsDate(this DateTime dateTime)
        {
            var gc = new GregorianCalendar();
           //int weekOfYear = gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int mongthOfYear = gc.GetMonth(dateTime);
            int dayOfMonth = gc.GetDayOfMonth(dateTime);
            return (dateTime.Year + mongthOfYear.ToString().PadLeft(2, '0') + dayOfMonth.ToString().PadLeft(2, '0')).ToString();
        }
    }
}
