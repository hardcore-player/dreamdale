using System;
using System.Collections.Generic;
using UnityEngine;

namespace Druid.Utils
{
    public class TimeUtils
    {
        public static long TimeNowMilliseconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        public static long TimeNowSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <returns></returns>
        public static DateTime DateTimeFromSeconds(double st)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = startTime.AddMilliseconds(st * 1000);//st为传入的时间戳

            return dt;
        }

        public static string YearMonthDay()
        {
            //获取当前年月日
            var year = DateTime.Now.Year.ToString().Substring(2, 2);
            var month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var day = DateTime.Now.Day.ToString().PadLeft(2, '0');

            return year + month + day;
        }

        public static string SecondsFormat(long second)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(second));

            string hs = ts.Hours >= 10 ? ts.Hours.ToString() : "0" + ts.Hours;
            string ms = ts.Minutes >= 10 ? ts.Minutes.ToString() : "0" + ts.Minutes;
            string ss = ts.Seconds >= 10 ? ts.Seconds.ToString() : "0" + ts.Seconds;

            if (ts.Hours > 0)
            {
                return hs + ":" + ms + ":" + ss;
            }

            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                return ms + ":" + ss;
            }

            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                return ms + ":" + ss;
            }

            return "";
        }
    }
}

