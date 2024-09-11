

namespace Deal
{
    /// <summary>
    /// 场景的枚举
    /// </summary>
    public enum SceneEnum
    {
        none,
        main,
        dungeon,
        pvp,
    }

    //public class SceneDefine
    //{
    //    public static string LandScene = "main";
    //    public static string DungeonScene = "dungeon";
    //}

    public class DataDefine
    {
        public static string UserData = "Data_UserData";
        public static string MapData = "Data_MapData";
        public static string DungeonData = "Data_DungeonData";
    }

    public class TableDefine
    {
        public static string MapData = "Table_MapData";
        public static string ConfigMrg = "Table_ConfigMrg";
    }

    public class NumDefine
    {
        public static float FallRadius = 1.0f;
        // 地图大小
        public static int MapTileWidth = 200;
        public static int MapTileHeight = 200;

        // 地图的边界
        public static int MapXLeft = -60;
        public static int MapXRight = 70;
        public static int MapYTop = 70;
        public static int MapYBotom = -30;
    }

    public class NameDefine
    {
        public static string Wood = "Wood";
        public static string Stone = "Stone";
        public static string AppleTree = "AppleTree";
        public static string Pumpkin = "Pumpkin";
        public static string Farm = "Farm";
        public static string Grain = "Grain";
        public static string Cactus = "Cactus";
        public static string WinterWood = "WinterWood";
        public static string DeadWood = "DeadWood";
        public static string Bamboo = "Bamboo";
        public static string Orange = "Orange";
        public static string Carrot = "Carrot";
        public static string CarrotPatch = "CarrotPatch";
        public static string OrangeTree = "OrangeTree";
        public static string Cotton = "Cotton";
        public static string CottonFarm = "CottonFarm";
    }

    public class EventDefine
    {
        // 场景的加载进度
        public static string EVENT_SCENE_LOAD_PROGRESS = "SCENE_LOAD_PROGRESS";
        public static string EVENT_ACTIVITY_SEVEN_SIGN = "EVENT_ACTIVITY_SEVEN_SIGN";
        public static string EVENT_ACTIVITY_DAILY_TASK = "EVENT_ACTIVITY_DAILY_TASK";

    }


    public class UrlDefine
    {
        //
        public static string url_wxlogin = "/account/v1/wxlogin";
        public static string url_devicelogin = "/account/v1/devicelogin";
        public static string url_wxUserinfoSave = "/account/v1/save";
        public static string url_wxUserinfoModify = "/account/v1/modify";
        public static string url_info = "/account/v1/info";
        public static string url_creatOrder = "/mall/v1/order/create";
        public static string url_OrderList = "/mall/v1/order/list";
        public static string url_mainbox = "/mail/v1/mailbox";
        public static string url_mailReceive = "/mail/v1/receive";

        public static string url_arenaRankInfo = "/arena/v1/rank/info";
        public static string url_arenaCombatInfo = "/arena/v1/combat/info?player={0}";
        public static string url_arenaCombat = "/arena/v1/combat/save";
        public static string url_arenaPkWin = "/arena/v1/pk/challengewin";

        public static string url_userSave = "/account/v1/archives/save";
        public static string url_userLoad = "/account/v1/archives/info";

        // 统计
        public static string url_stats = "/data/v2/stats";
        //public static string url_stats = "/data/v1/stats";
        public static string url_version = "/version/v1/get?version={0}&os={1}";

        //保存玩家附属信息
        public static string url_attrSave = "/account/v1/attr/save";
        public static string url_RankList = "/rank/v1/list";


    }
}
