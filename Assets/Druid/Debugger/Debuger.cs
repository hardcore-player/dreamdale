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
using System.IO;

namespace Druid
{
    public class Debuger
    {
        public static bool EnableLog;
        public static bool EnableLogLoop;
        public static bool EnableTime = true;
        public static bool EnableSave = false;
        public static bool EnableStack = false;
        public static string LogFileDir = "";
        public static string LogFileName = "";
        public static string Prefix = "> ";
        public static StreamWriter LogFileWriter = null;

        public static void LogError(string lOG_TAG, string v)
        {
            throw new NotImplementedException();
        }

        public static void Init()
        {
#if UNITY_2018_1_OR_NEWER
            LogFileDir = UnityEngine.Application.persistentDataPath + "/DebugerLog/";
#else
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            LogFileDir = path + "/DebugerLog/";
#endif
        }

        private static void Internal_Log(string msg, object context = null)
        {
#if UNITY_2018_1_OR_NEWER
            UnityEngine.Debug.Log(msg, (UnityEngine.Object)context);
#else
            Console.WriteLine(msg);
#endif
        }

        private static void Internal_LogWarning(string msg, object context = null)
        {
#if UNITY_2018_1_OR_NEWER
            UnityEngine.Debug.LogWarning(msg, (UnityEngine.Object)context);
#else
            Console.WriteLine(msg);
#endif
        }

        private static void Internal_LogError(string msg, object context = null)
        {
#if UNITY_2018_1_OR_NEWER
            UnityEngine.Debug.LogError(msg, (UnityEngine.Object)context);
#else
            Console.WriteLine(msg);
#endif
        }


        //----------------------------------------------------------------------
        [Conditional("ENABLE_LOG_LOOP")]
        public static void LogLoop(string tag, string methodName, string message = "")
        {
            if (!Debuger.EnableLogLoop)
            {
                return;
            }

            message = GetLogText(tag, methodName, message);
            Internal_Log(Prefix + message);
            LogToFile("[I]" + message);
        }

        [Conditional("ENABLE_LOG_LOOP")]
        public static void LogLoop(string tag, string methodName, string format, params object[] args)
        {
            if (!Debuger.EnableLogLoop)
            {
                return;
            }

            string message = GetLogText(tag, methodName, string.Format(format, args));
            Internal_Log(Prefix + message);
            LogToFile("[I]" + message);
        }

        [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
        public static void Log(string tag, string methodName, string message = "")
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            message = GetLogText(tag, methodName, message);
            Internal_Log(Prefix + message);
            LogToFile("[I]" + message);
        }

        [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
        public static void Log(string tag, string methodName, string format, params object[] args)
        {
            if (!Debuger.EnableLog)
            {
                return;
            }

            string message = GetLogText(tag, methodName, string.Format(format, args));
            Internal_Log(Prefix + message);
            LogToFile("[I]" + message);
        }

        public static void LogDeserObj(string tag, string methodName, string message)
        {
            message = GetLogText(tag, methodName, message);
            Internal_Log(Prefix + message);
            LogToFile("[E]" + message, true);
        }

        public static void LogError(string tag, string methodName, string message)
        {
            message = GetLogText(tag, methodName, message);
            Internal_LogError(Prefix + message);
            LogToFile("[E]" + message, true);
        }

        public static void LogError(string tag, string methodName, string format, params object[] args)
        {
            string message = GetLogText(tag, methodName, string.Format(format, args));
            Internal_LogError(Prefix + message);
            LogToFile("[E]" + message, true);
        }


        public static void LogWarning(string tag, string methodName, string message)
        {
            message = GetLogText(tag, methodName, message);
            Internal_LogWarning(Prefix + message);
            LogToFile("[W]" + message);
        }

        public static void LogWarning(string tag, string methodName, string format, params object[] args)
        {
            string message = GetLogText(tag, methodName, string.Format(format, args));
            Internal_LogWarning(Prefix + message);
            LogToFile("[W]" + message);
        }



        //----------------------------------------------------------------------
        private static string GetLogText(string tag, string methodName, string message)
        {
            string str = "";
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                str = now.ToString("HH:mm:ss.fff") + " ";
            }

            str = str + tag + "::" + methodName + "() " + message;
            return str;
        }



        //----------------------------------------------------------------------
        internal static string CheckLogFileDir()
        {
            if (string.IsNullOrEmpty(LogFileDir))
            {
                //该行代码无法在线程中执行！
                try
                {
#if UNITY_2018_1_OR_NEWER
                    LogFileDir = UnityEngine.Application.persistentDataPath + "/DebugerLog/";
#else
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    LogFileDir = path + "/DebugerLog/";
#endif
                }
                catch (Exception e)
                {
                    Internal_LogError("Debuger::CheckLogFileDir() " + e.ToString() + e.StackTrace);
                    return "";
                }
            }

            try
            {
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }
            }
            catch (Exception e)
            {
                Internal_LogError("Debuger::CheckLogFileDir() " + e.ToString() + e.StackTrace);
                return "";
            }

            return LogFileDir;
        }

        internal static string GenLogFileName()
        {
            DateTime now = DateTime.Now;
            string filename = now.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25
            filename = filename.Replace("-", "_");
            filename = filename.Replace(":", "_");
            filename = filename.Replace(" ", "");
            filename += ".log";

            return filename;
        }

        private static void LogToFile(string message, bool EnableStack = false)
        {
            if (!EnableSave)
            {
                return;
            }

            if (LogFileWriter == null)
            {
                LogFileName = GenLogFileName();
                LogFileDir = CheckLogFileDir();
                if (string.IsNullOrEmpty(LogFileDir))
                {
                    return;
                }

                string fullpath = LogFileDir + LogFileName;
                try
                {
                    LogFileWriter = File.AppendText(fullpath);
                    LogFileWriter.AutoFlush = true;
                }
                catch (Exception e)
                {
                    LogFileWriter = null;
                    Internal_LogError("Debuger::LogToFile() " + e.ToString() + e.StackTrace);
                    return;
                }
            }

            if (LogFileWriter != null)
            {
                try
                {
                    LogFileWriter.WriteLine(message);
#if UNITY_2018_1_OR_NEWER
                    if ((EnableStack || Debuger.EnableStack))
                    {
                        LogFileWriter.WriteLine(UnityEngine.StackTraceUtility.ExtractStackTrace());
                    }
#endif
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
