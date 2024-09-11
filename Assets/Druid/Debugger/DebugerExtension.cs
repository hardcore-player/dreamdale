////////////////////////////////////////////////////////////////////
//                            _ooOoo_                             //
//                           o8888888o                            //
//                           88" . "88                            //
//                           (| ^_^ |)                            //
//                           O\  =  /O                            //
//                        ____/`---'\____                         //
//                      .'  \\|     |//  `.                       //
//                     /  \\|||  :  |||//  \                      //
//                    /  _||||| -:- |||||-  \                     //
//                    |   | \\\  -  /// |   |                     //
//                    | \_|  ''\---/''  |   |                     //
//                    \  .-\__  `-`  ___/-. /                     //
//                  ___`. .'  /--.--\  `. . ___                   //
//                ."" '<  `.___\_<|>_/___.'  >'"".                //
//              | | :  `- \`.;`\ _ /`;.`/ - ` : | |               //
//              \  \ `-.   \_ __\ /__ _/   .-` /  /               //
//        ========`-.____`-.___\_____/___.-`____.-'========       //
//                             `=---='                            //
//        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^      //
//            佛祖保佑       无BUG        不修改                     //
////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Druid
{
    public static class DebugerExtension
    {

        [Conditional("ENABLE_LOG_LOOP")]
        public static void LogLoop(this object obj, string message = "")
        {
            if (!Debuger.EnableLogLoop)
            {
                return;
            }

            Debuger.LogLoop(GetLogTag(obj), GetLogCallerMethod(), (string)message);
        }

        [Conditional("ENABLE_LOG_LOOP")]
        public static void LogLoop(this object obj, string format, params object[] args)
        {
            if (!Debuger.EnableLogLoop)
            {
                return;
            }

            Debuger.LogLoop(GetLogTag(obj), GetLogCallerMethod(), string.Format(format, args));
        }


        //----------------------------------------------------------------------

        [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
        public static void Log(this object obj, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            Debuger.Log(GetLogTag(obj), GetLogCallerMethod(), (string)message);
        }

        [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
        public static void Log(this object obj, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            Debuger.Log(GetLogTag(obj), GetLogCallerMethod(), string.Format(format, args));
        }

        public static void LogJson(this object obj, string tips, object msg)
        {
            String str = LitJson.JsonMapper.ToJson(msg);
            Debuger.LogDeserObj(GetLogTag(obj), GetLogCallerMethod(), tips + str);
        }

        public static void LogJson(this object obj, object msg)
        {
            try
            {
                String str = LitJson.JsonMapper.ToJson(msg);
                Debuger.LogDeserObj(GetLogTag(obj), GetLogCallerMethod(), str);
            }
            catch (Exception)
            {
                return;
            }
        }

        #region LogDeserObj 当前只解析一层
        public static void LogDeserObj(this object obj, object msg, string format = "")
        {
            try
            {
                Type t = msg.GetType();
                PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                string Str = "";
                StringBuilder strcntent = new StringBuilder(20);
                strcntent.Append(format + "\n");
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo mi = p.GetGetMethod();
                    if (mi != null && mi.IsPublic)
                    {
                        strcntent.AppendFormat("{0} : {1}\n", (string)p.Name, mi.Invoke(msg, new Object[] { }));
                    }
                }
                Str = strcntent.ToString();
                Debuger.LogDeserObj(GetLogTag(obj), GetLogCallerMethod(), (string)Str);
                strcntent.Capacity = 0;
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        public static void LogMessage(this object obj, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            Debuger.Log(GetLogTag(obj), GetLogCallerMethod(), string.Format("<color=green>{0}</color>", message));
        }

        public static void LogMessage(this object obj, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            Debuger.Log(GetLogTag(obj), GetLogCallerMethod(), string.Format(string.Format("<color=green>{0}</color>", format), args));
        }




        //----------------------------------------------------------------------

        public static void LogError(this object obj, string message)
        {
            Debuger.LogError(GetLogTag(obj), GetLogCallerMethod(), (string)message);
        }

        public static void LogError(this object obj, string format, params object[] args)
        {
            Debuger.LogError(GetLogTag(obj), GetLogCallerMethod(), string.Format(format, args));
        }



        //----------------------------------------------------------------------

        public static void LogWarning(this object obj, string message)
        {
            Debuger.LogWarning(GetLogTag(obj), GetLogCallerMethod(), (string)message);
        }


        public static void LogWarning(this object obj, string format, params object[] args)
        {
            Debuger.LogWarning(GetLogTag(obj), GetLogCallerMethod(), string.Format(format, args));
        }

        //----------------------------------------------------------------------



        //----------------------------------------------------------------------
        private static string GetLogTag(object obj)
        {
            FieldInfo fi = obj.GetType().GetField("LOG_TAG");
            if (fi != null)
            {
                return (string)fi.GetValue(obj);
            }

            return obj.GetType().Name;
        }

        private static Assembly ms_Assembly;
        private static string GetLogCallerMethod()
        {
            StackTrace st = new StackTrace(2, false);
            if (st != null)
            {
                if (null == ms_Assembly)
                {
                    ms_Assembly = typeof(Debuger).Assembly;
                }

                int currStackFrameIndex = 0;
                while (currStackFrameIndex < st.FrameCount)
                {
                    StackFrame oneSf = st.GetFrame(currStackFrameIndex);
                    MethodBase oneMethod = oneSf.GetMethod();

                    if (oneMethod.Module.Assembly != ms_Assembly)
                    {
                        return oneMethod.Name;
                    }

                    currStackFrameIndex++;
                }

            }

            return "";
        }
    }
}
