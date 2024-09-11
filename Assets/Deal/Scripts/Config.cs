
namespace Deal
{
    public class Config
    {
        // 0 : 正式服， 1 ： 测试服
        public static int DebugMode = 1;

        // 防沉迷
        public static bool AntiAddiction = true;

        public static string VersionCode = "1.0.2";

        // 版本的最大任务（包含这个）
        public static int VersionMaxTaskId = 200;

        public static string HttpUrl = "";

        // 广告冷却时间
        public static int VideoCdSecond = 5;

        public static bool IsDebug()
        {
            return DebugMode != 0;
        }
    }

}
